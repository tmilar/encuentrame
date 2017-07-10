using System;
using System.Text;
using System.Web.Mvc;

namespace Encuentrame.Web.Helpers.DataTable
{
    internal class ColumnData
    {
        public bool Visible { get; set; }
        public bool Sortable { get; set; }
        public int Index { get; set; }
        public bool HasDataAssociated { get; set; }
        public string Name { get; set; }
        public string Template { get; set; }
        public Type Type { get; set; }
        public bool IsReference { get; set; }
        public string NamePath { get; set; }
        public bool Totalize { get; set; }

        public void Build(HtmlHelper helper)
        {
            var thBuilder = new StringBuilder();
            thBuilder.Append("<th ");

            if (this.HasDataAssociated)
                thBuilder.Append(string.Format(" data-name=\"{0}\"", this.Name));

            thBuilder.Append(string.Format(" data-index=\"{0}\"", this.Index));

            if (!this.Visible)
                thBuilder.Append(string.Format(" data-visible=\"{0}\"", this.Visible.ToString().ToLower()));

            if (this.Totalize)
                thBuilder.Append(string.Format(" data-totalize=\"{0}\"", this.Totalize.ToString().ToLower()));

            if (!this.Sortable)
                thBuilder.Append(string.Format(" data-sortable=\"{0}\"", this.Sortable.ToString().ToLower()));

            if (!string.IsNullOrEmpty(this.Template))
                thBuilder.Append(string.Format(" data-th-template=\"{0}\"", this.Template));

            if (this.Type != null)
            {
                if (this.Type == typeof(DateTime?))
                {
                    thBuilder.Append(string.Format(" data-type=\"{0}\"", typeof(DateTime).Name));
                }
                else
                {
                    thBuilder.Append(string.Format(" data-type=\"{0}\"", this.Type.Name));
                }
               
            }
                

            if (this.IsReference)
            {
                thBuilder.Append(string.Format(" data-is-reference=\"{0}\"", this.IsReference.ToString().ToLower()));
                thBuilder.Append(string.Format(" data-name-path=\"{0}\"", this.NamePath));
            }

            thBuilder.Append(" >");

            helper.ViewContext.Writer.Write(thBuilder.ToString());
            var text = TranslationsHelper.Get(this.Name) ?? this.Name;
            helper.ViewContext.Writer.Write(text);
            helper.ViewContext.Writer.Write("</th>");
        }
    }
}