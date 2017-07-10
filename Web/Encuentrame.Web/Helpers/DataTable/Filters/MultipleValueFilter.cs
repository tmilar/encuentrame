using System.Collections.Generic;
using System.Text;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal abstract class MultipleValueFilter<T, TFilterProperty, TProperty> : BaseFilter<T>, ITableMulipleValueFilter<T, TFilterProperty, TProperty>
        where T : class
        where TFilterProperty : IList<TProperty>, new()                                                                                                                                  
    {
        protected virtual char StringSeparator { get { return '|'; } }

        protected abstract bool CanAddDefaultValue { get; }
        public bool ShowGroups { get; set; }
        protected abstract string ElementToString(TProperty element);
        public bool IsMultiple { get; set; }

        public TFilterProperty DefaultValues { get; set; }

        public virtual ITableMulipleValueFilter<T, TFilterProperty, TProperty> AddDefaultValue(TFilterProperty values)
        {
            this.DefaultValues = values;
            return this;
        }

        public virtual ITableMulipleValueFilter<T, TFilterProperty, TProperty> AddDefaultValue(TProperty value)
        {
            if(this.DefaultValues==null)
                this.DefaultValues = new TFilterProperty();
            this.DefaultValues.Add(value);
            return this;
        }

        protected virtual void AppendProperties(StringBuilder filter)
        {
            if (this.IsMultiple)
                filter.Append("multiple='multiple' data-is-multiple ='{0}'".FormatWith(this.IsMultiple.ToString().ToLower()));

            if (this.ShowGroups)
                filter.Append(" data-show-groups='{0}' ".FormatWith(ShowGroups.ToString().ToLower()));

            if (this.CanAddDefaultValue)
            {
                var stringValue = this.ElementToString(this.DefaultValues[0]);
                for (int index = 1; index < this.DefaultValues.Count; index++)
                {
                    stringValue = string.Format("{0}{1}{2}", stringValue, this.StringSeparator,
                        this.ElementToString(this.DefaultValues[index]));
                }

                filter.Append("data-default-value='{0}'".FormatWith(stringValue));
            }
        }

        public ITableMulipleValueFilter<T, TFilterProperty, TProperty> AllowMultiple()
        {
            this.IsMultiple = true;
            return this;
        }

        public ITableMulipleValueFilter<T, TFilterProperty, TProperty> HasGroups()
        {
            this.ShowGroups = true;
            return this;
        }        

        public ITableMulipleValueFilter<T, TFilterProperty, TProperty> AddCssClass(string cssClass)
        {
            this.CssClass = cssClass;
            return this;
        }
    }
}