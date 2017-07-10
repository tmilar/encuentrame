using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using NHibernate.Criterion;
using Encuentrame.Model.Accounts;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Security.Authorizations;
using Encuentrame.Web.Helpers;
using Encuentrame.Web.Models;
using Encuentrame.Web.Models.Accounts;
using Encuentrame.Support;
using CreateOrEditParameters = Encuentrame.Model.Accounts.RoleCommand.CreateOrEditParameters;
namespace Encuentrame.Web.Controllers
{
    [AuthorizationPass(GroupsOfModulesEnum.Security, ModulesEnum.ManageRole, ActionsEnum.List)]
    public class ManageRoleController : ListBaseController<Role, RoleListModel>
    {
        [Inject]
        public IRoleCommand RoleCommand { get; set; }

        // GET: ManageRole
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Details(int id)
        {
            var role = RoleCommand.Get(id);

            var roleModel = new RoleModel()
            {
                Id = role.Id,
                Name = role.Name,
                Passes = role.Passes.Ids().Join(","),
            };


            return View(roleModel);
        }
        public ActionResult Create()
        {
            var roleModel = new RoleModel();

            return View(roleModel);
        }

        [HttpPost]
        public ActionResult Create(RoleModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var roleParameters = CreateOrEditParameters.Instance();
                roleParameters.Name = roleModel.Name;
                roleParameters.Passes = roleModel.Passes.Numbers(',').ToList();
                RoleCommand.Create(roleParameters);
                AddModelSuccess(Translations.CreateSuccess.FormatWith(TranslationsHelper.Get<Role>()));
                return RedirectToAction("Index");
            }
            else
            {
                return View(roleModel);
            }
        }

        public ActionResult Edit(int id)
        {
            var role = RoleCommand.Get(id);

            var roleModel = new RoleModel()
            {
                Id = role.Id,
                Name = role.Name,
                Passes = role.Passes.Ids().Join(","),
            };

            return View(roleModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, RoleModel roleModel)
        {
            if (ModelState.IsValid)
            {
                var roleParameters = CreateOrEditParameters.Instance();
                roleParameters.Name = roleModel.Name;
                roleParameters.Passes = roleModel.Passes.Numbers(',').ToList();
                RoleCommand.Edit(id, roleParameters);

                AddModelSuccess(Translations.EditSuccess.FormatWith(TranslationsHelper.Get<Role>()));
                return RedirectToAction("Index");
            }
            else
            {
                return View(roleModel);
            }
        }

        [HttpPost]
        public ActionResult Delete(int id)
        {
            RoleCommand.Delete(id);

            return RedirectToAction("Index");
        }

        protected override RoleListModel GetViewModelFrom(Role role)
        {
            var roleModel = new RoleListModel();
            roleModel.Id = role.Id;
            roleModel.Name = role.Name;
            return roleModel;
        }

        protected override IList<Role> GetItemList()
        {
            return RoleCommand.List();
        }
    }
}