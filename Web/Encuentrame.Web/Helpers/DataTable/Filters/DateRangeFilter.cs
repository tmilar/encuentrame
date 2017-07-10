using System;
using System.Text;
using System.Web.Mvc;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class DateRangeFilter<T> : RangeValueFilter<T, DateTime> where T : class
    {
        public override void Build(ColumnData columnData, HtmlHelper helper)
        {
            var from = TranslationsHelper.Get("From") ?? "From";
            var to = TranslationsHelper.Get("to") ?? "to";
            var filter = new StringBuilder();
            
            filter.Append("<div class='date-range row' data-index=\"{0}\" data-name=\"{1}\">".FormatWith(columnData.Index, columnData.Name));
                filter.Append("<div class=\"col-md-6\">");
                    filter.Append("<div class=\"form-group\">");
                        filter.AppendFormat("<input class='range-min datetime-control form-control' data-current-is-max-datetime='false' placeholder='{0}'",from);
                            this.AppendMinValue(filter);
                        filter.Append(">");
                    filter.Append("</div>");
                filter.Append("</div>");
                filter.Append("<div class=\"col-md-6\">");
                    filter.Append("<div class=\"form-group \">");
                        filter.AppendFormat("<input class='range-max datetime-control form-control' data-current-is-max-datetime='false' placeholder='{0}'",to);
                            this.AppendMaxValue(filter);
                        filter.Append(">");
                    filter.Append("</div>");
                filter.Append("</div>");
            filter.Append("</div>");

            this.Build(filter.ToString(), "date-range-filter", helper);
        }

        protected override string ElementToString(DateTime element)
        {
            return element.ToString("dd/MM/yyyy HH:mm");
        }
    }
}