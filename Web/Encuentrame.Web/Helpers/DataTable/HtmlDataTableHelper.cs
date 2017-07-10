using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Encuentrame.Web.Helpers.DataTable.Filters;
using Encuentrame.Web.Models.References.Commons;
using Encuentrame.Support;
using System.Linq;

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

            public HtmlDataTable(HtmlHelper helper, string url, string tableId)
            {
                this.tableId = tableId;
                this.url = url;
                this._helper = helper;
                this.searchEnabled = true;                
            }

            protected HtmlDataTable(HtmlHelper helper, string tableId, ITableBuilder parent, string url=null)
            {
                this.tableId = tableId;                
                this._helper = helper;
                this.searchEnabled = true;                
                this.parent = parent;
                this.url = url;
            }

            public void Dispose()
            {
                this.Build();
            }

            public string Build()
            {
                if (parent != null)
                    parent.Build();
                else
                    this.BuildFilters();
                
                var className = parent == null ? "data-table-editor" : string.Empty;
                this.BuildTable(className);

                return string.Empty;
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

                var clearFiltersLink = "<a href=\"#\" class='filter-clear'>{0}</a>".FormatWith(TranslationsHelper.Get("Clear"));
                var title = TranslationsHelper.Get("Filter") ?? "Filter";
                using (this._helper.BeginDisplaySection(title, rightSection: clearFiltersLink))
                {
                    var filterHeaderDivBuilder = new StringBuilder();
                    filterHeaderDivBuilder.Append("<div class='data-table-filters' role=\"form\" ");
                    filterHeaderDivBuilder.Append("data-table-id=\"{0}\"".FormatWith(this.tableId));
                    if (!string.IsNullOrEmpty(this.childPropertyName))
                    {
                        filterHeaderDivBuilder.Append("data-child-property='{0}'".FormatWith(childPropertyName));
                    }

                    filterHeaderDivBuilder.Append(">");

                    this._helper.ViewContext.Writer.Write(filterHeaderDivBuilder.ToString());

                    this._helper.ViewContext.Writer.Write("<div class='row'>");
                    int widthSum = 0;
                    foreach (var filter in this.Filters)
                    {
                        if ((widthSum + filter.Width) > 12)
                        {
                            this._helper.ViewContext.Writer.Write("</div>");   
                            this._helper.ViewContext.Writer.Write("<div class='row'>");
                            widthSum = 0;
                        }
                        var columnData = this.Columns[filter.Name];
                        filter.Build(columnData, this._helper);
                        widthSum += filter.Width;
                    }

                    var baseChildTable = childTable as BaseHtmlDataTable;
                    if (baseChildTable != null && baseChildTable.Filters!=null)
                    {
                        foreach (var filter in baseChildTable.Filters)
                        {
                            if ((widthSum + filter.Width) > 12)
                            {
                                this._helper.ViewContext.Writer.Write("</div>");
                                this._helper.ViewContext.Writer.Write("<div class='row'>");
                                widthSum = 0;
                            }
                            var columnData = baseChildTable.Columns[filter.Name];
                            filter.Build(columnData, this._helper);
                            widthSum += filter.Width;
                        }
                    }

                    this._helper.ViewContext.Writer.Write("</div>");
                    this._helper.ViewContext.Writer.Write("</div>");

                    if (deferredSearch)
                    {
                        var filterLnk = "<a href=\"#\" class='filter-table btn btn-primary filter-button-style'>{0}</a>".FormatWith(TranslationsHelper.Get("Filter"));
                        this._helper.ViewContext.Writer.Write(filterLnk);   
                    }                    
                }                
            }

            private void BuildTable(string className)
            {
                var childTableClass = parent != null ? "well" : string.Empty;
                var tableHeader = new StringBuilder();
                tableHeader.Append("<table class='table table-striped table-style {0} {1}' id=\"{2}\" data-server-side=\"{3}\" data-allow-search=\"{4}\""
                    .FormatWith(className, childTableClass, this.tableId, this.serverSide.ToString().ToLower(), this.searchEnabled.ToString().ToLower()));

                if (!string.IsNullOrEmpty(this.url))
                {
                    tableHeader.Append("data-url='{0}'".FormatWith(this.url));                    
                }

                if (!string.IsNullOrEmpty(sortColumn))
                {
                    var sortOrder = this.Columns[sortColumn].Index;
                    tableHeader.Append("data-sort-column='{0}'".FormatWith(sortOrder));
                    var defaultOrderString = defaultOrder == DefaultOrder.Ascending ? "asc" : "desc";
                    tableHeader.Append("data-default-order='{0}'".FormatWith(defaultOrderString));
                }

                if (!this.showAllOption)
                {
                    tableHeader.Append(" data-hide-show-all='{0}'".FormatWith(true.ToString().ToLower()));
                }

                if (lengths != null && lengths.Length > 0)
                {
                    var lenghtsToShow = lengths.Join(",");
                    tableHeader.Append(" data-lengths-to-show='{0}'".FormatWith(lenghtsToShow));
                    if (defaultLength.HasValue)
                    {
                        tableHeader.Append(" data-default-length='{0}'".FormatWith(defaultLength.Value));
                    }
                }

                if (!string.IsNullOrEmpty(this.parentPropertyName))
                {
                    tableHeader.Append("data-parent-property='{0}'".FormatWith(parentPropertyName));
                }

                tableHeader.Append(">");

                this._helper.ViewContext.Writer.Write(tableHeader.ToString());
                this._helper.ViewContext.Writer.Write("<thead>");
                this._helper.ViewContext.Writer.Write("<tr>");

                if (childTable!=null)
                {
                    this._helper.ViewContext.Writer.Write("<th></th>");
                }

                foreach (var columnData in this.Columns.Values)
                {
                    columnData.Build(this._helper);
                }                

                this._helper.ViewContext.Writer.Write("</tr>");
                this._helper.ViewContext.Writer.Write("</thead>");

                if (addFooter)
                    AddFooter();

                this._helper.ViewContext.Writer.Write("</table>");
            }

            private void AddFooter()
            {
                //tfoot
                this._helper.ViewContext.Writer.Write("<tfoot>");
                this._helper.ViewContext.Writer.Write("<tr>");

                if (childTable != null)
                {
                    this._helper.ViewContext.Writer.Write("<th></th>");
                }

                foreach (var columnData in this.Columns.Values)
                {
                    this._helper.ViewContext.Writer.Write("<th></th>");
                }

                this._helper.ViewContext.Writer.Write("</tr>");
                this._helper.ViewContext.Writer.Write("</tfoot>");
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
                return columnData;
            }

            public ITableSingleValueFilter<T, string> AddFilter(Expression<Func<T, string>> memberExpression)
            {
                return this.CreateFilter<StringValueFilter<T>, string>(memberExpression);                
            }

            public ITableBuilder<T> AddFilter(Expression<Func<T, ReferenceAny>> memberExpression)
            {
                var filter = new PropertyBasedFilter<T, ReferenceAny>();
                filter.MemberExpression = memberExpression;
                filter.Name = this.GetPropertyName(memberExpression);
                filter.ForChildTable = parent != null;
                this.Filters.Add(filter);
                filter.SetTableBuilder(this);

                return filter;
            }

            public ITableSingleValueFilter<T, bool> AddFilter(Expression<Func<T, bool>> memberExpression)
            {                
                return this.CreateFilter<BooleanValueFilter<T>, bool>(memberExpression);                
            }

            public ITablRangeValueFilter<T, int> AddFilter(Expression<Func<T, int>> memberExpression)
            {
                return this.CreateFilter<RangeFilter<T, int>, int>(memberExpression);
            }

            public ITablRangeValueFilter<T, decimal> AddFilter(Expression<Func<T, decimal>> memberExpression)
            {                
                return this.CreateFilter<RangeFilter<T, decimal>, decimal>(memberExpression);
            }

            public ITablRangeValueFilter<T, DateTime> AddFilter(Expression<Func<T, DateTime>> memberExpression)
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