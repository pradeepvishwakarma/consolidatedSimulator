using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using com.deere.proto;
using ProtoBuf;
using SimulationBussinessLayer;
using SimulationService.SimulationBussinessLayer.Enums;


namespace SimulationSocket
{
    public class DataGenerator
    {
        public static ProtoSessionContext CreateStartSessionContext(string sessionGuid, CFFCYInfo cffcyInfo, AppType appType)
        {
            return null;
        }

        public static ProtoSessionContext CreateEndSessionContext(ProtoSessionContext startSessionContext)
        {
            return null;
        }

        public static ProtoDataEpochTransmitted CreateEpochTransmitted(SimulationPattern simulationPattern, AppType appType)
        {
            return null;
        }
    }
}