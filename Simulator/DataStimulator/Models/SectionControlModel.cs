using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataStimulator.Models
{
    public class SectionControlModel
    {
        public int SectionID { get; set; }

        public double Width { get; set; }

        public bool isEnabled { get; set; }

        public double LateralOffset { get; set; }
    }
}