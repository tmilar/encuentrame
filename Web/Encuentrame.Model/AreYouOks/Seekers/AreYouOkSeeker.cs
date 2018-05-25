using NHibernate.Linq;

namespace Encuentrame.Model.AreYouOks.Seekers
{
    public class AreYouOkSeeker : BaseSeeker<AreYouOkActivity>, IAreYouOkSeeker
    {
        public IAreYouOkSeeker BySenderUsername(string userName)
        {
            Where(x => x.Sender.Username.Like($"%{userName}%"));
            return this;
        }
        

        public IAreYouOkSeeker OrderBySenderUsername(SortOrder sortOrder)
        {
            OrderBy(x => x.Sender.Username, sortOrder);
            return this;
        }
    }
}
