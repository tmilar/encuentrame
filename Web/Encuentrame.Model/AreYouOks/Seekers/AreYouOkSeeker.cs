using NHibernate.Linq;

namespace Encuentrame.Model.AreYouOks.Seekers
{
    public class AreYouOkSeeker : BaseSeeker<AreYouOkActivity>, IAreYouOkSeeker
    {
        public IAreYouOkSeeker BySenderUserName(string userName)
        {
            Where(x => x.Sender.Username.Like($"%{userName}%"));
            return this;
        }
        

        public IAreYouOkSeeker OrderBySenderUserName(SortOrder sortOrder)
        {
            OrderBy(x => x.Sender.Username, sortOrder);
            return this;
        }
    }
}
