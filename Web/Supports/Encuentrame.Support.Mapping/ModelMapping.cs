using System;
using System.Reflection;
using FluentNHibernate;

namespace Encuentrame.Support.Mappings
{
    public static class ModelMapping
    {
        public static string TableNameFor<T>() where T : IIdentifiable
        {
            return typeof (T).ToTableName();
        }

        public const string Id = "Id";

        public static string ForeignKey(Member member, Type type)
        {
            return String.Format("{0}{1}", Id, type.Name);
        }

        public static string TableNameFor(MemberInfo prop)
        {
            return prop.DeclaringType.Name + prop.Name;
        }
    }
}