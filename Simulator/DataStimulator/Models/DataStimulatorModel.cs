using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DataAnnotationsExtensions;
using DataStimulator.Models;

namespace DataStimulator.Models
{
    public class DataStimulatorModel : IValidatableObject
    {
        public int Type { get; set; }

        public int FieldID { get; set; }

        public string Fieldname { get; set; }

        public DataGeneratorModel DataGenerator { get; set; }

        //[Digits(ErrorMessage = "Please enter a number greater than zero.")]
        [RegularExpression(@"^\d+\.?\d*$", ErrorMessage = "Please enter a positive number")]
        //[Min(0.0, ErrorMessage="Please enter a number greater than or equal to zero.")]
        public double FieldValue { get; set; }
        //public int PlotType { get; set; }

        public RandomDataModel RandomData { get; set; }

        public DataUXModel DataUXModel { get; set; }

        public VarietyInfoModel VarietyModel { get; set; }

        public BrandModel BrandModel { get; set; }

        public CropModel CropModel { get; set; }

        public SectionControlModel sectionControlModel { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DataGenerator != null && DataGenerator.DataPattern != null)
            {
                if ((FieldID == 6) || (FieldID == 7))
                {
                    if (Type == 2)
                    {
                        if (DataGenerator.DataPattern.MaximumValue < DataGenerator.DataPattern.MinimumValue)
                        {
                            yield return new ValidationResult(Fieldname + ":- Max value must be greater than Min Value");
                        }
                    }

                }
                else
                {
                    if (DataGenerator.DataPattern.MaximumValue < DataGenerator.DataPattern.MinimumValue)
                    {
                        yield return new ValidationResult(Fieldname + ":- Max value must be greater than Min Value");
                    }

                }
            }
        }

        //private IEnumerable<ValidationResult> CompareMaxMinValues(ValidationContext validationContext)
        //{
        //    if (DataGenerator.DataPattern.MaximumValue < DataGenerator.DataPattern.MinimumValue)
        //    {
        //        yield return new ValidationResult(Fieldname + ":- Max value must be greater than Min Value");
        //    }
        //}
    }
}
