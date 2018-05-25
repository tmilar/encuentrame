using System.Collections.Generic;

namespace NailsFramework.IoC
{
    public interface IConfigurableObjectFactory : IObjectFactory
    {
        void Configure(IEnumerable<Lemming> lemmings, IEnumerable<Injection> staticInjections);
    }
}