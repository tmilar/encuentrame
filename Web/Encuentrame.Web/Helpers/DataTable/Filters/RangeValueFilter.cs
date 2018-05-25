using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Encuentrame.Web.Helpers.DataTable.Filters.Interfaces;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal abstract class RangeValueFilter<T, TProperty> : BaseFilter<T>, ITableRangeValueFilter<T, TProperty>
        where T : class
    {
        #region  Constructor

        public RangeValueFilter()
        {
            this.Width = 4;
        }

        #endregion

        #region Properties

        public TProperty DefaultMinValue { get; set; }
        public TProperty DefaultMaxValue { get; set; }
        protected bool DefaultMinValueSet { get; set; }
        protected bool DefaultMaxValueSet { get; set; }

        #endregion

        #region ITablRangeValueFilter<T,TProperty> Members

        public ITableRangeValueFilter<T, TProperty> AddDefaultValue(TProperty min, TProperty max)
        {
            this.AddDefaultMin(min);
            this.AddDefaultMax(max);
            return this;
        }

        public ITableRangeValueFilter<T, TProperty> AddDefaultMin(TProperty min)
        {
            this.DefaultMinValue = min;
            this.DefaultMinValueSet = true;
            return this;
        }

        public ITableRangeValueFilter<T, TProperty> AddDefaultMax(TProperty max)
        {
            this.DefaultMaxValue = max;
            this.DefaultMaxValueSet = true;
            return this;
        }

        public ITableRangeValueFilter<T, TProperty> AddCssClass(string cssClass)
        {
            this.CssClass = cssClass;
            return this;
        }

        #endregion

        protected abstract string ElementToString(TProperty element);

        public string DefaultMinStringValue
        {
            get
            {
                if (this.DefaultMinValueSet)
                    return this.ElementToString(this.DefaultMinValue);
                return string.Empty;
            }
        }

        public string DefaultMaxStringValue
        {
            get
            {
                if (this.DefaultMaxValueSet)
                    return this.ElementToString(this.DefaultMaxValue);
                return string.Empty;
            }
        }
        public override string DisplayTemplate { get { return typeof(IBooleanFilter).Name; } }
    }
}