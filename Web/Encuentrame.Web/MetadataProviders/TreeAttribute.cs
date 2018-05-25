using System;

namespace Encuentrame.Web.MetadataProviders
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class TreeAttribute : Attribute
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

        public TreeAttribute()
        {
            this.TemplateHint = "tree";
        }
    }
}