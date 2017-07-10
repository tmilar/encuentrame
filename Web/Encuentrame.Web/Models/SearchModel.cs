namespace Encuentrame.Web.Models
{
    public class SearchModel
    {
        public string Column { get; set; }
        public string Value { get; set; }
        public bool Regex { get; set; }
        public int Index { get; set; }
    }
}