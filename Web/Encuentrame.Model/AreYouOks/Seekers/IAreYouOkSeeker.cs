namespace Encuentrame.Model.AreYouOks.Seekers
{
    public interface IAreYouOkSeeker : ISeeker<AreYouOk>
    {
        IAreYouOkSeeker BySenderUserName(string userName);
        IAreYouOkSeeker OrderBySenderUserName(SortOrder sortOrder);

    }
}