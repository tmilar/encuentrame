using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace NailsFramework.Support
{
    /// <summary>
    ///   A set of methods that provide functionality to work with attributes, fields, descriptors, etc.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        ///   Returns an instance of the Attribute T that's applied to the member 
        ///   if one exists, or null if it does not.
        /// </summary>
        /// <typeparam name = "T">The Type of the Attribute.</typeparam>
        /// <param name = "member">The member to analyze.</param>
        /// <returns>The attribute found or null.</returns>
        public static T Attribute<T>(this ICustomAttributeProvider member) where T : Attribute
        {
            return Attribute<T>(member, false);
        }

        /// <summary>
        ///   Returns an instance of the Attribute T that's applied to the member
        ///   if one exists, or null if it does not.
        /// </summary>
        /// <typeparam name = "TAttribute">The Type of the Attribute.</typeparam>
        /// <param name = "member">The member to analyze.</param>
        /// <param name = "inherit">Specified whether to search this member's inheritance chain to find the attribute.</param>
        /// <returns>The attribute found or null.</returns>
        public static TAttribute Attribute<TAttribute>(this ICustomAttributeProvider member, bool inherit)
            where TAttribute : Attribute
        {
            var attributes = member.GetCustomAttributes(typeof (TAttribute), inherit);
            return attributes.Length == 0 ? null : (TAttribute) attributes[0];
        }

        private static void AppendTypeName(StringBuilder builder, MethodBase method)
        {
            builder.Append(method.Name.Split('`')[0]);
            if (!method.IsGenericMethod) return;

            builder.Append("<");

            foreach (var genericArgument in method.GetGenericArguments())
                AppendTypeFullName(builder, genericArgument);

            builder.Append(">");
        }

        /// <summary>
        ///   Returns the types decored by the specified attribute
        /// </summary>
        /// <typeparam name = "T">The type of the ttribute type.</typeparam>
        /// <param name = "assembly">The assembly.</param>
        /// <param name = "inherit">Specified whether to search this member's inheritance chain to find the attribute.</param>
        /// <returns></returns>
        public static IEnumerable<Type> TypesWithAttribute<T>(this Assembly assembly, bool inherit = false)
            where T : Attribute
        {
            return assembly.GetExportedTypes().Where(x => x.IsDefined(typeof (T), inherit) && !x.IsAbstract);
        }

        /// <summary>
        ///   Retrieves the property represented by the specified expression
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <param name = "expression">The expression.</param>
        /// <returns></returns>
        public static PropertyInfo ToPropertyInfo<T>(this Expression<Func<T, object>> expression)
        {
            return ToPropertyInfo<T, object>(expression);
        }

        /// <summary>
        ///   Retrieves the property represented by the specified expression
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <typeparam name = "TReturn"></typeparam>
        /// <param name = "expression">The expression.</param>
        /// <returns></returns>
        public static PropertyInfo ToPropertyInfo<T, TReturn>(this Expression<Func<T, TReturn>> expression)
        {
            var memberExpression = MemberExpression(expression);
            return (PropertyInfo) memberExpression.Member;
        }

        /// <summary>
        ///   Retrieves the static property represented by the specified expression
        /// </summary>
        /// <typeparam name = "TReturn"></typeparam>
        /// <param name = "expression">The expression.</param>
        /// <returns></returns>
        public static PropertyInfo ToPropertyInfo<TReturn>(this Expression<Func<TReturn>> expression)
        {
            var memberExpression = MemberExpression(expression);
            return (PropertyInfo) memberExpression.Member;
        }

        /// <summary>
        ///   Retrieves the member expression represented by the specified expression
        /// </summary>
        /// <typeparam name = "T"></typeparam>
        /// <typeparam name = "TReturn"></typeparam>
        /// <param name = "expression">The expression.</param>
        /// <returns></returns>
        public static MemberExpression MemberExpression<T, TReturn>(this Expression<Func<T, TReturn>> expression)
        {
            return expression.Body.MemberExpression();
        }

        /// <summary>
        ///   Retrieves the member expression represented by the specified expression
        /// </summary>
        /// <param name = "expression">The expression.</param>
        /// <returns></returns>
        public static MemberExpression MemberExpression(this Expression expression)
        {
            MemberExpression memberExpression = null;
            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    {
                        var body = (UnaryExpression) expression;
                        memberExpression = body.Operand as MemberExpression;
                    }
                    break;
                case ExpressionType.MemberAccess:
                    memberExpression = expression as MemberExpression;
                    break;
                case ExpressionType.Lambda:
                    {
                        var lambdaExpression = (LambdaExpression) expression;
                        memberExpression = lambdaExpression.Body as MemberExpression;
                    }
                    break;
            }

            if (memberExpression == null) throw new ArgumentException("Not a member access", "expression");
            return memberExpression;
        }

        public static IEnumerable<PropertyInfo> Setters(this Type type)
        {
            return from property in type.GetProperties() where property.CanWrite select property;
        }

        public static IEnumerable<Type> ConcreteSubclassesOf<T>(this Assembly assembly)
        {
            return assembly.GetTypes().Where(x => !x.IsAbstract && typeof (T).IsAssignableFrom(x));
        }

        public static IEnumerable<Type> ConcreteSubclassesOf<T>(this IEnumerable<Assembly> assemblies)
        {
            return assemblies.SelectMany(ConcreteSubclassesOf<T>);
        }

        public static string FullFriendlyName(this MethodBase method)
        {
            var builder = new StringBuilder();
            builder.Append(method.ReflectedType.FullFriendlyName()).Append(".");
            AppendTypeName(builder, method);
            return builder.ToString();
        }

        public static string FullFriendlyName(this Type self)
        {
            var builder = new StringBuilder();
            AppendTypeFullName(builder, self);

            return builder.ToString();
        }

        private static void AppendTypeFullName(StringBuilder builder, Type type)
        {
            if (type.IsGenericParameter)
            {
                builder.Append(type.Name);
            }
            else
            {
                builder.Append(type.Namespace).Append(".");
                AppendTypeName(builder, type);
            }
        }

        private static void AppendTypeName(StringBuilder builder, Type type)
        {
            var typesChain = new Stack<Type>();
            typesChain.Push(type);

            while (typesChain.Peek().DeclaringType != null)
                typesChain.Push(typesChain.Peek().DeclaringType);

            while (typesChain.Count > 0)
            {
                var nested = typesChain.Pop();

                builder.Append(nested.Name.Split('`')[0]);

                if (nested.IsGenericType)
                {
                    builder.Append("<");

                    var first = true;
                    foreach (var genericArgument in nested.GetGenericArguments())
                    {
                        if (!first)
                            builder.Append(", ");
                        AppendTypeFullName(builder, genericArgument);
                        first = false;
                    }

                    builder.Append(">");
                }

                if (typesChain.Count > 0)
                    builder.Append(".");
            }
        }

        public static bool IsStatic(this PropertyInfo self)
        {
            return (self.CanRead && self.GetGetMethod(true).IsStatic) ||
                   (self.CanWrite && self.GetSetMethod(true).IsStatic);
        }

        public static IEnumerable<Type> AllParentTypes(this Type self, bool includeItself = false)
        {
            var type = includeItself ? self : self.BaseType;
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }

            foreach (var @interface in self.GetInterfaces())
                yield return @interface;
        }

        public static string FullNameWithoutTypeParemeters(this Type self)
        {
            return self.Namespace + "." + self.Name.Split('`')[0];
        }

        public static bool Is<BaseType>(this Type self)
        {
            return typeof(BaseType).IsAssignableFrom(self);
        }
    }
}