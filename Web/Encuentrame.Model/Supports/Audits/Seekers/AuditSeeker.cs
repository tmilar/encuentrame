using System.Linq;
using NailsFramework.IoC;
using NailsFramework.Persistence;
using Encuentrame.Model.Accounts.Permissions;

namespace Encuentrame.Model.Supports.Audits.Seekers
{
    [Lemming(Singleton = false)]
    public class AuditSeeker : BaseSeeker<Audit>, IAuditSeeker
    {
        public AuditSeeker()
        {

        }

        public AuditSeeker(IBag<Audit> audits)
            : base(audits)
        {
        }

        protected override void ApplyDefaultFilters()
        {

        }

        public IAuditSeeker OrderByEntityId(SortOrder sortOrder)
        {
            OrderBy(x => x.EntityId, sortOrder);
            return this;
        }

        public IAuditSeeker OrderByAuditBehaviorType(SortOrder sortOrder)
        {
            OrderBy(x => x.Action, sortOrder);
            return this;
        }

        public IAuditSeeker OrderByUser(SortOrder sortOrder)
        {
            OrderBy(x => x.User.FullName, sortOrder);
            return this;
        }

        public IAuditSeeker OrderByDate(SortOrder sortOrder)
        {
            OrderBy(x => x.Date, sortOrder);
            return this;
        }

        public IAuditSeeker OrderByEntityType(SortOrder sortOrder)
        {
            OrderBy(x => x.EntityType, sortOrder);
            return this;
        }

        public IAuditSeeker ByEntityType(string value)
        {
            FilterReference(value, (entityType) => Where(x => x.EntityType == entityType), (entityTypes) => Where(x => entityTypes.Contains(x.EntityType)));
            return this;
        }

        public IAuditSeeker ByUser(string value)
        {
            FilterReference(value, (id) => Where(x => x.User.Id == id));

            return this;
        }

        public IAuditSeeker ByAuditBehaviorType(string value)
        {
            FilterEnum<ActionsEnum>(value, (status) => Where(x => x.Action == (int)status), (enumValues) =>
            {
                var enumIntvalues = enumValues.Select(y => (int)y).ToList();
                Where(x => enumIntvalues.Contains(x.Action));
            }
        );
            return this;
        }

        public IAuditSeeker ByDate(string value)
        {
            FilterDateRange(value, (minValue, maxValue) => Where(x => x.Date >= minValue && x.Date <= maxValue),
                        (maxValue) => Where(x => x.Date <= maxValue),
                        (minValue) => Where(x => x.Date >= minValue));
            return this;
        }

        public IAuditSeeker ByEntityId(string value)
        {
            FilterIntRange(value, (minValue, maxValue) => Where(x => x.EntityId >= minValue && x.EntityId <= maxValue),
                        (maxValue) => Where(x => x.EntityId <= maxValue),
                        (minValue) => Where(x => x.EntityId >= minValue));
            return this;
        }
    }
}