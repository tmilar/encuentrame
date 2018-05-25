using NailsFramework.IoC;

namespace NailsFramework.Persistence
{
    [Lemming]
    public class BagFactory
    {
        [Inject]
        public IObjectFactory ObjectFactory { private get; set; }

        public virtual IBag<T> GetBag<T>() where T : class
        {
            return ObjectFactory.GetObject<IBag<T>>();
        }
    }
}