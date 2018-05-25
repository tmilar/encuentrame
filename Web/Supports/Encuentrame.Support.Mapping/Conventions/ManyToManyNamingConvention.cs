using System;
using System.Collections.Generic;
using FluentNHibernate.Conventions.Inspections;
using FluentNHibernate.Conventions;

namespace Encuentrame.Support.Mappings.Conventions
{
    public class ManyToManyNamingConvention : ManyToManyTableNameConvention
    {
        protected override string GetBiDirectionalTableName(IManyToManyCollectionInspector collection, IManyToManyCollectionInspector otherSide)
        {
            return ManyToMany(collection.EntityType, otherSide.EntityType);
        }

        protected override string GetUniDirectionalTableName(IManyToManyCollectionInspector collection)
        {
            return ManyToMany(collection.EntityType, collection.ChildType);
        }

        private string ManyToMany(Type type1, Type type2)
        {
            var types = new List<string>
                                {
                                    type1.Name.ToPlural(),
                                    type2.Name.ToPlural()
                                };
            types.Sort();

            return string.Join(string.Empty, types.ToArray());
        }
    }
}
