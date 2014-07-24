using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationService.Entities
{
    public class CatchUpPattern
    {
        public byte[] SessionContext { get; set; }
        public List<byte[]> TransmittedEpochs { get; set;}
        public CatchUpPattern()
        {
            this.TransmittedEpochs = new List<byte[]>();
        }

    }
}
