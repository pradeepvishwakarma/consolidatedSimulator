using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataStimulator.Models
{
    public class CropModel
    {
        [Required(ErrorMessage = "EICCropID is required.")]
        public int? Erid { get; set; }
        [Required(ErrorMessage = "CropName is required.")]
        public string CropName { get; set; }
    }
}