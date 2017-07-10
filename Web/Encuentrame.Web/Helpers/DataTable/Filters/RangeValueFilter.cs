using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal abstract class RangeValueFilter<T, TProperty> : BaseFilter<T>, ITablRangeValueFilter<T, TProperty>
        where T : class                                                                                            
    {        
        public TProperty DefaultMinValue { get; set; }
        public TProperty DefaultMaxValue { get; set; }
        protected bool DefaultMinValueSet { get; set; }
        protected bool DefaultMaxValueSet { get; set; }

        protected abstract string ElementToString(TProperty element);

        public RangeValueFilter()
        {
            Width = 4;
        }
        public ITablRangeValueFilter<T, TProperty> AddDefaultValue(TProperty min, TProperty max)
        {
            AddDefaultMin(min);
            AddDefaultMax(max);
            return this;
        }

        public ITablRangeValueFilter<T, TProperty> AddDefaultMin(TProperty min)
        {
            DefaultMinValue = min;
            DefaultMinValueSet = true;
            return this;
        }

        public ITablRangeValueFilter<T, TProperty> AddDefaultMax(TProperty max)
        {
            DefaultMaxValue = max;
            DefaultMaxValueSet = true;
            return this;
        }

        public ITablRangeValueFilter<T, TProperty> AddCssClass(string cssClass)
        {
            this.CssClass = cssClass;
            return this;
        }

        protected void AppendMinValue(StringBuilder filter)
        {
            if (DefaultMinValueSet)
            {
                filter.Append("value='{0}'".FormatWith(ElementToString(DefaultMinValue)));
            }
        }

        protected void AppendMaxValue(StringBuilder filter)
        {
            if (DefaultMaxValueSet)
            {
                filter.Append("value='{0}'".FormatWith(ElementToString(DefaultMaxValue)));
            }
        }
    }
}