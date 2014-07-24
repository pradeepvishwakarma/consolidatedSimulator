using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using SimulationBussinessLayer;
using SimulationService.Entities;
using SimulationService.SimulationBussinessLayer.Enums;


namespace SimulationService
{
   [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
   public class SimulationService : ISimulationService
    {
       //static Stream lastStream;
       //static string id;
       //static string device="TESTACK";

       public string GetDataForICA(string deviceID, string reset)
        {
           try
           {
               SimulationBL simulationBL = new SimulationBL();
               return  simulationBL.GenerateICAData(deviceID, reset);               
           }
           catch (Exception)
           {
               return string.Empty ;
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


       public Stream EstablishSession(string deviceID)
       {
           try
           {
               return null;
               //SimulationBL simulationBL = new SimulationBL();
               //id=Guid.NewGuid().ToString().Replace("-", "").ToUpper();              
               // return simulationBL.CreateSeedStarSessionContext(Guid.NewGuid().ToString().Replace("-", "").ToUpper());
           }
           catch (Exception)
           {
               return null;
           }
       }



       public Stream FetchSeedStarProtoDataEpoch(Stream sessionContextAck)
       {
           try
           {

               SimulationBL simulationBL = new SimulationBL();
               return simulationBL.GenerateSeedStarProtoMessage(sessionContextAck);
               //simulationBL.GenerateSeedStarProtoDataEpoch(sessionContextAck);
           }
           catch (Exception)
           {
               return null;
           }
       }

       public Stream FetchBlackBoxProtoMessage(Stream protoMessage)
       {
           try
           {

               SimulationBL simulationBL = new SimulationBL();
               //return simulationBL.GenerateSeedStarProtoDataEpoch(simulationBL.TEST(device, id));
               return simulationBL.GenerateSeedStarProtoMessage(protoMessage);
           }
           catch (Exception)
           {
               return null;
           }
       }


       

    }
}
