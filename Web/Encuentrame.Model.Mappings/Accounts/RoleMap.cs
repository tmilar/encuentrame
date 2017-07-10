using Encuentrame.Model.Accounts;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Accounts
{
    public class RoleMap : MappingOf<Role>
    {
        public RoleMap()
        {
            Map(x => x.Name).Not.Nullable().Length(100).Unique();
            Map(x => x.DeletedKey).Nullable();
            HasManyToMany(x => x.Passes);
        }
    }
}