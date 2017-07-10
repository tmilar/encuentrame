using System;
using NailsFramework.Aspects;
using Spring.Aop;
using Spring.Aop.Framework;
using Spring.Aop.Framework.DynamicProxy;

namespace NailsFramework.IoC
{
    [Lemming]
    public class SpringVirtualMethodsProxyFactory : IVirtualMethodsProxyFactory
    {
        public T Create<T>(params Aspect[] with) where T : new()
        {
            var proxy = new ProxyFactory
                            {
                                ProxyTargetType = true,
                                TargetSource = new InheritanceBasedAopTargetSource(typeof(T))
                            };

            foreach (var aspect in with)
            {
                proxy.AddAdvisor(new SpringAspect
                                     {
                                         Aspect = aspect
                                     });
            }

            var proxyTypeBuilder = new InheritanceAopProxyTypeBuilder(proxy);
            var proxyType = proxyTypeBuilder.BuildProxyType();

            return (T) proxyType.GetConstructors()[0].Invoke( new object[]{ proxy});
        }

        private class InheritanceBasedAopTargetSource : ITargetSource
        {
            public InheritanceBasedAopTargetSource(Type targetType)
            {
                TargetType = targetType;
            }

            public object GetTarget()
            {
                return null;
            }

            public void ReleaseTarget(object target)
            {
            }

            public bool IsStatic
            {
                get
                {
                    return true;
                }
            }

            public Type TargetType { get; private set; }
        }
    }
}