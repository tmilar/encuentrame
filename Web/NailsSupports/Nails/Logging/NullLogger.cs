using System;
using NailsFramework.Config;

namespace NailsFramework.Logging
{
    public class NullLogger : Logger
    {
        public override Type LogType
        {
            get { return typeof (NullLog); }
        }

        public override void AddCustomConfiguration(INailsConfigurator configurator)
        {
        }
    }
}