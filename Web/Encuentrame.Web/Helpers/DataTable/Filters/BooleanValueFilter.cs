using System.Text;
using System.Web.Mvc;
using Encuentrame.Web.Helpers.DataTable.Filters.Interfaces;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class BooleanValueFilter<T> : ValueFilter<T, bool>, IBooleanFilter where T : class
    {
        public override ITableSingleValueFilter<T, bool> AddDefaultValue(bool value)
        {
            this.DefaultValueSet = true;
            return base.AddDefaultValue(value);
        }

        public bool DefaultValueSet { get; set; }

        public override bool CanAddDefaultValue
        {
            get { return this.DefaultValueSet; }
        }

        public override string DefaultValueToString
        {
            get { return DefaultValue.ToString().ToLower(); }
        }

        public override string DisplayTemplate { get { return typeof(IBooleanFilter).Name; } }
    }
}