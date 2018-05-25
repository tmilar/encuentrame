using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using NailsFramework.IoC;
using NailsFramework.Support;

namespace NailsFramework.Config
{
    public class LemmingConfigurator : BaseLemmingConfigurator
    {
        private readonly Type type;

        public LemmingConfigurator(Type type, Lemming lemming, IList<Lemming> registeredLemmings)
            : base(lemming, registeredLemmings)
        {
            this.type = type;
        }

        private PropertyInfo PropertyInfo(string property)
        {
            return type.GetProperty(property);
        }

        public LemmingConfigurator Value<TValue>(string property, TValue value)
        {
            Value(PropertyInfo(property), value);
            return this;
        }

        public LemmingConfigurator ValueFromConfiguration(string property, string appSetting = null)
        {
            ValueFromConfiguration(PropertyInfo(property), appSetting);
            return this;
        }

        public LemmingConfigurator Reference(string property)
        {
            Reference(PropertyInfo(property));
            return this;
        }

        public LemmingConfigurator Reference(string property, string referencedLemming)
        {
            Reference(PropertyInfo(property), referencedLemming);
            return this;
        }

        public LemmingConfigurator Reference<TReference>(string property) where TReference : class
        {
            Reference<TReference>(PropertyInfo(property));
            return this;
        }

        public LemmingConfigurator Singleton(bool value)
        {
            Lemming.Singleton = value;
            return this;
        }

        public LemmingConfigurator Name(string value)
        {
            Lemming.Name = value;
            return this;
        }

        public LemmingConfigurator Ignore()
        {
            Lemming.Ignore = true;
            return this;
        }
    }

    public class LemmingConfigurator<T> : BaseLemmingConfigurator where T : class
    {
        public LemmingConfigurator(Lemming lemming, IList<Lemming> registeredLemmings)
            : base(lemming, registeredLemmings)
        {
        }

        public LemmingConfigurator<T> Value<TValue>(Expression<Func<T, TValue>> property, TValue value)
        {
            Value(property.ToPropertyInfo(), value);
            return this;
        }

        public LemmingConfigurator<T> ValueFromConfiguration(Expression<Func<T, string>> property,
                                                             string appSetting = null)
        {
            ValueFromConfiguration(property.ToPropertyInfo(), appSetting);
            return this;
        }

        public LemmingConfigurator<T> Reference(Expression<Func<T, object>> property)
        {
            Reference(property.ToPropertyInfo());
            return this;
        }

        public LemmingConfigurator<T> Reference(Expression<Func<T, object>> property, string referencedLemming)
        {
            Reference(property.ToPropertyInfo(), referencedLemming);
            return this;
        }

        public LemmingConfigurator<T> Reference<TReference>(Expression<Func<T, object>> property)
            where TReference : class
        {
            Reference<TReference>(property.ToPropertyInfo());
            return this;
        }

        public LemmingConfigurator<T> Singleton(bool value)
        {
            Lemming.Singleton = value;
            return this;
        }

        public LemmingConfigurator<T> Name(string value)
        {
            Lemming.Name = value;
            return this;
        }

        public LemmingConfigurator<T> Ignore()
        {
            Lemming.Ignore = true;
            return this;
        }
    }
}