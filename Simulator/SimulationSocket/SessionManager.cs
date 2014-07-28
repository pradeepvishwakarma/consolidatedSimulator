using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using com.deere.proto;
using ProtoBuf;
using SimulationBussinessLayer;
using SimulationService.SimulationBussinessLayer.Enums;

namespace SimulationSocket
{
    enum AppType
    {
        UnknownApp,
        SeedStarApp,
        HarvestApp,
        TillageApp,
        CobarApp        
    }

    enum StreamingMode
    {
        StreamWithoutPlottingInfo,
        StreamWithPlottingInfo
    }

    class CustomEntity
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }

        CustomEntity(int entityId, string entityName)
        {
            this.EntityId = entityId;
            this.EntityName = entityName;
        }
    }
    
    class CFFCYInfo
    {
        public CustomEntity Client { get; set; }
        public CustomEntity Farm { get; set; }
        public CustomEntity Field { get; set; }
        public List<CustomEntity> Crops { get; set; }
        public List<CustomEntity> Brands { get; set; }
        public CustomEntity Operator { get; set; }
        public long StartTime { get; set; }
        public long EndTimeDelta { get; set; }
    }

    class SessionManager
    {
        public int SessionGuidCount { get; set; }
        public int SessionEpochCount { get; set; }
        public int SessionChunkCount { get; set; }
        public int SkipSession { get; set; }
        public int SkipChunk { get; set; }
        public int SkipEpoch { get; set; }
        public int ChangeCFFCY { get; set; }
        public List<int> SkipEpochList { get; set; }
        public List<int> SkipChunkList { get; set; }
        public CFFCYInfo CFFCYInformation { get; set; }
        public SimulationPattern simulationPattern { get; set; }
        public AppType CurrentAppType { get; set; }
        public StreamingMode CurremtStreamMode { get; set; }
        public bool PauseSteaming { get; set; }
        public string LiveStreamingSessionGuid { get; set; }
        private ProtoSessionContext pSessionContext;

        public void RandomizeEpoch()
        {
            this.SkipEpochList = new List<int>();
            for (int i = 0; i < SessionChunkCount; i++)
            {
                int value = Randomize(SessionEpochCount, SkipEpoch);
                SkipEpochList.Add(value != 0 ? (value + (i * SessionEpochCount)) : 0);
            }
        }
        public void RandomizeChunk()
        {
            SkipChunk = Randomize(SessionChunkCount, SkipChunk);
        }       

        private int Randomize(int value, int skipNum)
        {
            if (skipNum == 0 || skipNum > value)
            {
                return 0;
            }
            else
            {
                Random random = new Random();
                int randomNo = random.Next(1, skipNum);
                return randomNo;
            }
        }


        public ProtoSessionContext CreateStartSessionContext()
        {
            ProtoSessionContext sessionContext = DataGenerator.CreateStartSessionContext(this.LiveStreamingSessionGuid, this.CFFCYInformation, this.CurrentAppType);
            this.pSessionContext = sessionContext; 
            return sessionContext;
        }

        public ProtoSessionContext CreateEndSessionContext()
        {
            ProtoSessionContext sessionContext = DataGenerator.CreateEndSessionContext(this.pSessionContext);
            this.pSessionContext = sessionContext;
            return sessionContext;
        }

        public ProtoDataEpochTransmitted CreateEpochTransmitted()
        {
            ProtoDataEpochTransmitted epochTransmitted = DataGenerator.CreateEpochTransmitted(this.simulationPattern, this.CurrentAppType);
            return epochTransmitted;
        }

        private string GetSimulationSessionGuid()
        {
            string sessionGuid = Guid.NewGuid().ToString();
            return sessionGuid;
        }



    }
       
}
