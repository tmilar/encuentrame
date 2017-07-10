using System.Collections.Generic;
using NailsFramework.Aspects;
using NailsFramework.Config;

namespace NailsFramework.IoC
{
    public class NullIoCContainer : IoCContainer
    {
        public override IConfigurableObjectFactory GetObjectFactory()
        {
            return new NullObjectFactory();
        }

        public override void AddCustomConfiguration(INailsConfigurator configurator)
        {
        }

        public override void ConfigureAspects(IEnumerable<Aspect> aspects)
        {
        }
    }
}