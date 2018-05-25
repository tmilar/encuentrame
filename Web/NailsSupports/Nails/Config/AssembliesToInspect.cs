using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NailsFramework.IoC;

namespace NailsFramework.Config
{
    [Lemming]
    public class AssembliesToInspect : IEnumerable<Assembly>
    {
        #region IEnumerable<Assembly> Members

        public IEnumerator<Assembly> GetEnumerator()
        {
            return Nails.Configuration.AssembliesToInspect.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}