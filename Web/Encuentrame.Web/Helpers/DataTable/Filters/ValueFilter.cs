using System;
using System.Linq;
using System.Text;
using System.Web;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal abstract class ValueFilter<T, TFilterProperty> : BaseFilter<T>, ITableSingleValueFilter<T, TFilterProperty> where T : class
    {
        public TFilterProperty DefaultValue { get; set; }

        public virtual ITableSingleValueFilter<T, TFilterProperty> AddDefaultValue(TFilterProperty value)
        {
            DefaultValue = value;
            return this;
        }

        public ITableSingleValueFilter<T, TFilterProperty> AddClass(string cssClass)
        {
            this.CssClass = cssClass;
            return this;
        }

        protected abstract bool CanAddDefaultValue { get; }
        protected abstract string DefaultValueToString { get; }

        protected virtual void AppendDefaultValue(StringBuilder filter)
        {
            if (CanAddDefaultValue)
                filter.Append("data-default-value='{0}'".FormatWith(DefaultValueToString));
        }
    }
}