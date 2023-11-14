using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class TerminateAgreementRequestModel
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

        [Display(Name = "Farmer Name")]
        public string EntityName { get; set; }

        public long EntityAgreementId { get; set; }

        [Display(Name = "Agreement Number")]
        public string AgreementNumber { get; set; }

        [Display(Name = "Agreement Status")]
        public string AgreementStatus { get; set; }

        public string Crop { get; set; }

        [Display(Name = "Season Name")]
        public string SeasonName { get; set; }

        [Display(Name = "Unique Id")]
        public string UniqueId { get; set; }

        [Display(Name = "Request Reason")]
        public string RedFarmerReqReason { get; set; }

        [Display(Name = "Request Status")]
        public string RedFarmerReqStatus { get; set; }

        public string Notes { get; set; }

        [Display(Name = "Reviewed By")]
        public string ReviewedBy { get; set; }

        [Display(Name = "Review Date")]
        public System.DateTime ReviewDate { get; set; }

        [Display(Name = "Request Date")]
        public DateTime RequestDate { get; set; }

        public bool IsReviewed => !String.IsNullOrEmpty(ReviewedBy);

        public bool IsApproved => RedFarmerReqStatus.Equals("Approved", StringComparison.OrdinalIgnoreCase);
    }
}