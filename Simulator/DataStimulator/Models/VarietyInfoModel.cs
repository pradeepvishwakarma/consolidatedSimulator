using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataStimulator.Models
{
    public class VarietyInfoModel
    {
        public int ID { get; set; }

        [Required(ErrorMessage = "Erid is required.")]
        public string Erid { get; set; }

        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }
     
        public string BrandGuid { get; set; }
      
        public int CropID { get; set; }

        [Required(ErrorMessage = "ColorSpace Value is required.")]
        public int? ColorSpace { get; set; }

        [Required(ErrorMessage = "Color Value is required.")]
        public string Color { get; set; }

      
    }
}