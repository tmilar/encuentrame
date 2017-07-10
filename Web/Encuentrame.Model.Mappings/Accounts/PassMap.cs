using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Accounts
{
    public class PassMap:MappingOf<Pass>
    {
        public PassMap()
        {
            Map(x => x.Group).Column("groupOfModule").CustomSqlType(typeof(int).ToSqlType()).CustomType<GroupsOfModulesEnum>().Not.Nullable();
            Map(x => x.Module).CustomSqlType(typeof(int).ToSqlType()).CustomType<ModulesEnum>().Not.Nullable();
            Map(x => x.Action).CustomSqlType(typeof(int).ToSqlType()).CustomType<ActionsEnum>().Not.Nullable();
        }
    }
}
