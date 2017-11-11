using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Encuentrame.Support.Email.Templates.EmailModels
{
    public class ObjectCreatedEmailModel
    {
        public string Username { get; set; }        
        public string ObjectTypeCreated { get; set; }
        public string Description { get; set; }
    }
}
