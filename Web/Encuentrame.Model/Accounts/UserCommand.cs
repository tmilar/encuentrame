using System.Collections.Generic;
using System.Linq;
using Encuentrame.Model.Accounts.Permissions;
using NailsFramework.IoC;
using NailsFramework.Persistence;

using Encuentrame.Model.Supports;
using Encuentrame.Model.Supports.Audits;

namespace Encuentrame.Model.Accounts
{
    [Lemming]
    public class UserCommand : BaseCommand, IUserCommand
    {
        [Inject]
        public IBag<User> Users { get; set; }
        
        public User Get(int id)
        {
            return Users[id];
        }

        [Audit(BehaviorType = ActionsEnum.Create, EntityType = typeof(User))]
        public void Create(CreateOrEditParameters userParameters)
        {
            var user = new User();
            user.Username = userParameters.Username;
            user.LastName = userParameters.LastName;
            
            user.FirstName = userParameters.FirstName;
            user.Email = userParameters.Email;
            user.EmailAlternative = userParameters.EmailAlternative;
            user.InternalNumber = userParameters.InternalNumber;
            user.MobileNumber = userParameters.MobileNumber;
            user.PhoneNumber = userParameters.PhoneNumber;
            user.Image = userParameters.Image;
            user.Role = userParameters.Role;

            Users.Put(user);

            AuditContextManager.SetObject(user);
            AuditContextManager.Add(TranslationService.Translate("UserName"), user.Username);
        }
        
        public void NewRegister(CreateOrEditParameters userParameters)
        {
            if (Users.Where(x => x.Username == userParameters.Username).Any())
            {
                throw new UserUsernameUniqueException();
            }

            var user = new User
            {
                Username = userParameters.Username,
                LastName = userParameters.LastName,
                Password = userParameters.Password,
                FirstName = userParameters.FirstName,
                Email = userParameters.Email,
                EmailAlternative = userParameters.EmailAlternative,
                InternalNumber = userParameters.InternalNumber,
                MobileNumber = userParameters.MobileNumber,
                PhoneNumber = userParameters.PhoneNumber,
                Image = userParameters.Image,
                Role = userParameters.Role
            };

            Users.Put(user);
        }

        public void EditRegister( CreateOrEditParameters userParameters)
        {
            var user = Users.Where(x=>x.Username==userParameters.Username).First();
            user.LastName = userParameters.LastName;
            user.FirstName = userParameters.FirstName;
            user.Email = userParameters.Email;
            user.EmailAlternative = userParameters.EmailAlternative;
            user.InternalNumber = userParameters.InternalNumber;
            user.MobileNumber = userParameters.MobileNumber;
            user.PhoneNumber = userParameters.PhoneNumber;
            user.Image = userParameters.Image;
            
        }

        [Audit(BehaviorType = ActionsEnum.Edit, EntityType = typeof(User), IdField = "id")]
        public void Edit(int id, CreateOrEditParameters userParameters)
        {
            var user = Users[id];
            user.LastName = userParameters.LastName;
            user.FirstName = userParameters.FirstName;
            user.Email = userParameters.Email;
            user.EmailAlternative = userParameters.EmailAlternative;
            user.InternalNumber = userParameters.InternalNumber;
            user.MobileNumber = userParameters.MobileNumber;
            user.PhoneNumber = userParameters.PhoneNumber;
            user.Image = userParameters.Image;
            user.Role = userParameters.Role;
            AuditContextManager.SetObject(user);
        }

        [Audit(BehaviorType = ActionsEnum.Delete, EntityType = typeof(User), IdField = "id")]
        public void Delete(int id)
        {
            var user = Users[id];

            Users.Remove(user);
            AuditContextManager.SetObject(user);
        }

        public IList<User> List()
        {
            return Users.Where(x => x.DeletedKey == null).ToList();
        }

       

        public class CreateOrEditParameters
        {
            public virtual string Name { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
            public string LastName { get; set; }
            public string FirstName { get; set; }
            public string Email { get; set; }
            public string EmailAlternative { get; set; }
            public string InternalNumber { get; set; }
            public string PhoneNumber { get; set; }
            public string MobileNumber { get; set; }
            public string Image { get; set; }
            public RoleEnum Role { get; set; }
        }
    }
}
