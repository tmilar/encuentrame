using System;
using System.Collections;
using System.Collections.Generic;

namespace NailsFramework.Tests.IoC.Lemmings
{
    public class DependencyCollection : IEnumerable<IServiceDependency>
    {
        #region IEnumerable<IServiceDependency> Members

        public IEnumerator<IServiceDependency> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}