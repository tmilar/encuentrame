using System.Text;
using System.Web.Mvc;
using Encuentrame.Web.Helpers.DataTable.Filters.Interfaces;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class RangeFilter<T, TProperty> : RangeValueFilter<T, TProperty>, IRangeFilter where T : class
    {
        protected override string ElementToString(TProperty element)
        {
            return element.ToString();
        }
        public override string DisplayTemplate { get { return typeof(IRangeFilter).Name; } }
    }
}