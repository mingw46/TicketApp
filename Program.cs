using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace TicketEmailSender
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var myService = new Service1();
            if (Environment.UserInteractive)
            {
                Console.WriteLine("Starting service...");
                myService.Start();
                Console.WriteLine("Service is running");
                Console.ReadKey(true);
                Console.WriteLine("Stopping service");
                myService.Stop();
                Console.WriteLine("Service stopped");

            }
            else
            {
                ServiceBase[] ServicesToRun;
                ServicesToRun = new ServiceBase[]
                {
                new Service1()
                };
                ServiceBase.Run(ServicesToRun);
            }
        }
    }
}
