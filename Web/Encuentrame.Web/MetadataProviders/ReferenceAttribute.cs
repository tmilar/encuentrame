using System;
using Encuentrame.Support;

namespace Encuentrame.Web.MetadataProviders
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ReferenceAttribute : Attribute
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

        public string RelatedTo { get; set; }

        private string[] relatedToFields;
        public string[] RelatedToFields
        {
            get
            {
                return relatedToFields;
            }
            set
            {
                relatedToFields= value;
                RelatedTo = relatedToFields.Join("|");
            }
        }

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
                    TemplateHint = "referenceAny";
                }
                else
                {
                    TemplateHint = "reference";
                }
            }
        }

        public ReferenceAttribute()
        {
            TemplateHint = "reference";
        }
    }
}