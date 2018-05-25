using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encuentrame.Support.Email.Templates
{
    public class BaseTemplate : RazorGenerator.Templating.RazorTemplateBase
    {
        public dynamic Model { get; set; }
    }
}
