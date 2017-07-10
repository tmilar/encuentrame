using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Model.Supports;
using Encuentrame.Model.Supports.Audits;
using Encuentrame.Model.Supports.Interfaces;

namespace Encuentrame.Model.Accounts
{
    [Lemming]
    public class RoleCommand : BaseCommand, IRoleCommand
    {
        [Inject]
        public IBag<Role> Roles { get; set; }

        [Inject]
        public IBag<Pass> Passes { get; set; }

        [Inject]
        public ITranslationService TranslationService { get; set; }

        public class CreateOrEditParameters
        {
            protected CreateOrEditParameters() { }
            public static CreateOrEditParameters Instance()
            {
                return new CreateOrEditParameters();
            }

            public virtual int? Id { get; set; }
            public virtual string Name { get; set; }
            public IList<int> Passes { get; set; }
        }

        public Role Get(int id)
        {
            return Roles[id];
        }

        [Audit(BehaviorType = ActionsEnum.Create, EntityType = typeof(Role))]
        public void Create(CreateOrEditParameters roleParameters)
        {
            var role = new Role()
            {
                Name = roleParameters.Name,
            };

            var ids = roleParameters.Passes;
            var passes = Passes.Where(x => ids.Contains(x.Id));
            foreach (var pass in passes)
            {
                role.Passes.Add(pass);
            }

            Roles.Put(role);
            AuditContextManager.SetObject(role);
            AuditContextManager.Add(TranslationService.Translate("Name"), role.Name);
        }

        public IList<Role> List()
        {
            return Roles.Where(x => x.DeletedKey == null).ToList();
        }

        [Audit(BehaviorType = ActionsEnum.Edit, EntityType = typeof(Role), IdField = "id")]
        public void Edit(int id, CreateOrEditParameters roleParameters)
        {
            var role = Roles[id];

            role.Name = roleParameters.Name;

            role.Passes.Clear();
            var ids = roleParameters.Passes;
            var passes = Passes.Where(x => ids.Contains(x.Id));
            foreach (var pass in passes)
            {
                role.Passes.Add(pass);
            }
            AuditContextManager.SetObject(role);
        }

        [Audit(BehaviorType = ActionsEnum.Delete, EntityType = typeof(Role), IdField = "id")]
        public void Delete(int id)
        {
            var role = Roles[id];

            AuditContextManager.SetObject(role);
            Roles.Remove(role);            
        }
    }
}