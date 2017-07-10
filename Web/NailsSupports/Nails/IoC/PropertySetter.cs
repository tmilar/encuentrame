using System;
using System.Reflection;

namespace NailsFramework.IoC
{
    public abstract class PropertySetter
    {
        public abstract void SetValue(object instance, object value);

        public static PropertySetter From(PropertyInfo property)
        {
            var type = typeof (PropertySetter<,>).MakeGenericType(property.ReflectedType, property.PropertyType);
            return (PropertySetter) Activator.CreateInstance(type, property);
        }
    }

    public class PropertySetter<TObject, TProperty> : PropertySetter
    {
        private readonly Action<TObject, TProperty> method;

        public PropertySetter(PropertyInfo property)
        {
            method =
                (Action<TObject, TProperty>)
                Delegate.CreateDelegate(typeof (Action<TObject, TProperty>), property.GetSetMethod());
        }

        public override void SetValue(object instance, object value)
        {
            method((TObject) instance, (TProperty) value);
        }
    }
}