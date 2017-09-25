namespace Encuentrame.Model.Accounts.Seekers
{
    public interface IUserSeeker : ISeeker<User>
    {
        IUserSeeker ByUsername(string username);
        IUserSeeker ByRole(RoleEnum role);
        IUserSeeker ByEmail(string email);
        IUserSeeker ByFullName(string fullName);
        IUserSeeker OrderByUsername(SortOrder sortOrder);
        IUserSeeker OrderByEmail(SortOrder sortOrder);
        IUserSeeker OrderByInternalNumber(SortOrder sortOrder);
        IUserSeeker OrderByPhoneNumber(SortOrder sortOrder);
        IUserSeeker OrderByFullname(SortOrder sortOrder);

    }
}