using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using NailsFramework.IoC;

namespace NailsFramework.Config
{
    public abstract class BaseLemmingConfigurator
    {
        private readonly IList<Lemming> registeredLemmings;

        protected BaseLemmingConfigurator(Lemming lemming, IList<Lemming> registeredLemmings)
        {
            Lemming = lemming;
            this.registeredLemmings = registeredLemmings;
        }

        protected Lemming Lemming { get; private set; }

        protected void Value<TValue>(PropertyInfo property, TValue value)
        {
            Lemming.RemoveInjectionOf(property);
            Lemming.Inject(new ValueInjection(property, value));
        }

        protected void ValueFromConfiguration(PropertyInfo property, string appSetting)
        {
            Lemming.RemoveInjectionOf(property);
            Lemming.Inject(new ConfigurationInjection(property, appSetting));
        }

        protected void Reference(PropertyInfo property)
        {
            GetOrCreateReference(property);
        }

        private ReferenceInjection GetOrCreateReference(PropertyInfo property)
        {
            var injection = Lemming.Injections
                .OfType<ReferenceInjection>()
                .SingleOrDefault(x => x.Property == property);

            if (injection == null)
            {
                injection = new ReferenceInjection(property);
                Lemming.Inject(injection);
            }

            return injection;
        }

        protected void Reference(PropertyInfo property, string referencedLemming)
        {
            var injection = GetOrCreateReference(property);
            injection.ReferencedLemming = referencedLemming;
        }

        protected void Reference<TReference>(PropertyInfo property) where TReference : class
        {
            var referenceLemming = registeredLemmings.SingleOrDefault(x => x.ConcreteType == typeof (TReference));

            if (referenceLemming == null)
            {
                referenceLemming = Lemming.From(typeof (TReference));
                registeredLemmings.Add(referenceLemming);
            }
            Reference(property, referenceLemming.Name);
        }
    }
}