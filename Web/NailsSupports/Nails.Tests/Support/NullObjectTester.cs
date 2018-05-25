using System.Linq;
using System.Reflection;

namespace NailsFramework.Tests.Support
{
    public class NullObjectTester
    {
        public NullObjectTester Test<TInterface>(TInterface nullImplementaton)
        {
            var type = typeof (TInterface);
            var methods =
                type.GetMethods(BindingFlags.Public | BindingFlags.Instance).Where(x => !x.IsGenericMethodDefinition);

            foreach (var method in methods)
                method.Invoke(nullImplementaton, new object[method.GetParameters().Length]);

            foreach (var property in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var index = property.GetIndexParameters().Length == 0
                                ? null
                                : new object[property.GetIndexParameters().Length];

                if (property.CanRead)
                    property.GetValue(nullImplementaton, index);
                if (property.CanWrite)
                    property.SetValue(nullImplementaton, null, index);
            }

            return this;
        }
    }
}