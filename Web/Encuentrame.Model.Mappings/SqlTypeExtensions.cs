using System;
using NHibernate.Proxy;
using Encuentrame.Support;

namespace Encuentrame.Model.Mappings
{
    public static class SqlTypeExtensions
    {
        public static string ToSqlType(this Type type)
        {
            if (type == typeof (int))
            {
                return "int";
            }
            else
            {
                throw new ArgumentException("The type '{0}' not is valid.".FormatWith(type.FullName));
            }
        }
    }

   
}
