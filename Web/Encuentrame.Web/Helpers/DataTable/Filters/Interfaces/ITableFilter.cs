using System.Collections.Generic;

namespace Encuentrame.Web.Helpers.DataTable.Filters.Interfaces
{
    public interface ITableSingleValueFilter<T, in TFilterProperty> : ITableBuilder<T> where T : class
    {
        #region Methods

        ITableSingleValueFilter<T, TFilterProperty> AddDefaultValue(TFilterProperty value);
        ITableSingleValueFilter<T, TFilterProperty> AddClass(string cssClass);

        #endregion
    }

    public interface ITableMulipleValueFilter<T, in TFilterProperty, TProperty> : ITableBuilder<T>
        where T : class
        where TFilterProperty : IList<TProperty>
    {
        #region Methods

        ITableMulipleValueFilter<T, TFilterProperty, TProperty> AddDefaultValue(TFilterProperty values);
        ITableMulipleValueFilter<T, TFilterProperty, TProperty> AddDefaultValue(TProperty value);
        ITableMulipleValueFilter<T, TFilterProperty, TProperty> AllowMultiple();
        ITableMulipleValueFilter<T, TFilterProperty, TProperty> HasGroups();
        ITableMulipleValueFilter<T, TFilterProperty, TProperty> AddCssClass(string cssClass);

        #endregion
    }

    public interface ITableRangeValueFilter<T, TProperty> : ITableBuilder<T>
        where T : class
    {
        #region Methods

        ITableRangeValueFilter<T, TProperty> AddDefaultValue(TProperty min, TProperty max);
        ITableRangeValueFilter<T, TProperty> AddDefaultMin(TProperty min);
        ITableRangeValueFilter<T, TProperty> AddDefaultMax(TProperty max);
        ITableRangeValueFilter<T, TProperty> AddCssClass(string cssClass);

        #endregion
    }
}