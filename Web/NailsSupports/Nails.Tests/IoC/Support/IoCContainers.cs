using System;
using System.Collections.Generic;
using NailsFramework.Aspects;
using NailsFramework.IoC;

namespace NailsFramework.Tests.IoC.Support
{
    public static class IoCContainers
    {
        private static readonly IDictionary<string, Func<IoCContainer>> Containers;
        private static readonly IDictionary<string, Func<IConfigurableObjectFactory>> ObjectFactories;

        static IoCContainers()
        {
            Containers = new Dictionary<string, Func<IoCContainer>>
                             {
                                 {"Spring", () => new NailsFramework.IoC.Spring()},
                                 {"Unity", () => new Unity()}
                             };
            ObjectFactories = new Dictionary<string, Func<IConfigurableObjectFactory>>
                                  {
                                      {"Spring", () => new SpringObjectFactory()},
                                      {
                                          "Unity", () =>
                                                       {
                                                           var unity = new Unity();
                                                           unity.ConfigureAspects(new List<Aspect>());
                                                           return unity.GetObjectFactory();
                                                       }
                                          }
                                  };
        }

        public static IoCContainer GetContainer(string name)
        {
            return Containers[name]();
        }

        public static IConfigurableObjectFactory GetObjectFactory(string name)
        {
            return ObjectFactories[name]();
        }
    }
}