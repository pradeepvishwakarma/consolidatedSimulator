using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimulationService.Entities
{
    public class VarietyAssignment
    {
        public string Erid{get;set;}
        public string Name{get;set;}
        public string BrandGuid{get;set;}
        public uint EICCropID{get;set;}
        public uint FucntionID{get;set;}
        public uint ColorSpace{get;set;}
        public string Color{get;set;}
        public List<uint> RowElementID{get;set;}
    }
}
