using System;

namespace NailsFramework.TestSupport
{
    public class DuplicatedNailsTestsConfigurationException : NailsException
    {
        public DuplicatedNailsTestsConfigurationException(string configurationName)
            : base(String.Format("Tests configuration identified as [{0}] is already defined", configurationName))
        {
        }
    }
}
