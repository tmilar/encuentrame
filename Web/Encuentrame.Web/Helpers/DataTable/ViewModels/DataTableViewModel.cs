using System.Collections.Generic;
using Encuentrame.Web.Helpers.DataTable.Filters;

namespace Encuentrame.Web.Helpers.DataTable.ViewModels
{
    public class DataTableViewModel
    {
        public Dictionary<string, ColumnData> Columns { get; set; }
        public Dictionary<string, ColumnData> ChildColumns { get; set; }
        public IList<IBaseFilter> Filters { get; set; }
        public IList<IBaseFilter> ChildFilters { get; set; }
        public string ChildTableClass { get; set; }
        public string TableId { get; set; }
        public bool ServerSide { get; set; }
        public string ClassName { get; set; }
        public bool SearchEnabled { get; set; }
        public string Url { get; set; }
        public int? SortByColumnIndex { get; set; }
        public string DefaultSortOrder { get; set; }
        public string ParentPropertyName { get; set; }
        public bool HasChildTable { get; set; }
        public bool AddFooter { get; set; }
        public string ChildPropertyName { get; set; }
        public bool DeferredSearch { get; set; }
        public int[] Lengths { get; set; }
        public int? DefaultLength { get; set; }
        public bool ShowAllOption { get; set; }
        public bool AllowSelection { get; set; }
        public DataTableViewModel ChildTable { get; set; }
    }
}