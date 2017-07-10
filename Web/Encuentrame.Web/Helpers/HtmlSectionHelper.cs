using System;
using System.Text;
using System.Web.Mvc;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers
{
    public static class HtmlSectionHelper
    {
        public static IDisposable BeginEditSection(this HtmlHelper helper, string title)
        {
            BuildSection(helper,title, "panel-default");
            return new HtmlSection(helper);
        }
        
        public static IDisposable BeginEditSection(this HtmlHelper helper)
        {
            BuildSection(helper, String.Empty, "panel-default");
            return new HtmlSection(helper);
        }

        public static IDisposable BeginDisplaySection(this HtmlHelper helper, string title)
        {
            BuildSection(helper, title, "panel-info");
            return new HtmlSection(helper);
        }

        public static IDisposable BeginDisplaySection(this HtmlHelper helper, string title, string rightSection)
        {
            BuildSection(helper, title, "panel-info", rightSection);
            return new HtmlSection(helper);
        }

        public static IDisposable BeginDisplaySection(this HtmlHelper helper)
        {
            BuildSection(helper, String.Empty, "panel-info");
            return new HtmlSection(helper);
        }
        private static void BuildSection(HtmlHelper helper, string title, string classAdded, string rightSection = null)
        {
            helper.ViewContext.Writer.Write("<div class='panel {0}'>".FormatWith(classAdded));
            if (rightSection.NotIsNullOrEmpty() && title.NotIsNullOrEmpty())
            {
                var stringBuilder = new StringBuilder();
                stringBuilder.Append("<div class='panel-heading'>");
                stringBuilder.Append("<div class='row'>");                

                stringBuilder.Append("<div class='panel-left col-xs-4'>{0}</div>".FormatWith(title));
                stringBuilder.Append("<div class='panel-right col-xs-4 pull-right text-right'>");
                stringBuilder.Append(rightSection);
                stringBuilder.Append("</div>");                
                stringBuilder.Append("</div>");                
                stringBuilder.Append("</div>");
                helper.ViewContext.Writer.Write(stringBuilder.ToString());
            }
            else if (title.NotIsNullOrEmpty())
            {
                helper.ViewContext.Writer.Write("<div class='panel-heading'>{0}</div>".FormatWith(title));
            }

            helper.ViewContext.Writer.Write("<div class='panel-body'>");
        }

        class HtmlSection : IDisposable
        {
            private readonly HtmlHelper _helper;

            public HtmlSection(HtmlHelper helper)
            {
                this._helper = helper;
            }

            public void Dispose()
            {
                this._helper.ViewContext.Writer.Write("</div>");
                this._helper.ViewContext.Writer.Write("</div>");
            }
        }
    }
}