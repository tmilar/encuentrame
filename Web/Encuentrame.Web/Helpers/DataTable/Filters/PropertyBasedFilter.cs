using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using Encuentrame.Web.Helpers.DataTable.Filters.Interfaces;

namespace Encuentrame.Web.Helpers.DataTable.Filters
{
    internal class PropertyBasedFilter<T, TProperty> : BaseFilter<T>, IPropertyBasedFilter
        where T : class
        where TProperty: class
    {
        public Expression<Func<T, TProperty>> MemberExpression { get; set; }

        public MvcHtmlString GetEditor(HtmlHelper helper)
        {
            return helper.EditorForModel(MemberExpression);
        }

        public override string DisplayTemplate
        {
            get { return typeof(IPropertyBasedFilter).Name; }
        }
    }
}