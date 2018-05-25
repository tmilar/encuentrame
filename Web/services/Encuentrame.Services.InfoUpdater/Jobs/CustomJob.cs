using System;
using NailsFramework.IoC;
using NailsFramework.UnitOfWork;
using NailsFramework.UnitOfWork.ContextProviders;
using Quartz;
using Encuentrame.Support;

namespace Encuentrame.Services.InfoUpdater.Jobs
{
    [DisallowConcurrentExecution]
    public class CustomJob : IJob
    {
      

        [Inject]
        public static IWorkContextProvider ContextProvider { private get; set; }


        public void Execute(IJobExecutionContext context)
        {
            try
            {
                ContextProvider.CurrentContext.RunUnitOfWork(() =>
                {
                    Console.WriteLine("Job: Processing ");
                  //ACA VA LA COSA
                    
                }, new UnitOfWorkInfo(true));
                Console.WriteLine("Job: Processed ");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Job EXCEPTION: " + ex.Message);
                Console.ResetColor();
            }

            Console.WriteLine("Job: {0}".FormatWith(SystemDateTime.Now));
        }
    }
}