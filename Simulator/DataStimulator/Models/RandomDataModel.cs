using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DataStimulator.Models
{
    public class RandomDataModel
    {

        public int ID { get; set; }

        public int FieldID { get; set; }

        public string Value { get; set; }

        public bool boolValue { get; set; }
    }
}