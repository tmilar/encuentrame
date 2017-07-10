using System.Collections.Generic;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    public interface ITableSingleValueFilter<T, in TFilterProperty> : ITableBuilder<T> where T : class
    {
        ITableSingleValueFilter<T, TFilterProperty> AddDefaultValue(TFilterProperty value);
        ITableSingleValueFilter<T, TFilterProperty> AddClass(string cssClass);
    }

    public interface ITableMulipleValueFilter<T, in TFilterProperty, TProperty> : ITableBuilder<T>
        where T : class
        where TFilterProperty: IList<TProperty>
    {
        ITableMulipleValueFilter<T, TFilterProperty, TProperty> AddDefaultValue(TFilterProperty values);
        ITableMulipleValueFilter<T, TFilterProperty, TProperty> AddDefaultValue(TProperty value);
        ITableMulipleValueFilter<T, TFilterProperty, TProperty> AllowMultiple();
        ITableMulipleValueFilter<T, TFilterProperty, TProperty> HasGroups();
        ITableMulipleValueFilter<T, TFilterProperty, TProperty> AddCssClass(string cssClass);
    }

    public interface ITablRangeValueFilter<T, TProperty> : ITableBuilder<T>
        where T : class        
    {
        ITablRangeValueFilter<T, TProperty> AddDefaultValue(TProperty min, TProperty max);
        ITablRangeValueFilter<T, TProperty> AddDefaultMin(TProperty min);
        ITablRangeValueFilter<T, TProperty> AddDefaultMax(TProperty max);
        ITablRangeValueFilter<T, TProperty> AddCssClass(string cssClass);
    }
}