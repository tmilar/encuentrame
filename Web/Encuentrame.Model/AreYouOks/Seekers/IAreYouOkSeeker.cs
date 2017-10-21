namespace Encuentrame.Model.AreYouOks.Seekers
{
    public interface IAreYouOkSeeker : ISeeker<AreYouOkActivity>
    {
        IAreYouOkSeeker BySenderUserName(string userName);
        IAreYouOkSeeker OrderBySenderUserName(SortOrder sortOrder);

    }
}