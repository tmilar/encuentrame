using System;
using System.Collections.Generic;
using System.Linq;

namespace NailsFramework.IoC
{
    public static class InjectionsExtensions
    {
        public static IEnumerable<Type> UnmanagedTypesReferencedAsEnumerable(this IEnumerable<Injection> self)
        {
            //So, those are the ones without custom name and which property is IEnumerable<T>
            //If they have custom name, then it should be a lemming with that name that implements IEnumerable<T>
            return self.OfType<ReferenceInjection>()
                .Where(x => string.IsNullOrWhiteSpace(x.ReferencedLemming))
                .Select(x => x.Property.PropertyType)
                .Where(x => x.IsGenericType &&
                            x.GetGenericTypeDefinition() == typeof (IEnumerable<>))
                .Select(x => x.GetGenericArguments()[0]);
        }
    }
}