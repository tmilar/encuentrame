namespace Encuentrame.Model.AreYouOks.Seekers
{
    public interface IAreYouOkSeeker : ISeeker<AreYouOkActivity>
    {
        IAreYouOkSeeker BySenderUsername(string userName);
        IAreYouOkSeeker OrderBySenderUsername(SortOrder sortOrder);

    }
}