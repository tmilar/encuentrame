using System;
using log4net.Config;
using NailsFramework.Config;

namespace NailsFramework.Logging
{
    public class Log4net : Logger
    {
        public override Type LogType
        {
            get { return typeof (Log); }
        }

        public override void AddCustomConfiguration(INailsConfigurator configurator)
        {
        }

        public override void Initialize()
        {
            XmlConfigurator.Configure();
        }
    }
}