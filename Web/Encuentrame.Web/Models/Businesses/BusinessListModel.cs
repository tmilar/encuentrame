using System;

namespace Encuentrame.Web.Models.Businesses
{
    public class BusinessListModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Cuit { get; set; }
        public DateTime Created { get; set; }
        public string Address { get; set; }


    }
}