using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Encuentrame.Web.Helpers.DataTable.Filters.Interfaces
{
    public interface IMultipleValueFilter : IBaseFilter
    {
        bool CanAddDefaultValue { get; }
        bool IsMultiple { get; set; }
        bool ShowGroups { get; set; }
        string DefaultValue { get; }
    }
}