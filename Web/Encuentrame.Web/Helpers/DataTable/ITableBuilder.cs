using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Encuentrame.Web.Helpers.DataTable.Filters;
using Encuentrame.Web.Models.References.Commons;

namespace Encuentrame.Web.Helpers.DataTable
{
    public interface ITableBuilder : IDisposable
    {
        string Build();
    }
    public interface ITableBuilder<T> : ITableBuilder where T : class
    {
        ITableBuilder<T> AddColumn<TProperty>(Expression<Func<T, TProperty>> memberExpression,
            bool visible = true,
            bool sortable = true,
            string dataTemplate = "",
            bool totalize = false);

        ITableBuilder<T> AddColumn<TProperty>(Expression<Func<T, TProperty>> memberExpression,
            Expression<Func<TProperty, string>> nameExpression,
            bool visible = true,
            bool sortable = true,
            string dataTemplate = "",
            bool totalize = false) where TProperty : class;

        ITableBuilder<T> AddColumn<TReference>(Expression<Func<T, IList<TReference>>> memberExpression,
            Expression<Func<TReference, string>> nameExpression,
            bool visible = true,
            bool sortable = true,
            string dataTemplate = "",
            bool totalize = false) where TReference : class;


        ITableBuilder<T> AddColumn(string key, bool visible = true, bool sortable = true, string dataTemplate = "", bool totalize = false);

        ITableSingleValueFilter<T, string> AddFilter(Expression<Func<T, string>> memberExpression);
        ITableBuilder<T> AddFilter(Expression<Func<T, ReferenceAny>> memberExpression);
        ITableSingleValueFilter<T, bool> AddFilter(Expression<Func<T, bool>> memberExpression);
        ITablRangeValueFilter<T, int> AddFilter(Expression<Func<T, int>> memberExpression);
        ITablRangeValueFilter<T, decimal> AddFilter(Expression<Func<T, decimal>> memberExpression);
        ITablRangeValueFilter<T, DateTime> AddFilter(Expression<Func<T, DateTime>> memberExpression);
        ITableMulipleValueFilter<T, List<int>, int> AddFilter<TProperty>(Expression<Func<T, TProperty>> memberExpression, Expression<Func<TProperty, int>> idExpression, string filterUrl) where TProperty : class;
        ITableMulipleValueFilter<T, List<string>, string> AddFilter<TProperty>(Expression<Func<T, TProperty>> memberExpression, Expression<Func<TProperty, string>> idExpression, string filterUrl) where TProperty : class;
        ITableMulipleValueFilter<T, List<int>, int> AddFilter<TReference>(Expression<Func<T, IList<TReference>>> memberExpression, Expression<Func<TReference, int>> idExpression, string filterUrl) where TReference : class;
        ITableBuilder<T> InServer();
        ITableBuilder<T> ShowSearchButton();
        ITableBuilder<T> DisableSearch();
        ITableBuilder<T> PageLengths(int[] lengths, int? defaultLenght = null);
        ITableBuilder<T> HideShowAllOption();
        ITableMulipleValueFilter<T, List<TEnum>, TEnum> AddFilter<TEnum>(Expression<Func<T, TEnum>> memberExpression) where TEnum : struct;
        ITableBuilder<TChild> ChildTable<TChild>(Expression<Func<T, IEnumerable<TChild>>> memberExpression) where TChild: class;
        ITableBuilder<TChild> ChildTable<TChild>(Expression<Func<T, int>> parentPropertyExpression, string url) where TChild : class;
        ITableBuilder<T> SortBy<TProperty>(Expression<Func<T, TProperty>> memberExpression, DefaultOrder defaultOrder = DefaultOrder.Ascending);
    }

    public interface ITableFilterBuilder <T, TProperty> : ITableBuilder<T> where T : class
    {
        ITableBuilder<T> AddDefaultValue(Expression<Func<T, TProperty>> memberExpression);
    }
}