using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Encuentrame.Web.Helpers.DataTable.Filters;
using Encuentrame.Web.Models.References.Commons;
using Encuentrame.Support;
using System.Linq;
using System.Web.Mvc.Html;
using Encuentrame.Web.Helpers.DataTable.Filters.Interfaces;
using Encuentrame.Web.Helpers.DataTable.ViewModels;

namespace Encuentrame.Web.Helpers.DataTable
{
    public static class HtmlDataTableHelper
    {
        public static ITableBuilder<T> BuildTable<T>(this HtmlHelper helper, string url) where T : class
        {
            return new HtmlDataTable<T>(helper, url, string.Format("Datatable_{0}", typeof(T).Name));
        }

        public static ITableBuilder<T> BuildTable<T>(this HtmlHelper helper, string url, string tableId) where T : class
        {
            return new HtmlDataTable<T>(helper, url, tableId);
        }

        class BaseHtmlDataTable
        {
            protected ITableBuilder parent;
            protected ITableBuilder childTable;
            public IList<IBaseFilter> Filters;
            public Dictionary<string, ColumnData> Columns;

            public BaseHtmlDataTable()
            {
                this.Columns = new Dictionary<string, ColumnData>();
                this.Filters = new List<IBaseFilter>();
            }
        }

        class HtmlDataTable<T> : BaseHtmlDataTable, ITableBuilder<T> where T : class
        {
            private int index = 0;
            private readonly HtmlHelper _helper;
            private readonly string url;
            private string tableId;
            private bool serverSide;
            private bool searchEnabled;
            private string childPropertyName;
            private bool addFooter;
            private string sortColumn;
            private DefaultOrder defaultOrder;
            private string parentPropertyName;
            private bool deferredSearch;
            private int[] lengths;
            private int? defaultLength;
            private bool showAllOption = true;
            private bool allowSelection;
            private DataTableViewModel tableViewModel;

            public DataTableViewModel TableViewModel
            {
                get
                {
                    if (this.tableViewModel == null)
                        this.tableViewModel = new DataTableViewModel();
                    return this.tableViewModel;
                }
            }
            public HtmlDataTable(HtmlHelper helper, string url, string tableId)
            {
                this.tableId = tableId;
                this.url = url;
                this._helper = helper;
                this.searchEnabled = true;
            }

            protected HtmlDataTable(HtmlHelper helper, string tableId, ITableBuilder parent, string url = null)
            {
                this.tableId = tableId;
                this._helper = helper;
                this.searchEnabled = true;
                this.parent = parent;
                this.url = url;
            }

            public DataTableViewModel BuildViewModel()
            {
                DataTableViewModel parentViewModel = null;
                if (this.parent != null)
                    parentViewModel = parent.BuildViewModel();
                else
                    BuildFilters();

                var className = this.parent == null ? "data-table-editor" : string.Empty;
                this.BuildTable(className);
                if (parentViewModel != null)
                {
                    parentViewModel.ChildTable = this.tableViewModel;
                }

                return parentViewModel ?? this.TableViewModel;
            }

            public MvcHtmlString Build()
            {
                var currentViewModel = BuildViewModel();
                return _helper.Partial(@"~/Views/Shared/_DataTableViewModel.cshtml", currentViewModel);
            }

            public ITableBuilder<T> InServer()
            {
                this.serverSide = true;
                this.deferredSearch = true;
                return this;
            }

            public ITableBuilder<T> ShowSearchButton()
            {
                deferredSearch = true;
                return this;
            }

            public ITableBuilder<T> DisableSearch()
            {
                this.searchEnabled = false;
                return this;
            }

            public ITableBuilder<T> PageLengths(int[] lengths, int? defaultLenght = null)
            {
                this.lengths = lengths;
                this.defaultLength = defaultLenght;
                if (defaultLenght.HasValue && !lengths.Contains(defaultLenght.Value))
                    this.defaultLength = null;
                return this;
            }

