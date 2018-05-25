using System;
using System.Reflection;
using NailsFramework.Support;

namespace NailsFramework.IoC
{
    public abstract class Injection
    {
        protected Injection(PropertyInfo property)
        {
            Property = property;
        }

        protected Injection(Type type, string property)
        {
            Property = type.GetProperty(property, BindingFlags.Public | BindingFlags.NonPublic);
        }

        public PropertyInfo Property { get; private set; }

        public string Owner
        {
            get { return ParentLemming != null ? ParentLemming.UniqueName : Property.DeclaringType.FullFriendlyName(); }
        }

        public Lemming ParentLemming { get; set; }

        public abstract void Accept(IInjector injector);

        public Injection MakeGeneric(Type genericDeclaringType)
        {
            return MakeGeneric(genericDeclaringType.GetProperty(Property.Name));
        }

        public abstract Injection MakeGeneric(PropertyInfo genericProperty);
    }
}