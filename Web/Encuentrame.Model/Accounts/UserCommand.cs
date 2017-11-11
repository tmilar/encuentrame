using System.Collections.Generic;
using System.Linq;
using Encuentrame.Model.Businesses;
using Encuentrame.Model.Devices;
using NailsFramework.IoC;
using NailsFramework.Persistence;

using Encuentrame.Model.Supports;
using Encuentrame.Support;

namespace Encuentrame.Model.Accounts
{
    [Lemming]
    public class UserCommand : BaseCommand, IUserCommand
    {
        [Inject]
        public IBag<User> Users { get; set; }


        [Inject]
        public IBag<Business> Businesses { get; set; }
        public User Get(int id)
        {
            return Users[id];
        }

        //[Audit(BehaviorType = ActionsEnum.Create, EntityType = typeof(User))]
        public void Create(CreateOrEditParameters userParameters)
        {
            var user = new User();
            user.Username = userParameters.Username;
            user.Lastname = userParameters.Lastname;
            user.Password = "123";
            user.Firstname = userParameters.Firstname;
            user.Email = userParameters.Email;
            user.EmailAlternative = userParameters.EmailAlternative;
            user.InternalNumber = userParameters.InternalNumber;
            user.MobileNumber = userParameters.MobileNumber;
            user.PhoneNumber = userParameters.PhoneNumber;
            if (userParameters.Image.NotIsNullOrEmpty())
            {
                user.Image = userParameters.Image;
            }

            if (userParameters.Role == RoleEnum.EventAdministrator)
            {
                user.Business = Businesses[userParameters.Business];
            }

            user.Role = userParameters.Role;

            Users.Put(user);

            //AuditContextManager.SetObject(user);
            //AuditContextManager.Add(TranslationService.Translate("Username"), user.Username);
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
                Lastname = userParameters.Lastname,
                Password = userParameters.Password,
                Firstname = userParameters.Firstname,
                Email = userParameters.Email,
                EmailAlternative = userParameters.EmailAlternative,
                InternalNumber = userParameters.InternalNumber,
                MobileNumber = userParameters.MobileNumber,
                PhoneNumber = userParameters.PhoneNumber,
                Role = userParameters.Role
            };

            if (userParameters.Image.NotIsNullOrEmpty())
            {
                user.Image = userParameters.Image;
            }

            Users.Put(user);
        }

        public void EditRegister( CreateOrEditParameters userParameters)
        {
            var user = Users.Where(x=>x.Username==userParameters.Username).First();
            user.Lastname = userParameters.Lastname;
            user.Firstname = userParameters.Firstname;
            user.Email = userParameters.Email;
            user.EmailAlternative = userParameters.EmailAlternative;
            user.InternalNumber = userParameters.InternalNumber;
            user.MobileNumber = userParameters.MobileNumber;
            user.PhoneNumber = userParameters.PhoneNumber;
            if (userParameters.Image.NotIsNullOrEmpty())
            {
                user.Image = userParameters.Image;
            }

        }

        //[Audit(BehaviorType = ActionsEnum.Edit, EntityType = typeof(User), IdField = "id")]
        public void Edit(int id, CreateOrEditParameters userParameters)
        {
            var user = Users[id];
            user.Lastname = userParameters.Lastname;
            user.Firstname = userParameters.Firstname;
            user.Email = userParameters.Email;
            user.EmailAlternative = userParameters.EmailAlternative;
            user.InternalNumber = userParameters.InternalNumber;
            user.MobileNumber = userParameters.MobileNumber;
            user.PhoneNumber = userParameters.PhoneNumber;
            
            if (userParameters.Image.NotIsNullOrEmpty())
            {
                user.Image = userParameters.Image;
            }

            user.Role = userParameters.Role;

            if (userParameters.Role == RoleEnum.EventAdministrator)
            {
                user.Business = Businesses[userParameters.Business];
            }
            //AuditContextManager.SetObject(user);
        }

        //[Audit(BehaviorType = ActionsEnum.Delete, EntityType = typeof(User), IdField = "id")]
        public void Delete(int id)
        {
            var user = Users[id];

            Users.Remove(user);
            //AuditContextManager.SetObject(user);
        }

        public void SetDevice(DeviceParameters deviceParameters)
        {
            var user = Users[deviceParameters.UserId];

            if (!user.Devices.Where(x => x.Token == deviceParameters.Token).Any())
            {
                var device = new Device()
                {
                    Token = deviceParameters.Token,
                    User = user
                };

                user.Devices.Add(device);
            }

         
        }

        public IList<User> GetUsersByIds(IEnumerable<int> ids)
        {
            return Users.Where(x => x.DeletedKey == null && ids.Contains(x.Id)).ToList();
        }

        public IList<User> List()
        {
            return Users.Where(x => x.DeletedKey == null).ToList();
        }

        public IList<User> ListUsers()
        {
            return Users.Where(x => x.DeletedKey == null && x.Role==RoleEnum.User).ToList();
        }



        public class CreateOrEditParameters
        {
           
            public string Username { get; set; }
            public string Password { get; set; }
            public string Lastname { get; set; }
            public string Firstname { get; set; }
            public string Email { get; set; }
            public string EmailAlternative { get; set; }
            public string InternalNumber { get; set; }
            public string PhoneNumber { get; set; }
            public string MobileNumber { get; set; }
            public string Image { get; set; }
            public RoleEnum Role { get; set; }
            public int Business { get; set; }
        }

        public class DeviceParameters 
        {
            public int UserId { get; set; }
            public string Token { get; set; }
        }
    }
}