            public ITableBuilder<T> HideShowAllOption()
            {
                this.showAllOption = false;
                return this;
            }

            private void BuildFilters()
            {
                if (this.Filters.Count == 0 || !this.searchEnabled)
                    return;
                TableViewModel.Filters = this.Filters;
                var baseChildTable = this.childTable as BaseHtmlDataTable;
                if (baseChildTable != null && baseChildTable.Filters != null)
                    TableViewModel.ChildFilters = baseChildTable.Filters;

                TableViewModel.DeferredSearch = this.deferredSearch;

            }

            private void BuildTable(string className)
            {
                var childTableClass = this.parent != null ? "well" : string.Empty;

                TableViewModel.ClassName = className;
                TableViewModel.ChildTableClass = childTableClass;
                TableViewModel.TableId = this.tableId;
                TableViewModel.ServerSide = this.serverSide;
                TableViewModel.SearchEnabled = this.searchEnabled;
                TableViewModel.Url = this.url;
                TableViewModel.ChildPropertyName = this.childPropertyName;
                TableViewModel.Columns = this.Columns;
                TableViewModel.Lengths = this.lengths;
                TableViewModel.DefaultLength = this.defaultLength;
                TableViewModel.ShowAllOption = this.showAllOption;

                if (!string.IsNullOrEmpty(this.sortColumn))
                {
                    var sortOrder = this.Columns[this.sortColumn].Index;
                    var defaultOrderString = this.defaultOrder == DefaultOrder.Ascending ? "asc" : "desc";
                    TableViewModel.SortByColumnIndex = sortOrder;
                    TableViewModel.DefaultSortOrder = defaultOrderString;
                }

                TableViewModel.ParentPropertyName = this.parentPropertyName;

                TableViewModel.HasChildTable = this.childTable != null;
                TableViewModel.AddFooter = this.addFooter;
                TableViewModel.AllowSelection = this.allowSelection;
            }

            public ITableBuilder<T> AddColumn<TProperty>(Expression<Func<T, TProperty>> memberExpression, bool visible = true, bool sortable = true, string dataTemplate = "", bool totalize = false)
            {
                this.CreateColumnData(memberExpression, visible, sortable, dataTemplate, totalize);
                return this;
            }

            public ITableBuilder<T> SortBy<TProperty>(Expression<Func<T, TProperty>> memberExpression, DefaultOrder defaultOrder = DefaultOrder.Ascending)
            {
                this.sortColumn = this.GetPropertyName(memberExpression);
                this.defaultOrder = defaultOrder;
                return this;
            }

            public ITableBuilder<T> AllowSelection()
            {
                this.allowSelection = true;
                return this;
            }

            public ITableBuilder<T> AddColumn<TProperty>(Expression<Func<T, TProperty>> memberExpression,
                Expression<Func<TProperty, string>> nameExpression,
                bool visible = true,
                bool sortable = true,
                string dataTemplate = "", 
                bool totalize = false) where TProperty : class
            {
                var columnData = this.CreateColumnData(memberExpression, visible, sortable, dataTemplate, totalize);
                columnData.IsReference = true;
                var nameProperty = Reflect.GetProperty(nameExpression);
                columnData.NamePath = nameProperty.Name;

                return this;
            }

            public ITableBuilder<T> AddColumn<TReference>(Expression<Func<T, IList<TReference>>> memberExpression,
                Expression<Func<TReference, string>> nameExpression,
                bool visible = true,
                bool sortable = true,
                string dataTemplate = "",
                bool totalize = false) where TReference : class
            {
                var columnData = this.CreateColumnData(memberExpression, visible, sortable, dataTemplate, totalize);
                columnData.IsReference = true;
                var nameProperty = Reflect.GetProperty(nameExpression);
                columnData.NamePath = nameProperty.Name;

                return this;
            }

