namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    public interface IBaseFilter
    {
        string Name { get; set; }
        int Width { get; }
        ColumnData ColumnData { get; set; }
        int Index { get; }
        string ColumnName { get; }
        string CssClass { get; set; }
        bool ForChildTable { get; set; }

        string DisplayTemplate { get; }
        string TranslationKey { get; set; }
    }
}