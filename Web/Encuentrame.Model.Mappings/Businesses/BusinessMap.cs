using Encuentrame.Model.Businesses;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Businesses
{
    public class BusinessMap:MappingOf<Business>
    {
        public BusinessMap()
        {
            Map(x => x.Name).Not.Nullable().UniqueKey("nameUnique");
            Map(x => x.DeletedKey).UniqueKey("nameUnique");
            Map(x => x.Created);
            Map(x => x.Cuit).Nullable();
           
            Component(x => x.Address, m =>
            {
                m.Map(x => x.Number);
                m.Map(x => x.Street);
                m.Map(x => x.City);
                m.Map(x => x.Province);
                m.Map(x => x.FloorAndDepartament);
                m.Map(x => x.Zip);
            }).ColumnPrefix("address");
        }
    }
}
