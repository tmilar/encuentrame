using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Encuentrame.Web.Helpers.DataTable.Filters.Interfaces
{
    public interface IEnumFilter : IMultipleValueFilter
    {
        Type EnumType { get; }
    }
}