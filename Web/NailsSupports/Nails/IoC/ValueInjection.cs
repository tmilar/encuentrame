using System.Reflection;

namespace NailsFramework.IoC
{
    public class ValueInjection : Injection
    {
        public ValueInjection(PropertyInfo property, object value) : base(property)
        {
            Value = value;
        }

        public object Value { get; set; }

        public override void Accept(IInjector injector)
        {
            injector.Inject(this);
        }

        public override Injection MakeGeneric(PropertyInfo genericProperty)
        {
            return new ValueInjection(genericProperty, Value);
        }
    }
}