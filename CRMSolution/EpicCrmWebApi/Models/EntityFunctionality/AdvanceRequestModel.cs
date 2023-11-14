using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class AdvanceRequestModel : IValidatableObject
    {
        public long Id { get; set; }
        public long ActivityId { get; set; }
        [Display(Name = "Employee Id")]
        public long EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "HQ Code")]
        public string HQCode { get; set; }

        [Display(Name = "Client Name")]
        public string EntityName { get; set; }

        [Display(Name = "Agreement Number")]
        public string AgreementNumber { get; set; }

        [Display(Name = "Agreement Status")]
        public string AgreementStatus { get; set; }

        public string Crop { get; set; }

        [Display(Name = "Season Name")]
        public string SeasonName { get; set; }

        [Display(Name = "Unique Id")]
        public string UniqueId { get; set; }

        [Display(Name = "Amount Requested")]
        public decimal AmountRequested { get; set; }

        [Display(Name = "Amount Approved")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Required(ErrorMessage = "Amount is required")]
        [RegularExpression(@"[\d]{0,9}([.][\d]{0,2})?", ErrorMessage = "Enter amount with maximum 2 decimal places.")]
        public decimal AmountApproved { get; set; }

        [Display(Name = "Requested On")]
        public System.DateTime RequestDate { get; set; }

        [Display(Name = "Approved On")]
        public System.DateTime ApprovalDate { get; set; }

        [Display(Name = "Request Status")]
        public string AdvanceReqStatus { get; set; }

        [Display(Name = "Request Note")]
        public string RequestNote { get; set; }

        [Display(Name = "Approve Note")]
        [StringLength(512, ErrorMessage = "Approve note must not exceed 512 characters.")]
        public string ApproveNote { get; set; }

        [Display(Name = "Approved By")]
        public string ReviewedBy { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (AmountApproved > AmountRequested)
            {
                yield return new ValidationResult("Approved amount can't be more than the requested amount");
            }
        }
    }
}