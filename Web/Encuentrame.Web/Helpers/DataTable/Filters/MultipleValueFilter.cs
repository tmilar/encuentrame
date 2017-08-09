using System.Collections.Generic;
using System.Text;
using Encuentrame.Web.Helpers.DataTable.Filters.Interfaces;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal abstract class MultipleValueFilter <T, TFilterProperty, TProperty> : BaseFilter<T>,
        ITableMulipleValueFilter<T, TFilterProperty, TProperty>
        where T : class
        where TFilterProperty : IList<TProperty>, new()
    {
        #region Properties

        protected virtual char StringSeparator { get { return '|'; } }

        public abstract bool CanAddDefaultValue { get; }
        public bool ShowGroups { get; set; }
        public bool IsMultiple { get; set; }

        public TFilterProperty DefaultValues { get; set; }

        public string DefaultValue
        {
            get
            {
                if (this.CanAddDefaultValue)
                {
                    var stringValue = this.ElementToString(this.DefaultValues[0]);
                    for (var index = 1; index < this.DefaultValues.Count; index++)
                        stringValue = string.Format("{0}{1}{2}", stringValue, this.StringSeparator,
                            this.ElementToString(this.DefaultValues[index]));

                    return stringValue;
                }
                return string.Empty;
            }
        }

        #endregion

        #region ITableMulipleValueFilter<T,TFilterProperty,TProperty> Members

        public virtual ITableMulipleValueFilter<T, TFilterProperty, TProperty> AddDefaultValue(TFilterProperty values)
        {
            this.DefaultValues = values;
            return this;
        }

        public virtual ITableMulipleValueFilter<T, TFilterProperty, TProperty> AddDefaultValue(TProperty value)
        {
            if (this.DefaultValues == null)
                this.DefaultValues = new TFilterProperty();
            this.DefaultValues.Add(value);
            return this;
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

        #endregion

        protected abstract string ElementToString(TProperty element);
    }
}