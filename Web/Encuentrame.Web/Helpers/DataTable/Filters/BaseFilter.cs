using System.Web.Mvc;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal interface IBaseFilter
    {
        string Name { get; set; }
        int Width { get;  }
        void Build(ColumnData columnData, HtmlHelper helper);
    }

    internal abstract class BaseFilter<T> : FluentBuilder<T>, IBaseFilter where T : class
    {
        public string Name { get; set; }

        private int width=2;
        public int Width { get { return width; } protected set { width = value; } }

        public bool ForChildTable { get; set; }
        public string CssClass { get; set; }

        public abstract void Build(ColumnData columnData, HtmlHelper helper);

        protected void Build(string filter, string filterClass, HtmlHelper helper)
        {
            var text = TranslationsHelper.Get(this.Name) ?? this.Name;
            helper.ViewContext.Writer.Write("<div class='col-md-{0} table-filter-container {1} {2}' data-for-child-table=\'{3}\'>".FormatWith(Width, filterClass, CssClass, this.ForChildTable.ToString().ToLower()));            
            helper.ViewContext.Writer.Write("<div class='form-group'>");
            helper.ViewContext.Writer.Write("<label>{0}: </label>".FormatWith(text));
            helper.ViewContext.Writer.Write(filter);
            helper.ViewContext.Writer.Write("</div>");
            helper.ViewContext.Writer.Write("</div>");
        }
    }
}