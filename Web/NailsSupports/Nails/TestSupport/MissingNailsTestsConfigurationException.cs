using System;

namespace NailsFramework.TestSupport
{
    public class MissingNailsTestsConfigurationException : NailsException
    {
        public MissingNailsTestsConfigurationException(string configurationName)
            : base(String.Format("Unable to find the test configuration identified as [{0}]", configurationName))
        {
        }
    }
}
