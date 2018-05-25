using System;
using System.Text;
using System.Web.Mvc;

namespace Encuentrame.Web.Helpers.DataTable
{
    public class ColumnData
    {
        #region Properties

        public bool Visible { get; set; }
        public bool Sortable { get; set; }
        public int Index { get; set; }
        public bool HasDataAssociated { get; set; }
        public string Name { get; set; }
        public string Template { get; set; }
        public Type Type { get; set; }
        public bool IsReference { get; set; }
        public string NamePath { get; set; }
        public bool Totalize { get; set; }
        public string TranslationKey { get; set; }
        #endregion
    }
}