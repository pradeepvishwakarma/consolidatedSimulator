using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SimulationService.Entities
{
    public class JDDataPattern
    {
        public double CurrentValue { get; set; }
        public bool IsIncrementing { get; set; }
        public bool Stop { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double Step { get; set; }
        public bool Cycle { get; set; }
        public bool Randomized { get; set; }
        public double DefaultValue { get; set; }
        public double EventValue { get; set; }
        public double EventPropability { get; set; }  
    
    }

   
}