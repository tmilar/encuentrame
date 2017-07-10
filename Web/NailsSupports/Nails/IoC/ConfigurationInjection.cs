using System;
using System.Configuration;
using System.Reflection;

namespace NailsFramework.IoC
{
    public class ConfigurationInjection : Injection
    {
        public ConfigurationInjection(PropertyInfo property, string appSetting) : base(property)
        {
            AppSetting = appSetting;
        }

        public string AppSetting { get; set; }

        public object Value
        {
            get
            {
                var value = GetValue(AppSetting);
                value = value ?? GetValue(PropertyDefault(Owner));
                if (value == null && ParentLemming != null)
                    value = GetValue(PropertyDefault(ParentLemming.Name));

                return value;
            }
        }

        private string PropertyDefault(string key)
        {
            return key + "." + Property.Name;
        }

        public override void Accept(IInjector injector)
        {
            var valueInjector = new ValueInjection(Property, Value);
            injector.Inject(valueInjector);
        }

        private object GetValue(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                return null;

            var stringValue = ConfigurationManager.AppSettings[key];

            var parse = Property.PropertyType.GetMethod("Parse", new[]{typeof(string)});
            return parse != null
                       ? parse.Invoke(null, new[] {stringValue})
                       : stringValue;
        }

        public override Injection MakeGeneric(PropertyInfo genericProperty)
        {
            return new ConfigurationInjection(genericProperty, AppSetting);
        }
    }
}