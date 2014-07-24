using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationService.Entities
{
    public class CustomSampleUx
    {
        public int DataID { get; set; }
        public int RepDomainID { get; set; }
        public int ColorSpace { get; set; }
        public int Frequency { get; set; }
        public int NumOfEpochs { get; set; }
        public uint Color { get; set; }
    }
}
