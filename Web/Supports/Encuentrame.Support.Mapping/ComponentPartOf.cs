using System;
using FluentNHibernate;
using FluentNHibernate.Mapping;

namespace Encuentrame.Support.Mappings
{
    public class ComponentPartOf<TModel> : ComponentPart<TModel>
    {
        public ComponentPartOf(Type entity, Member property) : base(entity, property)
        {
        }
    }
}