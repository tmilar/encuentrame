using System;
using System.Collections.Generic;
using System.Linq;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Addresses;
using Encuentrame.Model.Supports;
using Encuentrame.Support;
using NailsFramework.IoC;
using NailsFramework.Persistence;

namespace Encuentrame.Model.Businesses
{
    [Lemming()]
    public class BusinessCommand : BaseCommand, IBusinessCommand
    {
        [Inject]
        public IBag<Business> Businesses { get; set; }

        [Inject]
        public IBag<User> Users { get; set; }

        public Business Get(int id)
        {
            return Businesses[id];
        }





        public void Create(CreateOrEditParameters parameters)
        {
            var business = new Business();

            business.Created = SystemDateTime.Now;
            business.Cuit = parameters.Cuit;
            business.Address = new Address()
            {
                City = parameters.City,
                FloorAndDepartament = parameters.FloorAndDepartament,
                Number = parameters.Number,
                Province = parameters.Province,
                Street = parameters.Street,
                Zip = parameters.Zip
            };
            business.Name = parameters.Name;
            Businesses.Put(business);

            var user = new User()
            {
                Username = parameters.Username,
                Role = RoleEnum.EventAdministrator,
                Email = parameters.Email,
                Firstname = parameters.Firstname,
                Lastname = parameters.Lastname,
                Password = parameters.Password,
                Business = business
            };
            Users.Put(user);

            business.Users.Add(user);


            Businesses.Put(business);
        }

        public IList<Business> List()
        {
            return Businesses.ToList();
        }

        public void Edit(int id, CreateOrEditParameters parameters)
        {
            var business = Businesses[id];

            business.Cuit = parameters.Cuit;
            business.Address = new Address()
            {
                City = parameters.City,
                FloorAndDepartament = parameters.FloorAndDepartament,
                Number = parameters.Number,
                Province = parameters.Province,
                Street = parameters.Street,
                Zip = parameters.Zip
            };
            business.Name = parameters.Name;
        }

        public void Delete(int id)
        {
            var business = Businesses[id];
            business.DeletedKey = SystemDateTime.Now;
        }

        public class CreateOrEditParameters
        {
            public string Name { get; set; }
            public string Cuit { get; set; }


            public string City { get; set; }
            public string Province { get; set; }
            public string Zip { get; set; }
            public string Street { get; set; }
            public string Number { get; set; }
            public string FloorAndDepartament { get; set; }

            public string Username { get; set; }
            public string Password { get; set; }
            public string Lastname { get; set; }
            public string Firstname { get; set; }
            public string Email { get; set; }

        }
    }
}