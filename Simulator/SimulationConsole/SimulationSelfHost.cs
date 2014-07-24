using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.Text;
using System.Threading.Tasks;
using SimulationServiceLibrary;
namespace SimulationConsole
{
    class SimulationSelfHost
    {

        static void Main(string[] args)
        {
            try
            {
                using (WebServiceHost host = new WebServiceHost(typeof(SimulationService)))
                {
                    //ServiceEndpoint ep = host.AddServiceEndpoint(typeof(ISimulationService), new WSHttpBinding(),"");
                    ServiceDebugBehavior serviceBehavior = host.Description.Behaviors.Find<ServiceDebugBehavior>();
                    serviceBehavior.HttpHelpPageEnabled = false;

                    host.Open();

                    Console.WriteLine("********Simulation service is up and running********\n");
                    foreach (var ea in host.Description.Endpoints)
                    {
                        Console.WriteLine(ea.Address);
                    }
                    Console.WriteLine("\n\n********Press enter to quit service********* ");
                    Console.ReadLine();
                    host.Close();
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Something went wrong...");
            }
          
        }
    }
}
