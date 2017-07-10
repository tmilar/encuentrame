using NailsFramework.Tests.Persistence.Common;

namespace NailsFramework.Tests.Persistence.Memory.TestModel
{
    public class City : ICity
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        object ICity.Id
        {
            get { return Id; }
        }
    }
}