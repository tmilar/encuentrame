using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Encuentrame.Web.Helpers.DataTable.Filters.Interfaces;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class IntValueFilter <T> : ValueFilter<T, int>, IIntFilter where T : class
    {
        #region Properties

        public override bool CanAddDefaultValue{get { return this.DefaultValueSet; }}

        public override string DefaultValueToString { get { return this.DefaultValue.ToString(); } }
        public bool DefaultValueSet { get; set; }
        public override string DisplayTemplate { get { return typeof(IIntFilter).Name; } }
        #endregion

        public override ITableSingleValueFilter<T, int> AddDefaultValue(int value)
        {
            this.DefaultValueSet = true;
            return base.AddDefaultValue(value);
        }
    }
}