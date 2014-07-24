using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationService.Entities
{
    public class UserInfo
    {
        public string Operator { get; set; }
        public string Farm { get; set; }
        public string Field { get; set; }
        public string Client { get; set; }
        public string CropName { get; set; }
        public string OperatorLastModified { get; set; }
    }
}
