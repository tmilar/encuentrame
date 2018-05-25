using System.Collections.Generic;
using NailsFramework.Aspects;
using NailsFramework.Support;

namespace NailsFramework.IoC
{
    public abstract class IoCContainer : NailsComponent
    {
        public abstract IConfigurableObjectFactory GetObjectFactory();
        public abstract void ConfigureAspects(IEnumerable<Aspect> aspects);
    }
}