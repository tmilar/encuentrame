using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using Encuentrame.Web.Helpers.DataTable.Filters.Interfaces;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class ReferenceValueFilter<T, TElement> : MultipleValueFilter<T, List<TElement>, TElement>, IReferenceFilter where T : class
    {
        #region Properties

        public string IdPath { get; set; }
        public string Url { get; set; }

        public bool DefaultValueSet { get; set; }

        public override bool CanAddDefaultValue { get { return this.DefaultValueSet; } }
        public override string DisplayTemplate { get { return typeof(IReferenceFilter).Name; } }
        #endregion

        #region Methods

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

        protected override string ElementToString(TElement element)
        {
            return element.ToString();
        }
        #endregion
    }
}