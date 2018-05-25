using Encuentrame.Support;

namespace Encuentrame.Web.Helpers
{
    public static class HtmlTagsHelper
    {
        public static string SurroundWithParagraph(this string text)
        {

            return "<p>{0}</p>".FormatWith(text);
        }
    }
}