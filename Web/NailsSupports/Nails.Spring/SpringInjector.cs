using Spring.Objects.Factory.Support;

namespace NailsFramework.IoC
{
    public class SpringInjector : IInjector
    {
        private readonly ObjectDefinitionBuilder builder;
        private readonly string property;

        public SpringInjector(string property, ObjectDefinitionBuilder builder)
        {
            this.property = property;
            this.builder = builder;
        }

        #region IInjector Members

        public void Inject(ValueInjection injection)
        {
            builder.AddPropertyValue(property, injection.Value);
        }

        public void Inject(ReferenceInjection injection)
        {
            builder.AddPropertyReference(property, injection.ReferencedLemming);
        }

        #endregion
    }
}