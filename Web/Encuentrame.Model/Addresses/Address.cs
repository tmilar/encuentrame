using Encuentrame.Support;

namespace Encuentrame.Model.Addresses
{
    public class Address: IDisplayable
    {
        public virtual string City { get; set; }
        public virtual string Province { get; set; }
        public virtual string Zip { get; set; }
        public virtual string Street { get; set; }
        public virtual string Number { get; set; }
        public virtual string FloorAndDepartament { get; set; }

        public string ToDisplay()
        {
            if (FloorAndDepartament.IsNullOrEmpty())
            {
                return $"{Street} {Number}, {City} ({Zip}), {Province}";
            }
            return $"{Street} {Number} {FloorAndDepartament}, {City} ({Zip}), {Province}";
        }
    }
   
}
