using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Encuentrame.Model.Accounts.Permissions;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers
{
    public static class HtmlEnumHelper
    {
        public static IHtmlString ToEnumString<TViewModel, TEnum>(this HtmlHelper htmlHelper, Expression<Func<TViewModel, TEnum>> memberExpression)
            where TEnum : IComparable 
            where TViewModel : class
        {
            var propertyName = Reflect.GetProperty(memberExpression).Name;
            var enumValue = Enum.GetValues(typeof(TEnum)).Cast<Enum>();
            var enumToStringBuilder = new StringBuilder();
            foreach (var value in enumValue)
            {
                enumToStringBuilder.AppendLine("<%");
                enumToStringBuilder.AppendLine(string.Format("if(row.{0} === {1}) {{ %>", propertyName, value.ToInt()));
                enumToStringBuilder.AppendLine(TranslationsHelper.Get(value));
                enumToStringBuilder.AppendLine("<% } %>");
            }
            
            return htmlHelper.Raw(enumToStringBuilder.ToString());
        }

        public static IHtmlString ToJsonTranslatedString<TEnum>(this HtmlHelper htmlHelper)
        {
            var enumValue = Enum.GetValues(typeof(TEnum)).Cast<Enum>();
            var entries = enumValue.Select(value =>
                $"\"{value.ToInt()}\": \"{ TranslationsHelper.Get(value)}\"");
            return htmlHelper.Raw("{" + string.Join(",", entries) + "}");
        }

      
    }
}