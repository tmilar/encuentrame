using System;
using System.Linq;
using System.Text;
using System.Web;
using Encuentrame.Web.Helpers.DataTable.Filters.Interfaces;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal abstract class ValueFilter<T, TFilterProperty> : BaseFilter<T>,
        ITableSingleValueFilter<T, TFilterProperty> where T : class
    {
        #region Properties

        public TFilterProperty DefaultValue { get; set; }

        public abstract bool CanAddDefaultValue { get; }
        public abstract string DefaultValueToString { get; }

        #endregion

        #region ITableSingleValueFilter<T,TFilterProperty> Members

        public virtual ITableSingleValueFilter<T, TFilterProperty> AddDefaultValue(TFilterProperty value)
        {
            this.DefaultValue = value;
            return this;
        }

        public ITableSingleValueFilter<T, TFilterProperty> AddClass(string cssClass)
        {
            this.CssClass = cssClass;
            return this;
        }

        #endregion
    }
}