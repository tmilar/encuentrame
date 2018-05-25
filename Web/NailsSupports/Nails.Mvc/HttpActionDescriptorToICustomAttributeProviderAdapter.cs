using System;
using System.Reflection;
using System.Web.Http.Controllers;
using System.Collections;
using System.Linq;

namespace NailsFramework.UserInterface
{
    class HttpActionDescriptorToICustomAttributeProviderAdapter : ICustomAttributeProvider
    {
        private static readonly MethodInfo GetCustomAttributesMethodInfo = typeof(HttpActionDescriptor).GetMethod("GetCustomAttributes", new Type[0]);
        private static readonly object[] EmptyAttributeArguments = new object[0];

        private readonly HttpActionDescriptor actionDescriptor;

        public HttpActionDescriptorToICustomAttributeProviderAdapter(HttpActionDescriptor actionDescriptor)
        {
            this.actionDescriptor = actionDescriptor;
            
        }

        public object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            
            var methodInfo = GetCustomAttributesMethodInfo.MakeGenericMethod(attributeType);
            
            var foundAttributes = methodInfo.Invoke(actionDescriptor, EmptyAttributeArguments) as IEnumerable;            
            return foundAttributes.Cast<object>().ToArray();
        }

        public bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }        
    }
}
