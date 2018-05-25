using System;

namespace Encuentrame.Web.MetadataProviders
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ReferenceMultipleAttribute : Attribute
    {
        /// <summary>
        /// Tipo de la clase fuente de datos
        /// </summary>
        public Type SourceType { get; set; }
        /// <summary>
        /// Nombre del metodo en la clase fuente de datos
        /// </summary>
        public string SourceName { get; set; }

        public string TemplateHint { get; set; }

        public string Parameters { get; set; }

        public string RelatedTo { get; set; }

        public string SourceController { get; set; }

        public string SourceAction { get; set; }

        private bool _multipleType;
        public bool MultipleType
        {
            get { return _multipleType; }
            set
            {
                _multipleType = value;
                if (value)
                {
                    TemplateHint = "referencemultipleAny";
                }
                else
                {
                    TemplateHint = "referencemultiple";
                }
            }
        }

        public ReferenceMultipleAttribute()
        {
            TemplateHint = "referencemultiple";
        }
    }
}