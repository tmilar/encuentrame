using Encuentrame.Model.Accounts;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Accounts
{
    public class TokenApiSessionMap : MappingOf<TokenApiSession>
    {
        public TokenApiSessionMap()
        {
            Map(x => x.Token).Not.Nullable();
            Map(x => x.UserId).Not.Nullable();
            Map(x => x.ExpiredDateTime).Not.Nullable();
        }
    }
}