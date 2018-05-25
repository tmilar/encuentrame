using System.Collections.Generic;
using NailsFramework.Aspects;
using NailsFramework.Config;
using Spring.Aop.Framework.AutoProxy;
using Spring.Aop.Framework.DynamicProxy;

namespace NailsFramework.IoC
{
    public class Spring : IoCContainer
    {
        public override void AddCustomConfiguration(INailsConfigurator configurator)
        {
            configurator.IoC
                .Lemming<SpringVirtualMethodsProxyFactory>()
                .Lemming<DefaultAdvisorAutoProxyCreator>(
                    m => m.Reference<DefaultAopProxyFactory>(x => x.AopProxyFactory));
        }

        public override IConfigurableObjectFactory GetObjectFactory()
        {
            return new SpringObjectFactory();
        }

        public override void ConfigureAspects(IEnumerable<Aspect> aspects)
        {
            var i = 0;
            foreach (var aspect in aspects)
            {
                i++;
                Nails.Configure()
                    .IoC.Lemming<SpringAspect>(x => x.Name("__SpringAspect" + i)
                                                        .Value(m => m.Aspect, aspect));
            }
        }
    }
}