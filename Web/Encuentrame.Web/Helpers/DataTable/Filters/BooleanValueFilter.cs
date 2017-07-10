using System.Text;
using System.Web.Mvc;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class BooleanValueFilter<T> : ValueFilter<T, bool> where T : class
    {
        public override void Build(ColumnData columnData, HtmlHelper helper)
        {
            var filter = new StringBuilder();
            var yesText = TranslationsHelper.Get("Yes") ?? "Yes";
            var noText = TranslationsHelper.Get("No") ?? "No";
            filter.AppendLine("<div class=\"controls\"");
            AppendDefaultValue(filter);
            filter.Append(">");

            filter.Append("<input type=\"checkbox\" data-index=\"{0}\" data-name=\"{1}\" data-value=\"true\">{2}</>".FormatWith(columnData.Index, columnData.Name, yesText));
            filter.Append("<input type=\"checkbox\" data-index=\"{0}\" data-name=\"{1}\" data-value=\"false\">{2}</>".FormatWith(columnData.Index, columnData.Name, noText));
            filter.AppendLine("</div>");
            this.Build(filter.ToString(), "boolean-filter", helper);
        }

        public override ITableSingleValueFilter<T, bool> AddDefaultValue(bool value)
        {
            this.DefaultValueSet = true;
            return base.AddDefaultValue(value);
        }

        public bool DefaultValueSet { get; set; }

        protected override bool CanAddDefaultValue
        {
            get { return this.DefaultValueSet; }
        }

        protected override string DefaultValueToString
        {
            get { return DefaultValue.ToString().ToLower(); }
        }
    }
}