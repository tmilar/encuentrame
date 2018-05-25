using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FluentNHibernate.Mapping;
using FluentNHibernate.Utils;

namespace Encuentrame.Support.Mappings
{
    public abstract class MappingOf<T> : BaseMapping<T> where T : IIdentifiable
    {
        public OneToManyPart<Child> HasManyElements<Child>(Expression<Func<T, IEnumerable<Child>>> expression)
        {
            var property = Reflect.GetProperty(expression);
            return HasMany(expression).Table(string.Format("{0}_{1}", typeof(T).Name, property.Name)).Element(property.Name);
        }
        public OneToManyPart<TypeCollection> HasManyElements<TypeCollection>(Expression<Func<T, IDictionary<T, TypeCollection>>> expression)
        {
            var property = Reflect.GetProperty(expression);
            return HasMany(expression).Table(string.Format("{0}_{1}", typeof(T).Name, property.Name)).Element(property.Name);
        }

        protected new ComponentPart<TComponent> Component<TComponent>(Expression<Func<T,TComponent>> expression, Action<FluentNHibernate.Mapping.ComponentPart<TComponent>> action)
        {
            var part = new ComponentPartOf<TComponent>(typeof(T), expression.ToMember());

            action(part);

            Component(part);
            
            return part;
        }
       
    }
}