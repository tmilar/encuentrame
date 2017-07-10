using System.Collections.Generic;

namespace Encuentrame.Web.Models
{
    public class DataTableModel
    {
        public int Draw { get; set; }
        public int Start { get; set; }
        public int? Length { get; set; }
        public bool IsServerSide { get; set; }
        public string SortColumn { get; set; }
        public string SortOrder { get; set; }

        private List<SearchModel> mySearchData;

        public List<SearchModel> SearchData
        {
            get { return this.mySearchData ?? (this.mySearchData = new List<SearchModel>()); }
            set { this.mySearchData = value; }
        }
    }

    public class ChildDataTableModel : DataTableModel
    {
        public int ParentId { get; set; }
    }
}