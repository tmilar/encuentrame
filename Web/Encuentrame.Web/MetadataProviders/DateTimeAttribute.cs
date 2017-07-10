using System;

namespace Encuentrame.Web.MetadataProviders
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class DateTimeAttribute : Attribute
    {
        /// <summary>
        /// Tipo de la clase fuente de datos
        /// </summary>
        public bool CurrentIsMaxDateTime { get; set; }

        public string TemplateHint { get; set; }

        public DateTimeAttribute()
        {
            this.TemplateHint = "datetime";
            CurrentIsMaxDateTime = false;
        }
    }
}