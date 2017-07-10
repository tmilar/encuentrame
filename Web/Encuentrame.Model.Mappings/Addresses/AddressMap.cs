using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encuentrame.Model.Addresses;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Addresses
{
    public abstract class AddressMap<T> : MappingOf<T> where T: Address
    {
        public AddressMap()
        {
            Map(x => x.City);
            Map(x => x.Province);
            Map(x => x.Street);
            Map(x => x.Number);
            Map(x => x.Zip);
            Map(x => x.FloorAndDepartament);
            Map(x => x.IsDefault);
            //UseUnionSubclassForInheritanceMapping();
            //DiscriminateSubClassesOnColumn("AddressType");
        }
    }

    public class ClientAddressMap : AddressMap<ClientAddress>
    {        
        public ClientAddressMap()
        {
            Table("ClientAddresses");
        }
    }
}
