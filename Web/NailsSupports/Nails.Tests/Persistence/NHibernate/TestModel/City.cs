using NailsFramework.Tests.Persistence.Common;

namespace NailsFramework.Tests.Persistence.NHibernate.TestModel
{
    public class City : ICity
    {
        public virtual int Id { get; set; }
        object ICity.Id
        {
            get { return Id; }
        }
        #region ICity Members

        public virtual string Name { get; set; }

        #endregion
    }
}