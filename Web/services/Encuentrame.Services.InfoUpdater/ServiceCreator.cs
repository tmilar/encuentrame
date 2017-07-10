using System;
using System.Threading;
using NailsFramework;
using NailsFramework.Logging;
using NailsFramework.UserInterface;
using Quartz;
using Quartz.Impl;
using Encuentrame.Services.InfoUpdater.Jobs;
using Encuentrame.Support.Mappings;

namespace Encuentrame.Services.InfoUpdater
{
    public class ServiceCreator
    {
        private IScheduler Scheduler { get; set; }
        public ServiceCreator()
        {
            Initialize();

            Scheduler = StdSchedulerFactory.GetDefaultScheduler();
          
            IJobDetail customJob = JobBuilder.Create<CustomJob>()
                .WithIdentity("CustomJob", "EncuentrameGroup")
                .Build();

          


          
            ITrigger customJobTrigger = TriggerBuilder.Create()
                .WithIdentity("CustomJobTrigger", "EncuentrameGroup")
                .StartAt(DateTimeOffset.Now.AddSeconds(30))
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(60)
                    .RepeatForever())
                .Build();

           

            Scheduler.ScheduleJob(customJob, customJobTrigger);
          

        }

        protected void Initialize()
        {
            Nails.Configure()
                   .IoC.Container<NailsFramework.IoC.Spring>()
                   .Persistence.DataMapper<NailsFramework.Persistence.NHibernate>(
                       x => x.Configure(c => MappingConfigurator.Configure(c)))
                   .UserInterface.Platform<NullUIPlatform>()
                   .Logging.Logger<NullLogger>()
                   .InspectAssembly(@"Encuentrame.Model.dll")
                   .InspectAssembly(@"Encuentrame.Support.Mappings.dll")
                   .InspectAssembly(@"Encuentrame.Model.Mappings.dll")
                   .InspectAssembly(@"Encuentrame.Support.Email.dll")
                   .InspectAssembly(@"Encuentrame.InfoUpdaterService.exe")
                   .Initialize();

            Thread.Sleep(3000);
        }

        public void Start()
        {
            Scheduler.Start();
        }

        public void Stop()
        {
            Scheduler.Shutdown();
        }
    }
}