namespace Encuentrame.Model.Supports.Audits.Seekers
{
    public interface IAuditSeeker : ISeeker<Audit>
    {
        IAuditSeeker OrderByEntityId(SortOrder sortOrder);
        IAuditSeeker OrderByAuditBehaviorType(SortOrder sortOrder);
        IAuditSeeker OrderByUser(SortOrder sortOrder);
        IAuditSeeker OrderByEntityType(SortOrder sortOrder);
        IAuditSeeker OrderByDate(SortOrder sortOrder);

        IAuditSeeker ByEntityType(string value);
        IAuditSeeker ByUser(string value);
        IAuditSeeker ByAuditBehaviorType(string value);
        IAuditSeeker ByEntityId(string value);
        IAuditSeeker ByDate(string value);
    }
}
