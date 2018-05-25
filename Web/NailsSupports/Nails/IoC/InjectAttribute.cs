using System;
using System.Reflection;

namespace NailsFramework.IoC
{
    /// <summary>
    ///   Defines that the decorated property has a dependency that should be injected.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class InjectAttribute : Attribute
    {
        /// <summary>
        ///   Initializes a new instance of the <see cref = "InjectAttribute" /> class.
        /// </summary>
        public InjectAttribute()
            : this(null)
        {
        }

        /// <summary>
        ///   Initializes a new instance of the <see cref = "InjectAttribute" /> class.
        /// </summary>
        /// <param name = "lemmingName">Name of the lemming.</param>
        public InjectAttribute(string lemmingName)
        {
            LemmingName = lemmingName;
        }

        public string LemmingName { get; private set; }

        public Injection InjectionFrom(PropertyInfo property)
        {
            if (property.PropertyType.IsValueType || property.PropertyType == typeof (string))
            {
                var appSettingName = LemmingName;
                return new ConfigurationInjection(property, appSettingName);
            }

            return new ReferenceInjection(property, LemmingName);
        }
    }
}