            public ITableBuilder<T> AddColumn(string key, bool visible = true, bool sortable = true, string dataTemplate = "", bool totalize = false)
            {
                var columnData = this.CreateColumnData(key, visible, sortable, dataTemplate, null, totalize);
                columnData.HasDataAssociated = false;
                return this;
            }

            private ColumnData CreateColumnData<TProperty>(Expression<Func<T, TProperty>> memberExpression, bool visible = true, bool sortable = true, string dataTemplate = "", bool totalize=false)
            {
                this.addFooter = totalize || addFooter;

                var columnData = this.CreateColumnData(this.GetPropertyName(memberExpression), visible, sortable, dataTemplate, typeof(TProperty), totalize);
                columnData.HasDataAssociated = true;
                return columnData;
            }

            private ColumnData CreateColumnData(string name, bool visible = true, bool sortable = true, string dataTemplate = "", Type type = null, bool totalize =false)
            {
                var columnData = new ColumnData();
                columnData.Name = name;
                columnData.Visible = visible;
                columnData.Sortable = sortable;
                columnData.Index = this.index++;
                columnData.Template = dataTemplate;
                columnData.HasDataAssociated = true;
                columnData.Type = type;
                columnData.Totalize = totalize;
                this.Columns.Add(columnData.Name, columnData);

                var property = typeof(T).GetProperty(name);
                if (property != null)
                {
                    var displayAttributes = property.GetCustomAttributes(typeof(DisplayAttribute), false);
                    var displayAttribute = displayAttributes.Length > 0
                        ? displayAttributes.First() as DisplayAttribute
                        : null;
                    if (displayAttribute != null)
                    {
                        columnData.TranslationKey = displayAttribute.Name;
                    }

                }
                return columnData;
            }

            public ITableSingleValueFilter<T, string> AddFilter(Expression<Func<T, string>> memberExpression)
            {
                return this.CreateFilter<StringValueFilter<T>, string>(memberExpression);
            }

            public ITableSingleValueFilter<T, int> AddExactMatchFilter(Expression<Func<T, int>> memberExpression)
            {
                return this.CreateFilter<IntValueFilter<T>, int>(memberExpression);
            }

            public ITableBuilder<T> AddFilter(Expression<Func<T, ReferenceAny>> memberExpression)
            {
                var filter = new PropertyBasedFilter<T, ReferenceAny>();
                filter.MemberExpression = memberExpression;
                filter.Name = this.GetPropertyName(memberExpression);
                filter.ForChildTable = parent != null;
                this.Filters.Add(filter);
                filter.SetTableBuilder(this);

                var property = typeof(T).GetProperty(filter.Name);
                if (property != null)
                {
                    var displayAttributes = property.GetCustomAttributes(typeof(DisplayAttribute), false);
                    var displayAttribute = displayAttributes.Length > 0
                        ? displayAttributes.First() as DisplayAttribute
                        : null;
                    if (displayAttribute != null)
                    {
                        filter.TranslationKey = displayAttribute.Name;
                    }
                }

                return filter;
            }

            public ITableSingleValueFilter<T, bool> AddFilter(Expression<Func<T, bool>> memberExpression)
            {
                return this.CreateFilter<BooleanValueFilter<T>, bool>(memberExpression);
            }

            public ITableRangeValueFilter<T, int> AddFilter(Expression<Func<T, int>> memberExpression)
            {
                return this.CreateFilter<RangeFilter<T, int>, int>(memberExpression);
            }

            public ITableRangeValueFilter<T, decimal> AddFilter(Expression<Func<T, decimal>> memberExpression)
            {
                return this.CreateFilter<RangeFilter<T, decimal>, decimal>(memberExpression);
            }

            public ITableRangeValueFilter<T, DateTime> AddFilter(Expression<Func<T, DateTime>> memberExpression)
            {
                return this.CreateFilter<DateRangeFilter<T>, DateTime>(memberExpression);
            }

