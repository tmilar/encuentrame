using System;
using System.Text;
using System.Web.Mvc;
using Encuentrame.Web.Helpers.DataTable.Filters.Interfaces;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class DateRangeFilter<T> : RangeValueFilter<T, DateTime>, IDateRangeFilter where T : class
    {
        protected override string ElementToString(DateTime element)
        {
            return element.ToString("dd/MM/yyyy HH:mm");
        }

        public override string DisplayTemplate { get { return typeof(IDateRangeFilter).Name; } }
    }
}