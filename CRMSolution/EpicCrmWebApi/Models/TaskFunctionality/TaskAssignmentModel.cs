using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class TaskAssignmentModel : IValidatableObject
    {
        [Display (Name = "Id")]
        public int Id { get; set; }
        public long XRefTaskId { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Task Description")]
        public string Description { get; set; }

        [Display(Name = "Employee Code")]
        public long EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        [Required(ErrorMessage ="The Employee Name field is required")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Display(Name = "Is Assigned")]
        public bool IsAssigned { get; set; }

        [Display(Name = "Self Assigned")]
        public bool IsSelfAssigned { get; set; }

        [Display(Name = "Comments")]
        [MaxLength(2000, ErrorMessage = "Comments can be maximum 2000 characters long.")]
        public string Comments { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        [Display(Name = "Activity Count")]
        public long ActivityCount { get; set; }

        [Display(Name = "Start Date")]
        public string StartDateAsText { get; set; }

        [Display(Name = "End Date")]
        public string EndDateAsText { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            StartDate = Helper.ConvertStringToDateTime(StartDateAsText);
            EndDate = Helper.ConvertStringToDateTime(EndDateAsText);

            if (StartDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify Start Date.");
                yield break;
            }

            if (EndDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Please specify End Date.");
                yield break;
            }

            StartDate = StartDate.Date;
            EndDate = EndDate.Date;

            if (StartDate > EndDate)
            {
                yield return new ValidationResult("Start Date can not greater than End Date");
                yield break;
            }

        }
    }
}