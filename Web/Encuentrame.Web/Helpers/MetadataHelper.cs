using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Newtonsoft.Json;
using Encuentrame.Web.MetadataProviders;
using Encuentrame.Web.MetadataProviders.ConditionalValidations;
using Encuentrame.Web.MetadataProviders.CustomValidations;
using Encuentrame.Support;

namespace Encuentrame.Web.Helpers
{
    public static class MetadataHelper
    {
        public static MvcHtmlString ErrorSummary<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
            var messages=new List<string>();
            var allErrors = htmlHelper.ViewDataContainer.ViewData.ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in allErrors)
            {
                messages.Add(error.ErrorMessage);
            }
            
            if (htmlHelper.ViewContext.TempData.ContainsKey("ErrorMessage"))
            {
                var message = htmlHelper.ViewContext.TempData["ErrorMessage"] as string;
                messages.Add(message);
            }

            if (messages.Any())
            {
                var html = new StringBuilder();
                html.Append("<div class=\"alert alert-danger alert-dismissible\" role=\"alert\">");
                foreach (var message in messages)
                {
                    html.AppendFormat("<span>{0}</span>", message);

                }

                html.Append(
                    "<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">×</span></button>");
                html.Append("</div>");
                return new MvcHtmlString(html.ToString());
            }
            return new MvcHtmlString("");
        }

        public static MvcHtmlString SuccessSummary<TModel>(this HtmlHelper<TModel> htmlHelper,
            string message)
        {
            if (message.IsNullOrEmpty())
            {
                return SuccessSummary(htmlHelper,new string[0]);    
            }
            return SuccessSummary(htmlHelper, new[] {message});
        }
        public static MvcHtmlString SuccessSummary<TModel>(this HtmlHelper<TModel> htmlHelper)
        {
                return SuccessSummary(htmlHelper, new string[0]);
        }
        public static MvcHtmlString SuccessSummary<TModel>(this HtmlHelper<TModel> htmlHelper, IEnumerable<string> messages)
        {
            var list=new List<string>(messages);
            if (htmlHelper.ViewContext.TempData.ContainsKey("SuccessMessage"))
            {
                var message=htmlHelper.ViewContext.TempData["SuccessMessage"] as string;
                list.Add(message);
            }

            if (list.Any())
            {
                var html = new StringBuilder();
                html.Append("<div class=\"alert alert-success alert-dismissible alert-auto-dismissible\" role=\"alert\">");
                
              
                foreach (var err in list)
                {
                    html.AppendFormat("<span>{0}</span>", err);

                }
                html.Append("<button type=\"button\" class=\"close\" data-dismiss=\"alert\" aria-label=\"Close\"><span aria-hidden=\"true\">×</span></button>");
                html.Append("</div>");
                return new MvcHtmlString(html.ToString());
            }
            return new MvcHtmlString("");
        }


        public static int GetStringLengthValue<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {

            var metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>(htmlHelper.ViewDataContainer.ViewData));
            var stringLength = metadata.ContainerType.GetProperty(metadata.PropertyName)
                .GetCustomAttributes(typeof(StringLengthAttribute), false)
                .FirstOrDefault() as StringLengthAttribute;


            if (stringLength != null)
            {
                return stringLength.MaximumLength;
            }

            return 0;
        }

        public static string GetRequiredMessage<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>(htmlHelper.ViewDataContainer.ViewData));

            var required = metadata.ContainerType.GetProperty(metadata.PropertyName)
                .GetCustomAttributes(typeof(RequiredAttribute), false)
                .FirstOrDefault() as RequiredAttribute;

            if (required != null)
            {
                return required.FormatErrorMessage(metadata.DisplayName);
            }          
            return "";
        }

        public static bool GetIfConditionalRequired<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>(htmlHelper.ViewDataContainer.ViewData));          
            var requiredIf = metadata.ContainerType.GetProperty(metadata.PropertyName)
               .GetCustomAttributes(typeof(RequiredIfAttribute), false)
               .FirstOrDefault() as RequiredIfAttribute;

            if (requiredIf != null)
            {
                return true;
            }

            var rangeIf = metadata.ContainerType.GetProperty(metadata.PropertyName)
               .GetCustomAttributes(typeof(RangeIfAttribute), false)
               .FirstOrDefault() as RangeIfAttribute;

            if (rangeIf != null)
            {
                return true;
            }

            var regularExpressionIf = metadata.ContainerType.GetProperty(metadata.PropertyName)
               .GetCustomAttributes(typeof(RegularExpressionIfAttribute), false)
               .FirstOrDefault() as RegularExpressionIfAttribute;

            if (regularExpressionIf != null)
            {
                return true;
            }

            return false;
        }

        public static bool GetIfRequired<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            var metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>(htmlHelper.ViewDataContainer.ViewData));
            var requiredAttribute = RequiredAttribute<RequiredAttribute>(metadata);

            if (requiredAttribute != null)
            {
                return true;
            }

            var requiredReference2Attribute = RequiredAttribute<RequiredReference2Attribute>(metadata);

            if (requiredReference2Attribute != null)
            {
                return true;
            }

            var requiredListAttribute = RequiredAttribute<RequiredListAttribute>(metadata);

            if (requiredListAttribute != null)
            {
                return true;
            }

            var requiredReferenceAny2Attribute = RequiredAttribute<RequiredReferenceAny2Attribute>(metadata);

            if (requiredReferenceAny2Attribute != null)
            {
                return true;
            }           

            var requiredIf = metadata.ContainerType.GetProperty(metadata.PropertyName)
               .GetCustomAttributes(typeof(RequiredIfAttribute), false)
               .FirstOrDefault() as RequiredIfAttribute;

            if (requiredIf != null)
            {
                return true;
            }

            var rangeIf = metadata.ContainerType.GetProperty(metadata.PropertyName)
               .GetCustomAttributes(typeof(RangeIfAttribute), false)
               .FirstOrDefault() as RangeIfAttribute;

            if (rangeIf != null)
            {
                return true;
            }

            var regularExpressionIf = metadata.ContainerType.GetProperty(metadata.PropertyName)
               .GetCustomAttributes(typeof(RegularExpressionIfAttribute), false)
               .FirstOrDefault() as RegularExpressionIfAttribute;

            if (regularExpressionIf != null)
            {
                return true;
            }

            return false;
        }

        private static TAttribute RequiredAttribute<TAttribute>(ModelMetadata metadata) where TAttribute : Attribute
        {
            var requiredAttribute = metadata.ContainerType.GetProperty(metadata.PropertyName)
                .GetCustomAttributes(typeof(TAttribute), true)
                .FirstOrDefault() as TAttribute;
            return requiredAttribute;
        }

        public static MvcHtmlString RequiredMark<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {

            var metadata = ModelMetadata.FromLambdaExpression(expression, new ViewDataDictionary<TModel>(htmlHelper.ViewDataContainer.ViewData));
            if (metadata.IsRequired)
            {
                return new MvcHtmlString("<p class='text-danger pull-left' aria-hidden='true'>*</p>");                
            }
            var isConditionallyRequired = GetIfConditionalRequired(htmlHelper, expression);

            if (isConditionallyRequired)
            {
                return new MvcHtmlString("<p class='text-danger pull-left required-mark' aria-hidden='true'>*</p>");
            }

            return new MvcHtmlString("");
        }
    }
}