using NHibernate.Linq;

namespace Encuentrame.Model.Activities.Seekers
{
    public class ActivitySeeker : BaseSeeker<Activity>, IActivitySeeker
    {
        public IActivitySeeker ByName(string name)
        {
            Where(x => x.Name.Like($"%{name}%"));
            return this;
        }
     
        public IActivitySeeker OrderByName(SortOrder sortOrder)
        {
            OrderBy(x => x.Name, sortOrder);
            return this;
        }
    }
}
