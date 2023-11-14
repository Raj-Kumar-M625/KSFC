using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class FollowUpTaskViewModel : IValidatableObject
    {
        public long Id { get; set; }

        [Display(Name = "Project Name")]
        public long XRefProjectId { get; set; }

        [Display(Name = "Project Name")]
        public string ProjectName { get; set; }

        [Display(Name = "Task Description")]
        [MaxLength(200, ErrorMessage = "Task Description can be maximum 200 characters long.")]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Activity Type")]
        [MaxLength(50, ErrorMessage = "ActivityType not selected")]
        [Required]
        public string ActivityType { get; set; }

        [Display(Name = "Client Type")]
        [MaxLength(50, ErrorMessage = "ClientType not selected")]
        //[Required]
        public string ClientType { get; set; }

        [Display(Name = "Client Name")]
        [MaxLength(50, ErrorMessage = "Client Name can be maximum 50 characters long.")]
        public string ClientName { get; set; }

        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }

        [Display(Name = "Planned Start Date")]
        public DateTime PlannedStartDate { get; set; }

        [Display(Name = "Planned End Date")]
        public DateTime PlannedEndDate { get; set; }

        [Display(Name = "Actual Start Date")]
        public DateTime ActualStartDate { get; set; }

        [Display(Name = "Actual End Date")]
        public DateTime ActualEndDate { get; set; }

        [Display(Name = "Notes/Comments")]
        public string Comments { get; set; }

        [Display(Name = "Status")]
        [MaxLength(50, ErrorMessage = "Status not selected")]
        [Required]
        public string Status { get; set; }

        public int CyclicCount { get; set; }

        [Display(Name = "IsActive")]
        public bool IsActive { get; set; }
        
        [Display(Name = "IsCreatedOnPhone")]
        public bool IsCreatedOnPhone { get; set; }

        [Display(Name = "Date Created")]
        public DateTime DateCreated { get; set; }

        [Display(Name = "Date Updated")]
        public DateTime DateUpdated { get; set; }

        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        [Display(Name = "Updated By")]
        public string UpdatedBy { get; set; }

        [Display(Name = "Planned Start Date")]
        public string PlannedStartDateAsText { get; set; }

        [Display(Name = "Planned End Date")]
        public string PlannedEndDateAsText { get; set; }

        [Display(Name = "Activity Count")]
        public long ActivityCount { get; set; }

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

    public class FollowUpTaskEntityTypeHQ
    {
        //[Display(Name = "Zone")]
        //[MaxLength(50, ErrorMessage = "Invalid Zone")]
        //public string ZoneCode { get; set; }

        //[Display(Name = "Area")]
        //[MaxLength(50, ErrorMessage = "Invalid Area")]
        //public string AreaCode { get; set; }

        //[Display(Name = "Territory")]
        //[MaxLength(50, ErrorMessage = "Invalid Territory")]
        //public string TerritoryCode { get; set; }

        public long Id { get; set; }

        [Display(Name = "HQ Name")]
        [MaxLength(50, ErrorMessage = "Invalid HQ")]
        public string HQName { get; set; }

        [Display(Name = "HQ")]
        [MaxLength(50, ErrorMessage = "Invalid HQ")]
        [Required]
        public string HQCode { get; set; }

        [Display(Name = "Client Type")]
        [Required]
        public string ClientType { get; set; }

    }

    public class FollowUpTaskEntity : FollowUpTaskEntityTypeHQ
    {

        [Display(Name = "Client Code (Existing)")]
        public string ClientCodeOld { get; set; }

        [Display(Name = "Client Code")]
        [Required]
        public string ClientCode { get; set; }

        [Display(Name = "Client Name (Existing)")]
        public string ClientNameOld { get; set; }

        [Display(Name = "Client Name (Code)")]
        public string ClientName { get; set; }

    }
}