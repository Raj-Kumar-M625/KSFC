using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class STRTagModel : IValidatableObject
    {
        public long Id { get; set; }

        [Display(Name = "STR Number")]
        [MaxLength(50, ErrorMessage = "STRNumber can be maximum 50 characters long.")]
        [Required]
        public string STRNumber { get; set; }

        [Display(Name = "STR Date")]
        public DateTime STRDate { get; set; }

        [Display(Name = "# Loads")]
        public long STRCount { get; set; }

        [Display(Name = "# DWS")]
        public long DWSCount { get; set; }

        [Display(Name = "# Bags")]
        public long BagCount { get; set; }

        [Display(Name = "Gross Weight")]
        public decimal GrossWeight { get; set; }

        [Display(Name = "Net Weight")]
        public decimal NetWeight { get; set; }

        [Display(Name = "STR Date")]
        public string STRDateAsText { get; set; }

        public string Status { get; set; }
        public long CyclicCount { get; set; }
        public long STRWeightCyclicCount { get; set; }

        [Display(Name = "Is Silo Checked?")]
        public bool IsFinal { get; set; }

        [Display(Name = "Cancel STR?")]
        public bool IsCancel { get; set; }

        [Display(Name = "Start Odometer")]
        public long StartOdo { get; set; }

        public STRWeight STRWeight { get; set; }

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

            // check that str number is unique
            STRNumber = Utils.TruncateString(STRNumber, 50);
            DomainEntities.STRFilter searchCriteria = new DomainEntities.STRFilter()
            {
                STRNumber = STRNumber,
                IsExactSTRNumberMatch = true,
                ApplySTRNumberFilter = true
            };

            ICollection<STRTag> strTagRecs = Business.GetSTRTag(searchCriteria);
            int recCount = (strTagRecs?.Count ?? 0);
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
                    if (strTagRecs.First().Id != Id)
                    {
                        // strnumber can exist, but in the same record
                        yield return new ValidationResult("STR Number already exist.");
                        yield break;
                    }
                }


                DomainEntities.STRTag origRec = Business.GetSingleSTRTag(Id);
                if (!origRec.IsEditAllowed)
                {
                    yield return new ValidationResult($"STR {origRec.STRNumber} is marked as Silo Checked. Edit operation is not allowed.");
                    yield break;
                }

                if (!origRec.STRNumber.Equals(STRNumber, StringComparison.OrdinalIgnoreCase))
                {
                    // in Edit mode, user is not allowed to change STR # and mark it as final 
                    // at same time.
                    if (IsFinal)
                    {
                        yield return new ValidationResult("STR # and Status can't be changed together.");
                        yield break;
                    }
                }

                // if Weighment record is not there, user can't mark it as final
                STRWeight = Business.GetSTRWeight(origRec.STRNumber);

                if (STRWeight == null && IsFinal)
                {
                    yield return new ValidationResult("STR can't be marked as final, as Weighment data is not available.");
                    yield break;
                }

                // model state is marked as final - cyclic count for STRWeight record must also match
                // as we are going to update status in STRWeight table also.
                if (IsFinal && STRWeightCyclicCount != STRWeight.CyclicCount)
                {
                    yield return new ValidationResult("An error occured while saving changes. Please refresh the page and try again.");
                    yield break;
                }
            }
        }
    }
}