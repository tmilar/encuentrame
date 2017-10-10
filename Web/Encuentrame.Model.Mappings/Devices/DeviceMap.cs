using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Encuentrame.Model.Devices;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Model.Mappings.Devices
{
    public class DeviceMap : MappingOf<Device>
    {
        public DeviceMap()
        {
            Map(x => x.Token).Not.Nullable();
            Map(x => x.Created).Not.Nullable();
            References(r => r.User).Not.Nullable();
        }
    }
}
