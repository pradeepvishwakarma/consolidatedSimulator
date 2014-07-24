using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataStimulator.Models;

namespace DataStimulator.Models
{
    public class DataGeneratorModel
    {                
        public DataPatternModel DataPattern { get; set; }
        public DataPointModel DataPoint { get; set; }
    }
}