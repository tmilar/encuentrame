using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Mvc;
using Encuentrame.Web.Helpers.DataTable.Filters;
using Encuentrame.Web.Helpers.DataTable.Filters.Interfaces;
using Encuentrame.Web.Helpers.DataTable.ViewModels;
using Encuentrame.Web.Models.References.Commons;

namespace Encuentrame.Web.Helpers.DataTable
{
    internal class FluentBuilder<T> : ITableBuilder<T> where T : class
    {
        private ITableBuilder<T> tableBuilder;

        public void SetTableBuilder(ITableBuilder<T> tableBuilder)
        {
            this.tableBuilder = tableBuilder;
        }

        public DataTableViewModel BuildViewModel()
        {
            throw new NotImplementedException();
        }

        public ITableBuilder<T> AddColumn <TProperty>(Expression<Func<T, TProperty>> memberExpression,
            bool visible = true,
            bool sortable = true,
            string dataTemplate = "",
            bool totalize = false)
        {
            return this.tableBuilder.AddColumn(memberExpression, visible, sortable, dataTemplate, totalize);
        }

        public ITableBuilder<T> AddColumn <TProperty>(Expression<Func<T, TProperty>> memberExpression,
            Expression<Func<TProperty, string>> nameExpression,
            bool visible = true,
            bool sortable = true,
            string dataTemplate = "",
            bool totalize = false) where TProperty : class
        {
            return this.tableBuilder.AddColumn(memberExpression, nameExpression, visible, sortable, dataTemplate, totalize);
        }

        public ITableBuilder<T> AddColumn <TReference>(Expression<Func<T, IList<TReference>>> memberExpression,
            Expression<Func<TReference, string>> nameExpression,
            bool visible = true,
            bool sortable = true,
            string dataTemplate = "",
            bool totalize = false) where TReference : class
        {
            return this.tableBuilder.AddColumn(memberExpression, nameExpression, visible, sortable, dataTemplate, totalize);
        }

        public ITableBuilder<T> AddColumn(string key,
            bool visible = true,
            bool sortable = true,
            string dataTemplate = "",
            bool totalize = false)
        {
            return this.tableBuilder.AddColumn(key, visible, sortable, dataTemplate, totalize);
        }

        public ITableSingleValueFilter<T, string> AddFilter(Expression<Func<T, string>> memberExpression)
        {
            return this.tableBuilder.AddFilter(memberExpression);
        }

        public ITableSingleValueFilter<T, int> AddExactMatchFilter(Expression<Func<T, int>> memberExpression)
        {
            return this.tableBuilder.AddExactMatchFilter(memberExpression);
        }

        public ITableBuilder<T> AddFilter(Expression<Func<T, ReferenceAny>> memberExpression)
        {
            return this.tableBuilder.AddFilter(memberExpression);
        }

        public ITableSingleValueFilter<T, bool> AddFilter(Expression<Func<T, bool>> memberExpression)
        {
            return this.tableBuilder.AddFilter(memberExpression);
        }

        public ITableRangeValueFilter<T, int> AddFilter(Expression<Func<T, int>> memberExpression)
        {
            return this.tableBuilder.AddFilter(memberExpression);
        }

        public ITableRangeValueFilter<T, decimal> AddFilter(Expression<Func<T, decimal>> memberExpression)
        {
            return this.tableBuilder.AddFilter(memberExpression);
        }

        public ITableRangeValueFilter<T, DateTime> AddFilter(Expression<Func<T, DateTime>> memberExpression)
        {
            return this.tableBuilder.AddFilter(memberExpression);
        }

        public ITableMulipleValueFilter<T, List<int>, int> AddFilter<TProperty>(Expression<Func<T, TProperty>> memberExpression, Expression<Func<TProperty, int>> idExpression, string filterUrl) where TProperty : class
        {
            return this.tableBuilder.AddFilter(memberExpression, idExpression, filterUrl);
        }

        public ITableMulipleValueFilter<T, List<string>, string> AddFilter<TProperty>(Expression<Func<T, TProperty>> memberExpression, Expression<Func<TProperty, string>> idExpression, string filterUrl) where TProperty : class
        {
            return this.tableBuilder.AddFilter(memberExpression, idExpression, filterUrl);
        }

        public ITableMulipleValueFilter<T, List<int>, int> AddFilter<TReference>(Expression<Func<T, IList<TReference>>> memberExpression, Expression<Func<TReference, int>> idExpression, string filterUrl) where TReference : class
        {
            return this.tableBuilder.AddFilter(memberExpression, idExpression, filterUrl);
        }

        public ITableBuilder<T> InServer()
        {
            return this.tableBuilder.InServer();
        }

        public ITableBuilder<T> ShowSearchButton()
        {
            return this.tableBuilder.ShowSearchButton();
        }

        public ITableBuilder<T> DisableSearch()
        {
            return this.tableBuilder.DisableSearch();
        }

        public ITableBuilder<T> PageLengths(int[] lengths, int? defaultLenght = null)
        {
            return this.tableBuilder.PageLengths(lengths, defaultLenght);
        }

        public ITableBuilder<T> HideShowAllOption()
        {
            return this.tableBuilder.HideShowAllOption();
        }

        public ITableMulipleValueFilter<T, List<TEnum>, TEnum> AddFilter<TEnum>(Expression<Func<T, TEnum>> memberExpression) where TEnum : struct
        {
            return this.tableBuilder.AddFilter(memberExpression);
        }

        public ITableBuilder<TChild> ChildTable <TChild>(Expression<Func<T, IEnumerable<TChild>>> memberExpression) where TChild : class
        {
            return this.tableBuilder.ChildTable(memberExpression);
        }

        public ITableBuilder<TChild> ChildTable<TChild>(Expression<Func<T, int>> parentPropertyExpression, string url) where TChild : class
        {
            return this.tableBuilder.ChildTable<TChild>(parentPropertyExpression, url);
        }

        public ITableBuilder<T> SortBy <TProperty>(Expression<Func<T, TProperty>> memberExpression, DefaultOrder defaultOrder = DefaultOrder.Ascending)
        {
            return this.tableBuilder.SortBy(memberExpression);
        }

        public ITableBuilder<T> AllowSelection()
        {
            return this.tableBuilder.AllowSelection();
        }

        MvcHtmlString ITableBuilder.Build()
        {
            return this.tableBuilder.Build();
        }
    }
}