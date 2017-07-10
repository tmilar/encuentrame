using System;

namespace Encuentrame.Support
{
    public static class TypeExtensions
    {
        public static string ToTableName(this Type type)
        {
            return type.Name.ToPlural();
        }
        public static R As<R>(this object o)
        {
            return (R) o;
        }
    }
}