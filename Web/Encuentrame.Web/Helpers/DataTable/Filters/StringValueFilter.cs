using System.Text;
using System.Web.Mvc;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class StringValueFilter<T> : ValueFilter<T, string> where T : class
    {
        public override void Build(ColumnData columnData, HtmlHelper helper)
        {
            var filter = new StringBuilder();

            filter.Append("<input type=\"text\" data-index=\"{0}\" data-name=\"{1}\" class=\"form-control\"".FormatWith(columnData.Index, columnData.Name));
            AppendDefaultValue(filter);            
            filter.Append("/>");

            this.Build(filter.ToString(), "input-filter", helper);
        }

        protected override bool CanAddDefaultValue
        {
            get { return !string.IsNullOrEmpty(DefaultValue); }
        }

        protected override string DefaultValueToString
        {
            get { return DefaultValue; }
        }
    }
}