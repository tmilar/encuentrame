using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NailsFramework.Support;

namespace NailsFramework.IoC
{
    public class ReferenceInjection : Injection
    {
        private static readonly IEnumerable<IDependencyHandler> DependencyHandlers;

        static ReferenceInjection()
        {
            DependencyHandlers = Nails.Configuration
                .AssembliesToInspect
                .ConcreteSubclassesOf<IDependencyHandler>()
                .Select(x => (IDependencyHandler) Activator.CreateInstance(x));
        }

        public ReferenceInjection(PropertyInfo property, string referencedLemming)
            : base(property)
        {
            if (string.IsNullOrWhiteSpace(referencedLemming))
            {
                var dependencyHandler = DependencyHandlers.FirstOrDefault(x => x.Handles(property));

                if (dependencyHandler != null)
                    ReferencedLemming = dependencyHandler.LemmingNameFor(property);
            }
            else
            {
                ReferencedLemming = referencedLemming;
            }
        }

        public ReferenceInjection(PropertyInfo property) : this(property, null)
        {
        }

        public string ReferencedLemming { get; set; }

        public bool IsGeneric
        {
            get
            {
                return Property.PropertyType.IsGenericType &&
                       Property.PropertyType.GetGenericArguments()[0].IsGenericParameter;
            }
        }

        public override void Accept(IInjector injector)
        {
            injector.Inject(this);
        }

        public override Injection MakeGeneric(PropertyInfo genericProperty)
        {
            return new ReferenceInjection(genericProperty, ReferencedLemming);
        }

        public override string ToString()
        {
            return Property.DeclaringType.FullFriendlyName() + "." + Property.Name + " -> " + ReferencedLemming ?? "?";
        }
    }
}