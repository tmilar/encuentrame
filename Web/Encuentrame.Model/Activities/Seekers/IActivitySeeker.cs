namespace Encuentrame.Model.Activities.Seekers
{
    public interface IActivitySeeker : ISeeker<Activity>
    {
        IActivitySeeker ByName(string name);
        IActivitySeeker OrderByName(SortOrder sortOrder);

    }
}