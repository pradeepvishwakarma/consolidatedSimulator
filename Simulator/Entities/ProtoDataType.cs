using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class ProtoDataType
    {
        public int DataID { get; set; }
        public string TypeName { get; set; }
        public double Offset { get; set; }
        public double ScaleFactor { get; set; }
        public string UOM { get; set;}
    }
}
