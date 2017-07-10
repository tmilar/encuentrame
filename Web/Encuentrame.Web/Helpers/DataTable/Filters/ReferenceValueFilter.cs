using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class ReferenceValueFilter<T, TElement> : MultipleValueFilter<T, List<TElement>, TElement> where T : class
    {
        public string IdPath { get; set; }
        public string Url { get; set; }
        
        public override void Build(ColumnData columnData, HtmlHelper helper)
        {
            var filter = new StringBuilder();

            filter.Append("<select class='select-reference-filter form-control reference reference-single' data-index=\"{0}\" data-name=\"{1}\" data-url=\"{2}\" data-id-path=\"{3}\""
                        .FormatWith(columnData.Index, columnData.Name, this.Url, this.IdPath));            

            this.AppendProperties(filter);

            filter.Append(" >");
            if (!this.IsMultiple)
                filter.AppendLine("<option data-id=\"{0}\" value=\" \"></option>".FormatWith(-1));

            filter.AppendLine("</select>");

            this.Build(filter.ToString(), "reference-filter", helper);
        }

        public bool DefaultValueSet { get; set; }

        protected override bool CanAddDefaultValue
        {
            get { return this.DefaultValueSet; }
        }

        protected override string ElementToString(TElement element)
        {
            return element.ToString();
        }

        public override ITableMulipleValueFilter<T, List<TElement>, TElement> AddDefaultValue(List<TElement> values)
        {
            this.DefaultValueSet = true;
            base.AddDefaultValue(values);
            return this;
        }

        public override ITableMulipleValueFilter<T, List<TElement>, TElement> AddDefaultValue(TElement value)
        {
            this.DefaultValueSet = true;
            base.AddDefaultValue(value);
            return this;
        }
    }
}