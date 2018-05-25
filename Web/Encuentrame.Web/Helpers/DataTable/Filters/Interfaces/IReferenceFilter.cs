using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Encuentrame.Web.Helpers.DataTable.Filters.Interfaces
{
    public interface IReferenceFilter : IMultipleValueFilter
    {
        string IdPath { get; set; }
        string Url { get; set; }
    }
}