using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationService.Entities
{
    public class ChunkSearch
    {
        public string SessionGuid { get; set; }
        public List<uint> ChunkIds { get; set; }

        public ChunkSearch()
        {
            this.ChunkIds = new List<uint>();
        }
    }
}
