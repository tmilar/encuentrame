using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Encuentrame.Support
{
    public static class Reflect
    {

        public static object GetStaticMethod(this Type type, string methodName, params object[] parameters )
        {
            MethodInfo methodInfo = type.GetMethod(
                            methodName,
                            BindingFlags.Public | BindingFlags.Static
                        );
            return methodInfo.Invoke(null, parameters);
        }

        public static IList<string> GetPropertyList<T>(T obj)
        {
            var type = obj.GetType();
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public);
            return properties.Select(property => property.GetValue(obj, null)).Select(o => o == null ? "" : o.ToString()).ToList();
        }
        
        public static PropertyInfo GetProperty<TClass, TValue>(Expression<Func<TClass, TValue>> expression)
        {
            MemberExpression memberExpression;
            var unary = expression.Body as UnaryExpression;
            if (unary != null)
            {
                memberExpression = unary.Operand as MemberExpression;
            }
            else
            {
                memberExpression = expression.Body as MemberExpression;
            }
            if (memberExpression == null || !(memberExpression.Member is PropertyInfo))
            {
                return null;
            }
            return (PropertyInfo)memberExpression.Member;
        }

        public static ATT GetAttribute<ATT>(Type type) where ATT : Attribute
        {
            var values = type.GetCustomAttributes(typeof(ATT), true);
            if (values.Length == 0)
                return null;
            return (ATT)values[0];
        }

        public static bool ContainsAttribute<ATT>(Type type) where ATT : Attribute
        {
            return type.GetCustomAttributes(typeof(ATT), true).Length > 0;
        }

        public static IEnumerable<T> GetAttributeOfEnumValue<T>(this Enum enumVal) where T : Attribute
        {
            var type = enumVal.GetType();
            var memInfo = type.GetMember(enumVal.ToString());
            var attributes = memInfo[0].GetCustomAttributes<T>( false);
            return attributes;
        }

        public static ATT GetAttribute<ATT>(MethodInfo method) where ATT : Attribute
        {
            var values = method.GetCustomAttributes(typeof(ATT), true);
            if (values.Length == 0)
                return null;
            return (ATT)values[0];
        }

        public static bool ContainsAttribute<ATT>(MethodInfo method) where ATT : Attribute
        {
            return method.GetCustomAttributes(typeof(ATT), true).Length > 0;
        }
        public static MemberExpression MemberExpression<T, R>(this Expression<Func<T, R>> expression)
        {
            return expression.Body.MemberExpression();
        }

        public static MemberExpression MemberExpression(this Expression expression)
        {
            MemberExpression memberExpression = null;
            switch (expression.NodeType)
            {
                case ExpressionType.Convert:
                    {
                        var body = (UnaryExpression)expression;
                        memberExpression = body.Operand as MemberExpression;
                    }
                    break;
                case ExpressionType.MemberAccess:
                    memberExpression = expression as MemberExpression;
                    break;
            }

            if (memberExpression == null) throw new ArgumentException("Not a member access", "expression");
            return memberExpression;
        }

        public static void SetValue<T>(T obj, string property, object propertyValue)
        {
            Type type = obj.GetType();

            PropertyInfo prop = type.GetProperty(property);

            if (propertyValue!=null)
                prop.SetValue(obj, propertyValue, null);
            else
                prop.SetValue(obj, default(object), null);
        }

        public static object GetValue<T>(T obj, string property)
        {
            Type type = obj.GetType();

            PropertyInfo prop = type.GetProperty(property);
            
            return prop.GetValue(obj);
        }


        public static void UpdateAttributes<T, S>(S source, T target)
        {
            const BindingFlags bindingFlags = BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.SetProperty;

            var targetProperties = typeof(T).GetProperties(bindingFlags).ToDictionary(propertyInfo => propertyInfo.Name);

            foreach (PropertyInfo propertyInfo in typeof(S).GetProperties(bindingFlags).ToList())
            {
                if (!propertyInfo.GetCustomAttributes(typeof(IgnoreReflectAttribute), true).Any() && targetProperties.ContainsKey(propertyInfo.Name))
                {

                    var value = propertyInfo.GetValue(source, null);
                    PropertyInfo targetProperty;
                    targetProperties.TryGetValue(propertyInfo.Name, out targetProperty);
                    if (targetProperty != null &&
                        !targetProperty.GetCustomAttributes(typeof(IgnoreReflectAttribute), true).Any())
                    {
                        targetProperty.SetValue(target, value, null);
                    }
                }
            }

        }

    }

    public class IgnoreReflectAttribute : Attribute
    {

    }
}