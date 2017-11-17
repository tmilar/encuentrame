namespace Encuentrame.Web.Models.Homes
{
    public class PieModel
    {
        public int Value { get; set; }
        public string Color { get; set; }

        public string Highlight { get; set; }
        public string Label  { get; set; }
        
    }

    public class LineModel
    {
        public string[]  Labels { get; set; }
        public int[] Data1 { get; set; }
        public int[] Data2 { get; set; }
    }
}