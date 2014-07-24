using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions.ClientValidation;
using DataAnnotationsExtensions;



namespace DataStimulator.Models
{
    public class DataUXModel
    {

        public int ID { get; set; }

        [Required(ErrorMessage = "DataID Value is required.")]
        public int? DataID { get; set; }

        [Required(ErrorMessage = "RepDomainID Value is required.")]
        public int? RepDomainID { get; set; }

        [Required(ErrorMessage = "Color Value is required.")]
        public string Color { get; set; }

        [Required(ErrorMessage = "ColorSpace Value is required.")]
        public int? ColorSpace { get; set; }

        [Required(ErrorMessage = "Frequency Value is required.")]
        public int? Frequency { get; set; }

        [Required(ErrorMessage = "NoOfEpochs Value is required.")]
        public int? NoOfEpochs { get; set; }

   }
}