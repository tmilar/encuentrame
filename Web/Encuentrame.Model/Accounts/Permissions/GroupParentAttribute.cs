using System;

namespace Encuentrame.Model.Accounts.Permissions
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple = false)]
    public class GroupParentAttribute : Attribute
    {
        public GroupsOfModulesEnum[] Groups { get;protected set; }
        public GroupParentAttribute(params GroupsOfModulesEnum[] groups)
        {
            this.Groups = groups;
        }
    }
}