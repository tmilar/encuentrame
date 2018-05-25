using Encuentrame.Model.Accounts;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Accounts
{
    public class BaseUserMap : MappingOf<BaseUser>
    {
        public BaseUserMap()
        {
            Map(x => x.Username).Not.Nullable().Length(100).Unique();
            Map(x => x.Password).Nullable().Length(100);
            Map(x => x.Lastname).Nullable().Length(100);
            Map(x => x.Firstname).Nullable().Length(100);
            Map(x => x.Email).Nullable().Nullable().Length(100);
            Map(x => x.EmailAlternative).Nullable().Length(100);
            Map(x => x.InternalNumber).Nullable().Length(10);
            Map(x => x.PhoneNumber).Nullable().Length(50);
            Map(x => x.MobileNumber).Nullable().Length(50);
            Map(x => x.Image).Nullable().Length(10000).LazyLoad();
            Map(x => x.Role);
            Map(x => x.DeletedKey).Nullable();
            
            DiscriminateSubClassesOnColumn("UserType");
            Table("Users");
        }
    }

    public class UserMap : SubMappingOf<User>
    {
        public UserMap()
        {
            DiscriminatorValue(typeof(User).Name);
            HasMany(x => x.Devices).Inverse().Cascade.AllDeleteOrphan();
            HasMany(x => x.Contacts).KeyColumn("user_id").Cascade.AllDeleteOrphan();
            References(x => x.Business).Nullable();
        }
    }

    public class SystemUserMap : SubMappingOf<SystemUser>
    {
        public SystemUserMap()
        {
            DiscriminatorValue(typeof(SystemUser).Name);
        }
    }
}
