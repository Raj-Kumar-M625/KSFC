using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EntityWorkFlowDetailEditModel : IValidatableObject
    {
        public long Id { get; set; }

        [Display(Name = "Planned From Date")]
        public DateTime PlannedFromDate { get; set; }

        [Display(Name = "Planned End Date")]
        public DateTime PlannedEndDate { get; set; }

        [Display(Name = "Planned From Date")]
        public string PlannedFromDateAsText { get; set; }

        [Display(Name = "Planned End Date")]
        public string PlannedEndDateAsText { get; set; }

        [Display(Name = "Operation Status")]
        public int IsActiveAsNumber { get; set; }

        [MaxLength(100, ErrorMessage = "Notes can be max 100 characters")]
        public string Notes { get; set; }

        public bool IsActive { get; set; }

        public string Phase { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // from date conversion
            PlannedFromDate = Helper.ConvertStringToDateTime(PlannedFromDateAsText);

            if (PlannedFromDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify From Date.");
                yield break;
            }

            PlannedFromDate = PlannedFromDate.Date;

            // End Date conversion
            PlannedEndDate = Helper.ConvertStringToDateTime(PlannedEndDateAsText);

            if (PlannedEndDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify End Date.");
                yield break;
            }

            PlannedEndDate = PlannedEndDate.Date;

            // IsActive
            IsActive = (IsActiveAsNumber == 1);

            if (PlannedEndDate < PlannedFromDate)
            {
                yield return new ValidationResult("Invalid Date range.");
                yield break;
            }
        }
    }
}