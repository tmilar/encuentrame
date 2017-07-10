namespace Encuentrame.Model.Accounts.Permissions
{
    public enum ActionsEnum
    {
        [ModuleParent( ModulesEnum.ManageRole, ModulesEnum.ManageUser, ModulesEnum.ManageAudit, ModulesEnum.EmailServerConfiguration, ModulesEnum.NotificationsConfiguration)]
        Create=1,
        [ModuleParent(ModulesEnum.ManageRole, ModulesEnum.ManageUser, ModulesEnum.ManageAudit, ModulesEnum.EmailServerConfiguration, ModulesEnum.NotificationsConfiguration)]
        Edit =2,
        [ModuleParent(ModulesEnum.ManageRole, ModulesEnum.ManageUser, ModulesEnum.ManageAudit, ModulesEnum.EmailServerConfiguration, ModulesEnum.NotificationsConfiguration)]
        Delete = 3,
        [ModuleParent(ModulesEnum.ManageRole, ModulesEnum.ManageUser, ModulesEnum.ManageAudit, ModulesEnum.EmailServerConfiguration, ModulesEnum.NotificationsConfiguration)]
        List =4,
        [ModuleParent(ModulesEnum.Profile)]
        View=5,
      
    }
}
