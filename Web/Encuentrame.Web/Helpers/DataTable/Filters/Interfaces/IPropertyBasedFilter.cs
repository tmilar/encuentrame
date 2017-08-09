using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Encuentrame.Web.Helpers.DataTable.Filters.Interfaces
{
    public interface IPropertyBasedFilter : IBaseFilter
    {
        MvcHtmlString GetEditor(HtmlHelper helper);
    }
}