            public ITableMulipleValueFilter<T, List<TEnum>, TEnum> AddFilter<TEnum>(Expression<Func<T, TEnum>> memberExpression) where TEnum : struct
            {
                if (!typeof (TEnum).IsEnum)
                {
                    var nullFilter = new EnumFilter<T, TEnum>();
                    nullFilter.SetTableBuilder(this);
                    return nullFilter;
                }                    

                var filter = this.CreateFilter<EnumFilter<T, TEnum>, TEnum>(memberExpression);
                return filter;
            }

            public ITableBuilder<TChild> ChildTable <TChild>(Expression<Func<T, IEnumerable<TChild>>> memberExpression) where TChild : class
            {
                var name = GetPropertyName(memberExpression);                
                var child = new HtmlDataTable<TChild>(_helper, string.Format("{0}_Details_{1}", tableId, name), this);
                this.childPropertyName = name;
                childTable = child;
                return child;
            }

            public ITableBuilder<TChild> ChildTable<TChild>(Expression<Func<T, int>> parentPropertyExpression, string url) where TChild : class
            {
                var name = typeof(TChild).Name;
                this.parentPropertyName = GetPropertyNameFrom<T, int>(parentPropertyExpression);
                var child = new HtmlDataTable<TChild>(_helper, string.Format("{0}_Details_{1}", tableId, name), this, url);
                child.InServer();
                childTable = child;
                deferredSearch = true;
                return child;
            }

            public ITableMulipleValueFilter<T, List<int>, int> AddFilter<TProperty>(Expression<Func<T, TProperty>> memberExpression, Expression<Func<TProperty, int>> idExpression, string filterUrl) where TProperty : class
            {
                return this.CreateReferenceFilter<ReferenceValueFilter<T, int>, TProperty, TProperty, int>(memberExpression, idExpression, filterUrl);
            }

            public ITableMulipleValueFilter<T, List<string>, string> AddFilter<TProperty>(Expression<Func<T, TProperty>> memberExpression, Expression<Func<TProperty, string>> idExpression, string filterUrl) where TProperty : class
            {
                return this.CreateReferenceFilter<ReferenceValueFilter<T, string>, TProperty, TProperty, string>(memberExpression, idExpression, filterUrl);
            }
            
            public ITableMulipleValueFilter<T, List<int>, int> AddFilter<TReference>(Expression<Func<T, IList<TReference>>> memberExpression, Expression<Func<TReference, int>> idExpression, string filterUrl) where TReference : class
            {
                var filter = this.CreateReferenceFilter<ReferenceValueFilter<T, int>, IList<TReference>, TReference, int>(memberExpression, idExpression, filterUrl);
                filter.AllowMultiple();
                
                return filter;
            }

            private TFilter CreateReferenceFilter<TFilter, TProperty, TReference, TElement>(Expression<Func<T, TProperty>> memberExpression, Expression<Func<TReference, TElement>> idExpression, string filterUrl) where TFilter : ReferenceValueFilter<T, TElement>, new()
            {
                var filter = this.CreateFilter<TFilter, TProperty>(memberExpression);
                var idProperty = Reflect.GetProperty(idExpression);
                filter.IdPath = idProperty.Name;
                filter.Url = filterUrl;                
                return filter;
            }

            private TFilter CreateFilter<TFilter, TProperty>(Expression<Func<T, TProperty>> memberExpression) where TFilter : BaseFilter<T>, new()
            {
                var filter = new TFilter();
                filter.Name = this.GetPropertyName(memberExpression);
                filter.ForChildTable = parent != null;
                this.Filters.Add(filter);
                filter.SetTableBuilder(this);
                return filter;
            }

            private string GetPropertyName<TProperty>(Expression<Func<T, TProperty>> memberExpression)
            {
                return GetPropertyNameFrom<T, TProperty>(memberExpression);
            }

            private string GetPropertyNameFrom<TClass, TProperty>(Expression<Func<TClass, TProperty>> memberExpression)
            {
                return Reflect.GetProperty(memberExpression).Name;
            }
        }
    }
}