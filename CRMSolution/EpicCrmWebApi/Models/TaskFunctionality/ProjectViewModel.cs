using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EpicCrmWebApi
{
    public class ProjectViewModel : IValidatableObject
    {
        public long Id { get; set; }

        [Display(Name = "Name")]
        [MaxLength(50, ErrorMessage = "Project Name can be maximum 50 characters long.")]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Description")]
        [MaxLength(200, ErrorMessage = "Project Description can be maximum 200 characters long.")]
        //[MinLength(1, ErrorMessage = "Project Description cann't be blank")]
        public string Description { get; set; }

        [Display(Name = "Category")]
        [MaxLength(200, ErrorMessage = "Select a Project Category")]
        [Required]
        public string Category { get; set; }

        [Display(Name = "Planned Start Date")]
        public DateTime PlannedStartDate { get; set; }

        [Display(Name = "Planned End Date")]
        public DateTime PlannedEndDate { get; set; }


        [Display(Name = "Actual Start Date")]
        public DateTime ActualStartDate { get; set; }
        
        [Display(Name = "Actual End Date")]
        public DateTime ActualEndDate { get; set; }
        
        [Display(Name = "Status")]
        public string Status { get; set; }
        
        [Display(Name = "Current User")]
        public string CurrentUser { get; set; }
        
        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }
        public int CyclicCount { get; set; }

        [Display(Name = "Planned Start Date")]
        public string PlannedStartDateAsText { get; set; }

        [Display(Name = "Planned End Date")]
        public string PlannedEndDateAsText { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            PlannedStartDate = Helper.ConvertStringToDateTime(PlannedStartDateAsText);
            PlannedEndDate = Helper.ConvertStringToDateTime(PlannedEndDateAsText);

            if (PlannedStartDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify Planned Start Date.");
                yield break;
            }

            if (PlannedEndDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify Planned End Date.");
                yield break;
            }

            PlannedStartDate = PlannedStartDate.Date;
            PlannedEndDate = PlannedEndDate.Date;

            if (PlannedStartDate > PlannedEndDate)
            {
                yield return new ValidationResult("Planned Start Date can not greater than Planned End Date");
                yield break;
            }

        }
    }

}
