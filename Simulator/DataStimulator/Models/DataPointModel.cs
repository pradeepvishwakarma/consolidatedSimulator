using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;

namespace DataStimulator.Models
{
    public class DataPointModel
    {
        [Required(ErrorMessage = "Please enter a valid  value.")]
        public double Current { get; set; }

        [Required(ErrorMessage = "Please enter a valid  value.")]
        public double Target { get; set; }
        //public float Adjusting { get; set; }
        public bool Adjusting { get; set; }
    }
}