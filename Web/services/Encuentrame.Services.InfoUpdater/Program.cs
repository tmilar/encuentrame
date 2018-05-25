using System;
using Topshelf;

namespace Encuentrame.Services.InfoUpdater
{
    public class Program
    {
        public static void Main(string[] args)
        {
            HostFactory.Run(x =>
            {
                x.Service<ServiceCreator>(s =>
                {
                    s.ConstructUsing(name => new ServiceCreator());
                    s.WhenStarted(sc => sc.Start());
                    s.WhenStopped(sc => sc.Stop());
                    
                });
                x.RunAsLocalSystem();
                
                x.SetDescription("Encuentrame Service");
                x.SetDisplayName("Encuentrame Service");
                x.SetServiceName("Encuentrame Service");
            });

            Console.WriteLine("Cualquier tecla para salir..."); 
            Console.ReadLine();
        }
    }
}
