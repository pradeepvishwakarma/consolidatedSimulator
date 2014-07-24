using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DataStimulator.Models
{
    public class BrandModel
    {
        [Required(ErrorMessage = "BrandErid is required.")]
        public string BrandID { get; set; }
        [Required(ErrorMessage = "BrandName is required.")]
        public string BrandName { get; set; }
    }
}