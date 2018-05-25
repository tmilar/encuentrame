using System.Collections.Generic;
using NailsFramework.Aspects;

namespace NailsFramework.Tests.IoC.Aspects
{
    public class NullBehavior : ILemmingBehavior
    {
        #region ILemmingBehavior Members

        public object ApplyTo(MethodInvocationInfo invocation)
        {
            return null;
        }

        #endregion

        public void ExcludeMethods(IEnumerable<string> methods)
        {
        }
    }
}