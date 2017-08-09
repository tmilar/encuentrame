using System.Web.Mvc;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal abstract class BaseFilter<T> : FluentBuilder<T>, IBaseFilter where T : class
    {
        public int Index { get { return ColumnData.Index; } }
        public string ColumnName { get { return ColumnData.Name; } }

        public ColumnData ColumnData { get; set; }
        public string Name { get; set; }

        private int width = 2;
        public int Width { get { return width; } protected set { width = value; } }

        public bool ForChildTable { get; set; }
        public string CssClass { get; set; }

        public abstract string DisplayTemplate { get; }
        public string TranslationKey { get; set; }
    }
}