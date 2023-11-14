using BusinessLayer;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class STRWeightModel : IValidatableObject
    {
        public long Id { get; set; }

        [Display(Name = "STR Number")]
        [MaxLength(50, ErrorMessage = "STRNumber can be maximum 50 characters long.")]
        [Required]
        public string STRNumber { get; set; }

        [Display(Name = "STR Date")]
        public DateTime STRDate { get; set; }

        [Display(Name = "Truck Wt. at Silo")]
        public decimal EntryWeight { get; set; }

        [Display(Name = "Unloaded Truck Wt.")]
        public decimal ExitWeight { get; set; }

        [Display(Name = "Silo #")]
        [Required]
        [MaxLength(50, ErrorMessage ="Silo # can be maximum 50 characters.")]
        public string SiloNumber { get; set; }

        [Display(Name = "Silo Incharge Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "Silo Incharge Name can be maximum 50 characters.")]
        public string SiloIncharge { get; set; }

        [Display(Name = "Unloading Incharge Name")]
        [Required]
        [MaxLength(50, ErrorMessage = "Unloading Incharge Name can be maximum 50 characters.")]
        public string UnloadingIncharge { get; set; }

        [Display(Name = "Odometer after Unload")]
        public long ExitOdometer { get; set; }

        [Display(Name = "# Bags")]
        public long BagCount { get; set; }

        [Display(Name = "Notes")]
        [MaxLength(100, ErrorMessage = "Notes can be maximum 100 characters.")]
        public string Notes { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; }

        [Display(Name = "STR Date")]
        public string STRDateAsText { get; set; }

        [Display(Name = "% Deduction")]
        public decimal DeductionPercent { get; set; }

        [Display(Name = "Actual Wt. (incl Bags)")]
        public decimal GrossItemWeight => EntryWeight - ExitWeight;

        // removing this - as at the time of Silo, the GrossItemWeight is the wt inclusive of 
        // bags.  Silo Deduction has to happen after bag wt is taken out from total wt.
        // Whereas this number was being calculated on wt with bags - which is not correct.
        //[Display(Name = "After deduction Wt.")]
        //public decimal NetItemWeight => (GrossItemWeight * (100 - DeductionPercent)) / 100.0M;

        public long CyclicCount { get; set; }

        [Display(Name = "# DWS")]
        public long DWSCount { get; set; }
        [Display(Name = "Vehicle #")]
        [Required]
        [MaxLength(50, ErrorMessage = "Vehicle # can be maximum 50 characters.")]
        public string VehicleNumber { get; set; }

        public bool IsEditAllowed { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            STRDate = Helper.ConvertStringToDateTime(STRDateAsText);

            if (STRDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify STR Date.");
                yield break;
            }

            STRDate = STRDate.Date;

            DateTime today = Helper.GetCurrentIstDateTime().Date;
            if (STRDate > today)
            {
                yield return new ValidationResult("STR Date can not be in the future.");
                yield break;
            }

            if (ExitOdometer <= 0)
            {
                yield return new ValidationResult("Invalid Odometer Reading");
                yield break;
            }

            if (BagCount <= 0)
            {
                yield return new ValidationResult($"Invalid Bag Count {BagCount}");
                yield break;
            }

            if (EntryWeight < 0 || ExitWeight < 0)
            {
                yield return new ValidationResult("Weights can't be less than zero.");
                yield break;
            }

            if (GrossItemWeight <= 0)
            {
                yield return new ValidationResult("Invalid weights.");
                yield break;
            }

            if (DeductionPercent < 0 || DeductionPercent > 100.0M)
            {
                yield return new ValidationResult("Invalid Deduction Percentage.");
                yield break;
            }

            // check that STR # is unique
            DomainEntities.STRFilter searchCriteria = new DomainEntities.STRFilter()
            {
                STRNumber = STRNumber.Trim(),
                IsExactSTRNumberMatch = true,
                ApplySTRNumberFilter = true
            };

            ICollection<STRWeight> strWeightRecs = Business.GetSTRWeight(searchCriteria);
            int recCount = (strWeightRecs?.Count ?? 0);
            if (Id == 0 && recCount > 0) // add
            {
                yield return new ValidationResult("STR Number already exist.");
                yield break;
            }

            if (Id > 0) // edit
            {
                if (recCount == 0) // adding a complete new str number
                {
                    ;
                }
                else
                {
                    if (strWeightRecs.First().Id != Id)
                    {
                        // strnumber can exist, but in the same record
                        yield return new ValidationResult("STR Number already exist.");
                        yield break;
                    }
                }

                STRWeight origStrWeightRec = Business.GetSingleSTRWeight(Id);
                if (!origStrWeightRec.IsEditAllowed)
                {
                    yield return new ValidationResult($"STR {origStrWeightRec.STRNumber} can not be edited.");
                    yield break;
                }
            }
        }
    }
}