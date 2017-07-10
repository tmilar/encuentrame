using System;
using System.Xml.Schema;

namespace Encuentrame.Model.Accounts.Permissions
{
    [AttributeUsage(AttributeTargets.Field,AllowMultiple = false)]
    public class ModuleParentAttribute : Attribute
    {
        public ModulesEnum[] Modules { get; protected set; }
        public ModuleParentAttribute(params ModulesEnum[] modules)
        {
            this.Modules = modules;
        }
    }
}