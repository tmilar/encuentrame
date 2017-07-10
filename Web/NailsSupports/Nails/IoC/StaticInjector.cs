using System.Collections.Generic;

namespace NailsFramework.IoC
{
    public class StaticInjector
    {
        private readonly IObjectFactory objectFactory;

        public StaticInjector(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        public void Inject(IEnumerable<Injection> injections)
        {
            if (injections == null)
                return;

            foreach (var injection in injections)
                injection.Accept(new Injector(objectFactory));
        }

        #region Nested type: Injector

        private class Injector : IInjector
        {
            private readonly IObjectFactory objectFactory;

            public Injector(IObjectFactory objectFactory)
            {
                this.objectFactory = objectFactory;
            }

            #region IInjector Members

            public void Inject(ValueInjection injection)
            {
                injection.Property.SetValue(null, injection.Value, null);
            }

            public void Inject(ReferenceInjection injection)
            {
                var value = new ReferenceResolver(objectFactory).GetReferenceFor(injection);

                injection.Property.SetValue(null, value, null);
            }

            #endregion
        }

        #endregion
    }
}