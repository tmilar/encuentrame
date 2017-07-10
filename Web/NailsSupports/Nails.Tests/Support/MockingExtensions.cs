using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Rhino.Mocks.Interfaces;

namespace NailsFramework.Tests.Support
{
    public static class MockingExtensions
    {
        private static IEnumerable<Type> TypeHierarchy(this Type self)
        {
            return self == typeof (object)
                       ? new[] {self}
                       : new[] {self}.Union(self.BaseType.TypeHierarchy());
        }

        public static IMethodOptions<object> Expect<T>(this T self, string member, params Type[] parameterTypes)
        {
            var methods =
                typeof (T).TypeHierarchy()
                    .SelectMany(m => m.GetMethods(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance)
                                         .Where(x => x.Name == member)
                    )
                    .Select(x => x.GetBaseDefinition())
                    .Distinct()
                    .ToArray();

            if (methods.Length == 0)
            {
                var property = typeof (T).GetProperty(member, BindingFlags.Instance | BindingFlags.NonPublic);

                if (property == null)
                    throw new Exception("Member " + member + " not found in " + typeof (T).FullName);

                return Rhino.Mocks.Expect.Call(property.GetValue(self, null));
            }

            object[] parameters;
            MethodInfo method;

            if (methods.Length == 1)
            {
                method = methods.Single();
                parameters = new object[method.GetParameters().Length];
            }
            else
            {
                parameters = new object[parameterTypes.Length];
                method = methods.SingleOrDefault(
                    x => MatchTypes(x.GetParameters().Select(p => p.ParameterType), parameterTypes));

                if (method == null)
                    throw new Exception("Member " + member + " not found in " + typeof (T).FullName);
            }

            return Rhino.Mocks.Expect.Call(method.Invoke(self, parameters));
        }

        private static bool MatchTypes(IEnumerable<Type> t1, IEnumerable<Type> t2)
        {
            var l1 = t1.ToList();
            var l2 = t2.ToList();

            if (l1.Count != l2.Count)
                return false;

            return !l1.Where((t, i) => t != l2[i]).Any();
        }
    }
}