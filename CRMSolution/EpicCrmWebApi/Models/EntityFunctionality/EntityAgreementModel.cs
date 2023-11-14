using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CRMUtilities;
using System.Web.Http.ModelBinding;
//using System.Web.Mvc;

namespace EpicCrmWebApi
{
    public class EntityAgreementModel : IValidatableObject
    {
        public bool isValidDate = false;

        public long Id { get; set; }
        public long EntityId { get; set; }

        //[RegularExpression(@"^[1-9]$", ErrorMessage = "Select Season")]
        public long WorkflowSeasonId { get; set; }
        
        [Display(Name ="Season Name")]
        public string WorkflowSeasonName { get; set; }

        [Display(Name = "Crop Name")]
        public string TypeName { get; set; }

        [Display(Name = "Land Size (Acres)")]
        public decimal LandSizeInAcres { get; set; }

        [Display(Name = "Agreement Number")]
        //[RegularExpression(@"^([a-zA-Z0-9/]{2,50})$", ErrorMessage = "Invalid Agreement Number")]
        public string AgreementNumber { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Select Agreement Status")]
        public string Status { get; set; }

        [Display(Name = "Client Type")]
        public string EntityType { get; set; }

        [Display(Name = "Client Name")]
        public string EntityName { get; set; }

        public string UniqueId { get; set; }

        [Display(Name = "PassBook Received?")]
        public bool IsPassBookReceived { get; set; }

        [Display(Name = "PassBook Received On")]
        public DateTime PassBookReceivedDate { get; set; }

        [Display(Name = "Rate / Kg. (Rs.)")]
        public decimal RatePerKg { get; set; }

        public int DWSCount { get; set; }
        public int IssueReturnCount { get; set; }
        public int AdvanceRequestCount { get; set; }
        public bool HasWorkflow { get; set; }

        public long ActivityId { get; set; }
        public string CreatedBy { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Date display/storing anomaly - 01-Jan-01 is actually not DateTime.MinValue
            // but 01 Jan 2001
            if (PassBookReceivedDate.Year < 2010)
            {
                PassBookReceivedDate = DateTime.MinValue;
            }

            if (PassBookReceivedDate != DateTime.MinValue)
            {
                if (IsPassBookReceived)
                {
                    DateTime todayDate = Helper.GetCurrentIstDateTime().Date;
                    if (PassBookReceivedDate > todayDate)
                    {
                        yield return new ValidationResult("PassBook received date can't be a future date.");
                    }
                }
                else
                {
                    yield return new ValidationResult("PassBook received date can't be given without passbook received.");
                }
            }
            else
            {
                if (IsPassBookReceived)
                {
                    yield return new ValidationResult("PassBook received date is required for passbook received.");
                }
            }
        }
    }
}