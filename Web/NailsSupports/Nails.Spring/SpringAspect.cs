using System;
using System.Reflection;
using AopAlliance.Aop;
using AopAlliance.Intercept;
using NailsFramework.Aspects;
using Spring.Aop;
using Spring.Aop.Support;

namespace NailsFramework.IoC
{
    [Serializable]
    public class SpringAspect : StaticMethodMatcherPointcut, IPointcutAdvisor, IMethodInterceptor
    {
        public Aspect Aspect { get; set; }

        #region IMethodInterceptor Members

        public object Invoke(IMethodInvocation invocation)
        {
            return
                Aspect.Behavior.ApplyTo(new MethodInvocationInfo(invocation.Proceed, invocation.Method,
                                                                 invocation.Arguments));
        }

        #endregion

        #region IPointcutAdvisor Members

        public bool IsPerInstance
        {
            get { return true; }
        }

        public IAdvice Advice
        {
            get { return this; }
        }

        public IPointcut Pointcut
        {
            get { return this; }
        }

        #endregion

        public override bool Matches(MethodInfo method, Type targetType)
        {
            return Aspect.AppliesTo(method);
        }
    }
}