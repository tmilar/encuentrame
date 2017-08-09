using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Encuentrame.Web.Helpers.DataTable.Filters.Interfaces
{
    public interface IBooleanFilter : IBaseFilter
    {
        bool CanAddDefaultValue { get; }

        string DefaultValueToString { get; }
    }
}