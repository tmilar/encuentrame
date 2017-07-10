using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Encuentrame.Web.MetadataProviders
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class EnumReferenceAttribute : Attribute
    {
        public string TemplateHint { get; set; }
        public int[] ExcludeValues { get; set; }
        public int[] IncludeValues { get; set; }

        private bool _multipleSelection;

        public bool MultipleSelection
        {
            get { return _multipleSelection; }
            set
            {
                _multipleSelection = value;
                if (value)
                {
                    TemplateHint = "enumMultiple";
                }
                else
                {
                    TemplateHint = "enum";
                }
            }
        }

        public EnumReferenceAttribute()
        {
            TemplateHint = "enum";
        }
    }
}