using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encuentrame.Support.Email.Templates.EmailModels
{
    public class WelcomeUserEmailModel
    {
        public string Username { get; set; }
        public string Site { get; set; }
        public string WelcomeInstructions { get; set; }
    }
}
