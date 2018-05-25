using System.Text;
using System.Web.Mvc;
using Encuentrame.Web.Helpers.DataTable.Filters.Interfaces;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class StringValueFilter <T> : ValueFilter<T, string>, IStringFilter where T : class
    {
        #region Properties

        public override bool CanAddDefaultValue{get { return !string.IsNullOrEmpty(this.DefaultValue); }}

        public override string DefaultValueToString { get { return this.DefaultValue; } }
        public override string DisplayTemplate { get { return typeof(IStringFilter).Name; } }
        #endregion
    }
}