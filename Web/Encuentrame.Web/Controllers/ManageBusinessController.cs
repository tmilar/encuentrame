using System.Collections.Generic;
using System.Web.Mvc;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Businesses;
using Encuentrame.Security.Authorizations;
using Encuentrame.Support;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.Models.Businesses;
using NailsFramework.IoC;

namespace Encuentrame.Web.Controllers
{
    [AuthorizationPass(new[] { RoleEnum.Administrator })]
    public class ManageBusinessController : ListBaseController<Business,BusinessListModel>
    {

        [Inject]
        public IBusinessCommand BusinessCommand { get; set; }
        // GET: ManageBusiness
        public ActionResult Index()
        {
            return View();
        }

        protected override BusinessListModel GetViewModelFrom(Business item)
        {
            return new BusinessListModel()
            {
                 Id = item.Id,
                 Address = item.Address.ToDisplay(),
                 Cuit = item.Cuit,
                 Name = item.Name,
                 Created = item.Created
            };
        }

        protected override IList<Business> GetItemList()
        {
            return BusinessCommand.List();
        }



        public ActionResult Details(int id)
        {
            var businesst = BusinessCommand.Get(id);

           

            var businesstModel = new BusinessModel()
            {
                Id = businesst.Id,
                Name = businesst.Name,
                Cuit = businesst.Cuit,
               
                Number = businesst.Address.Number,
                City = businesst.Address.City,
                Province = businesst.Address.Province,
                Street = businesst.Address.Street,
                Zip = businesst.Address.Zip,
                FloorAndDepartament = businesst.Address.FloorAndDepartament,

            };

            return View(businesstModel);
        }


        public ActionResult Create()
        {
            var businesstModel = new BusinessModel();

            return View(businesstModel);
        }

        [HttpPost]
        public ActionResult Create(BusinessModel businesstModel)
        {
            if (ModelState.IsValid)
            {
                var businesstParameters = GetCreateOrEditParameters(businesstModel);

                businesstParameters.Username = businesstModel.Username;
                businesstParameters.Password = businesstModel.Password;
                businesstParameters.Lastname = businesstModel.Lastname;
                businesstParameters.Firstname = businesstModel.Firstname;
                businesstParameters.Email = businesstModel.Email;

                BusinessCommand.Create(businesstParameters);

                AddModelSuccess(Translations.CreateSuccess.FormatWith(TranslationsHelper.Get<Business>()));

                return RedirectToAction("Index");
            }
            else
            {
                return View(businesstModel);
            }
        }

        private BusinessCommand.CreateOrEditParameters GetCreateOrEditParameters(BusinessModel businesstModel)
        {
            var businesstParameters = new BusinessCommand.CreateOrEditParameters
            {
                Name = businesstModel.Name,
                Cuit = businesstModel.Cuit,
               
                Number = businesstModel.Number,
                City = businesstModel.City,
                Province = businesstModel.Province,
                Street = businesstModel.Street,
                Zip = businesstModel.Zip,
                FloorAndDepartament = businesstModel.FloorAndDepartament,

               
            };

            return businesstParameters;
        }

        public ActionResult Edit(int id)
        {
            var businesst = BusinessCommand.Get(id);

            var businesstModel = new BusinessModel()
            {
                Id = businesst.Id,
                Name = businesst.Name,
                Cuit = businesst.Cuit,

                Number = businesst.Address.Number,
                City = businesst.Address.City,
                Province = businesst.Address.Province,
                Street = businesst.Address.Street,
                Zip = businesst.Address.Zip,
                FloorAndDepartament = businesst.Address.FloorAndDepartament,

            };

            return View(businesstModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, BusinessModel businesstModel)
        {
            if (ModelState.IsValid)
            {
                var businesstParameters = GetCreateOrEditParameters(businesstModel);
               
                BusinessCommand.Edit(id, businesstParameters);

                AddModelSuccess(Translations.EditSuccess.FormatWith(TranslationsHelper.Get<Business>()));
                return RedirectToAction("Index");
            }
            else
            {
                return View(businesstModel);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            BusinessCommand.Delete(id);

            return RedirectToAction("Index");
        }
    }
}