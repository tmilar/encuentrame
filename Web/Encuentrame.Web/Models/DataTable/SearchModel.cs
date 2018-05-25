namespace Encuentrame.Web.Models.DataTable
{
    public class SearchModel
    {
        public string Column { get; set; }
        public string Value { get; set; }
        public bool Regex { get; set; }
        public int Index { get; set; }
        public string FilterType { get; set; }
    }
}