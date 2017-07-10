namespace Encuentrame.Model.Accounts.Permissions
{
    public enum ModulesEnum
    {
        [GroupParent(GroupsOfModulesEnum.Security)]
        ManageUser=31,
        [GroupParent(GroupsOfModulesEnum.Security)]
        ManageRole = 32,
        [GroupParent(GroupsOfModulesEnum.Security)]
        ManageAudit = 33,
        
        [GroupParent(GroupsOfModulesEnum.Account)]
        Profile=42,
       
       
        [GroupParent(GroupsOfModulesEnum.Settings)]
        EmailServerConfiguration = 58,
        [GroupParent(GroupsOfModulesEnum.Settings)]
        NotificationsConfiguration = 59,


      
    }
}