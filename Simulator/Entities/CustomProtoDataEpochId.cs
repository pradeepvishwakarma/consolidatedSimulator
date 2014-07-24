using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationService.Entities
{
    public class CustomProtoDataEpochId
    {
       public string SessionGuid { get; set; }
       //public int ChunkSeqNum { get; set; }
       public int DataEpochSeq { get; set; }
    }
}
