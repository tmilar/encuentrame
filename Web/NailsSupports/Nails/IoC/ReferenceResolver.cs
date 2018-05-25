namespace NailsFramework.IoC
{
    public class ReferenceResolver
    {
        private readonly IObjectFactory objectFactory;

        public ReferenceResolver(IObjectFactory objectFactory)
        {
            this.objectFactory = objectFactory;
        }

        public object GetReferenceFor(ReferenceInjection injection)
        {
            object value = null;

            if (!string.IsNullOrWhiteSpace(injection.ReferencedLemming))
                value = objectFactory.GetObject(injection.ReferencedLemming);

            if (value == null)
                value = objectFactory.GetObject(injection.Property.PropertyType);

            return value;
        }
    }
}