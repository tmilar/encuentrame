using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class EnumFilter<T, TEnum> : MultipleValueFilter<T, List<TEnum>, TEnum>
                                                        where T : class
                                                        where TEnum : struct
    {
        public override void Build(ColumnData columnData, HtmlHelper helper)
        {
            var filter = new StringBuilder();

            filter.Append("<select class='select-enum-filter form-control reference reference-single' data-index=\"{0}\" data-name=\"{1}\""
                            .FormatWith(columnData.Index, columnData.Name));

            AppendProperties(filter);
            filter.Append(">");

            if (!this.IsMultiple)
                filter.AppendLine("<option value=\"{0}\"></option>".FormatWith(-1));

            foreach (var value in Enum.GetValues(this.EnumType))
            {
                var name = Enum.GetName(this.EnumType, value);
                int intValue = (int)Enum.Parse(this.EnumType, name);
                var text = TranslationsHelper.Get(this.EnumType.Name, name) ?? name;
                filter.AppendLine("<option value=\"{0}\">{1}</option>".FormatWith(intValue, text));
            }
            
            filter.AppendLine("</select>");

            this.Build(filter.ToString(), "enum-filter", helper);
        }

        public Type EnumType { get { return typeof(TEnum); } }        

        protected override bool CanAddDefaultValue
        {
            get { return this.DefaultValueSet; }
        }

        protected override string ElementToString(TEnum element)
        {
            var name = Enum.GetName(this.EnumType, element);
            int intValue = (int)Enum.Parse(this.EnumType, name);
            return intValue.ToString();
        }

        public override ITableMulipleValueFilter<T, List<TEnum>, TEnum> AddDefaultValue(List<TEnum> values)
        {
            this.DefaultValueSet = true;
            base.AddDefaultValue(values);
            return this;
        }

        public bool DefaultValueSet { get; set; }

        public override ITableMulipleValueFilter<T, List<TEnum>, TEnum> AddDefaultValue(TEnum value)
        {
            this.DefaultValueSet = true;
            base.AddDefaultValue(value);
            return this;
        }
    }
}