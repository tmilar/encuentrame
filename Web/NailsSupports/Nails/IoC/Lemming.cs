using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NailsFramework.Support;

namespace NailsFramework.IoC
{
    /// <summary>
    ///   Represents a Lemming.
    /// </summary>
    public class Lemming
    {
        private readonly Type concreteType;
        private readonly IList<Injection> injections;
        private Type[] typeParameters = new Type[0];

        private Lemming(Type type)
        {
            concreteType = type;
            var attribute = concreteType.Attribute<LemmingAttribute>(true);

            if (attribute == null)
            {
                Name = type.FullNameWithoutTypeParemeters();
                Singleton = true;
            }
            else
            {
                Name = string.IsNullOrEmpty(attribute.Name)
                           ? concreteType.FullNameWithoutTypeParemeters()
                           : attribute.Name;

                Singleton = attribute.Singleton;
            }

            injections = new List<Injection>();
            InjectAccordingTo(concreteType);

            if (!IsGenericDefinition)
                typeParameters = type.GetGenericArguments();
        }

        public string UniqueName
        {
            get
            {
                if (IsGenericDefinition)
                    throw new InvalidOperationException(
                        "Generic definitions cannot have an unique name because they doesn't have defined which type parameters will use.");

                return typeParameters.Length == 0
                           ? Name
                           : LemmingUniqueName.From(Name, typeParameters);
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether this <see cref = "LemmingAttribute" /> should be in cache.
        /// </summary>
        /// <value><c>true</c> if this instance should be in cache; otherwise, <c>false</c>.</value>
        public bool Singleton { get; set; }

        /// <summary>
        ///   Gets the concrete type.
        /// </summary>
        /// <value>The concrete type.</value>
        public Type ConcreteType
        {
            get { return concreteType; }
        }

        /// <summary>
        ///   Gets the name of the lemming.
        /// </summary>
        /// <value>The name of the lemming.</value>
        public string Name { get; set; }

        /// <summary>
        ///   Gets the injections.
        /// </summary>
        /// <value>The injections.</value>
        public IEnumerable<Injection> Injections
        {
            get { return injections; }
        }

        public bool IsGenericDefinition
        {
            get { return ConcreteType.IsGenericTypeDefinition; }
        }

        public bool Ignore { get; set; }

        /// <summary>
        ///   Creates a Lemming from the specified type.
        /// </summary>
        /// <param name = "type">The type.</param>
        /// <returns></returns>
        public static Lemming From(Type type)
        {
            return new Lemming(type);
        }

        /// <summary>
        ///   Creates a Lemming from the specified type.
        /// </summary>
        /// <typeparam name = "T">The concrete type</typeparam>
        /// <returns></returns>
        public static Lemming From<T>() where T : class
        {
            return From(typeof (T));
        }

        /// <summary>
        ///   Injects the specified injection.
        /// </summary>
        /// <param name = "injection">The injection.</param>
        /// <returns></returns>
        public Lemming Inject(Injection injection)
        {
            injections.Add(injection);
            injection.ParentLemming = this;
            return this;
        }

        private void InjectAccordingTo(Type type)
        {
            foreach (var property in type.Setters().Where(x => !x.IsStatic()))
            {
                var attribute = property.Attribute<InjectAttribute>();

                if (attribute == null)
                    continue;

                var injection = attribute.InjectionFrom(property);

                Inject(injection);
            }
        }

        public Lemming MakeGenericLemming(IEnumerable<Type> types)
        {
            var lemming = From(ConcreteType.MakeGenericType(types.ToArray()));

            lemming.injections.Clear();

            foreach (var injection in Injections)
                lemming.Inject(injection.MakeGeneric(lemming.ConcreteType));

            lemming.Name = Name;

            lemming.typeParameters = types.ToArray();

            return lemming;
        }

        public override string ToString()
        {
            return (Name ?? string.Empty) + " [" + ConcreteType.FullFriendlyName() + "]";
        }

        public void RemoveInjectionOf(PropertyInfo property)
        {
            var injection = Injections
                .SingleOrDefault(x => x.Property == property);

            if (injection != null)
                injections.Remove(injection);
        }
    }
}