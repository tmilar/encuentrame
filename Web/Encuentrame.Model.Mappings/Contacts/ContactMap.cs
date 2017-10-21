using Encuentrame.Model.Contacts;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Contacts
{
    public class ContactMap:MappingOf<Contact>
    {
        public ContactMap()
        {
            Map(x => x.Created).Not.Nullable();
            Map(x => x.AcceptedDatetime).Nullable();
            Map(x => x.Status).Not.Nullable();
            References(r => r.User).Column("contact_user_id").Not.Nullable();
        }
    }
}
