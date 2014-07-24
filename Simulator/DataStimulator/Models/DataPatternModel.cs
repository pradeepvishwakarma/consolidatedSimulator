using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions.ClientValidation;
using DataAnnotationsExtensions;

namespace DataStimulator.Models
{
    public class DataPatternModel
    {
       [Digits(ErrorMessage = "Please enter a non-negative non-decimal number.")]
        public int? DisplayIndexID { get; set; }

        [Required(ErrorMessage = "Minimum Value is required.")]
        [Numeric(ErrorMessage = "Please enter a  double value.")]
        public double MinimumValue { get; set; }


        [Required(ErrorMessage = "Maximum Value is required.")]
        [Numeric(ErrorMessage = "Please enter a  double value.")]
        public double MaximumValue { get; set; }

        [Required(ErrorMessage = "Step is required.")]
        [Numeric(ErrorMessage = "Please enter a postive double value.")]
        [Min(0.0,ErrorMessage = "Please enter a postive double value.")]
        public double Step { get; set; }

        public bool Cycle { get; set; }

        public bool Randomize { get; set; }

        public double EventValue { get; set; }

        public double DefaultValue { get; set; }

        public double EventProbability { get; set; }


       
    }
}