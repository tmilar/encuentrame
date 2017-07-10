using System.Text;
using System.Web.Mvc;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class RangeFilter<T, TProperty> : RangeValueFilter<T, TProperty> where T : class
    {
       
        public override void Build(ColumnData columnData, HtmlHelper helper)
        {
            var from = TranslationsHelper.Get("From") ?? "From";
            var to = TranslationsHelper.Get("to") ?? "to";
            var filter = new StringBuilder();
            
            filter.Append("<div class='range row' data-index=\"{0}\" data-name=\"{1}\">".FormatWith(columnData.Index, columnData.Name));
                filter.Append("<div class=\"col-md-6\">");
                    filter.Append("<div class=\"form-group \">");
                        filter.AppendFormat("<input class='range-min form-control' placeholder='{0}' ",from);
                            this.AppendMinValue(filter);
                        filter.Append(">");
                    filter.Append("</div>");
                filter.Append("</div>");
                filter.Append("<div class=\"col-md-6\">");
                    filter.Append("<div class=\"form-group \">");
                        filter.AppendFormat("<input class='range-max form-control' placeholder='{0}' ", to);
                        this.AppendMaxValue(filter);
                        filter.Append(">");
                    filter.Append("</div>");
                filter.Append("</div>");
            filter.Append("</div>");

            this.Build(filter.ToString(), "range-filter", helper);
        }        

        protected override string ElementToString(TProperty element)
        {
            return element.ToString();
        }
    }
}