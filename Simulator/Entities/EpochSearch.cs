using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationService.Entities
{
    public class EpochSearch
    {
        public string SessionGuid { get; set; }
        public List<uint> EpochIds { get; set; }
        public uint StartEpochID { get; set; }
        public uint EndEpochID { get; set; }

        public EpochSearch()
        {
            this.EpochIds = new List<uint>();
        }
    }
}
