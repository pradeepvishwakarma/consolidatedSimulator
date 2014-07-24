using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimulationService.Entities
{
    public class ICADataPointPattern
    {
        public double Current { get; set; }
        public bool Adjusting { get; set; }
        public double Target { get; set; }
    }
}