using System;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using NailsFramework.Support;

namespace Encuentrame.Support.Mappings
{
    public static class MappingExtensions
    {
        private static string GetColumnName<T,C>(Expression<Func<T, object>> component, Expression<Func<C, object>> property)
        {
            return GetColumnName(component, property.ToPropertyInfo().Name);
        }
        private static string GetColumnName<T>(Expression<Func<T, object>> component, string name)
        {
            var componentName = component.ToPropertyInfo().Name;
            return string.Format("{0}{1}", componentName, name);
        }
        public static OneToManyPart<TModel> Elements<TModel>(this OneToManyPart<TModel> oneToManyPart)
        {
            return oneToManyPart.Element("string");
        }
        
    }
}