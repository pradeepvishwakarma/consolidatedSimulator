using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using SimulationBussinessLayer;

namespace SimulationServiceLibrary
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
     [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class SimulationService : ISimulationService
    {
         public string GetDataForICA(string deviceID, string reset)
        {
            try
            {
                SimulationBL simulationBL = new SimulationBL();
                return simulationBL.GenerateICAData(deviceID, reset);

            }
            catch (Exception)
            {
                return string.Empty;
            }

        }
        
         public string GetDataForSeedStar(string deviceID, string reset)
         {

            try
            {
                SimulationBL simulationBL = new SimulationBL();
                return simulationBL.GenerateSeedStarData(deviceID, reset);

            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

    }
}
