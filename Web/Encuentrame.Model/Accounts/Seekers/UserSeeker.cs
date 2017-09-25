using System;
using NHibernate.Linq;

namespace Encuentrame.Model.Accounts.Seekers
{
    public class UserSeeker : BaseSeeker<User>, IUserSeeker
    {
        public IUserSeeker ByUsername(string username)
        {
            Where(x => x.Username.Like($"%{username}%"));
            return this;
        }

        public IUserSeeker ByRole(RoleEnum role)
        {

            Where(x => x.Role == role);
            return this;

        }

        public IUserSeeker ByEmail(string email)
        {
            Where(x => x.Username.Like($"%{email}%"));
            return this;
        }

        public IUserSeeker ByFullName(string fullName)
        {
            Where(x => x.Username.Like($"%{fullName}%"));
            return this;
        }

        public IUserSeeker OrderByUsername(SortOrder sortOrder)
        {
            OrderBy(x => x.Username, sortOrder);
            return this;
        }

        public IUserSeeker OrderByEmail(SortOrder sortOrder)
        {
            OrderBy(x => x.Email, sortOrder);
            return this;
        }

        public IUserSeeker OrderByInternalNumber(SortOrder sortOrder)
        {
            OrderBy(x => x.InternalNumber, sortOrder);
            return this;
        }

        public IUserSeeker OrderByPhoneNumber(SortOrder sortOrder)
        {
            OrderBy(x => x.PhoneNumber, sortOrder);
            return this;
        }

        public IUserSeeker OrderByFullname(SortOrder sortOrder)
        {
            OrderBy(x => x.FullName, sortOrder);
            return this;
        }
    }
}
