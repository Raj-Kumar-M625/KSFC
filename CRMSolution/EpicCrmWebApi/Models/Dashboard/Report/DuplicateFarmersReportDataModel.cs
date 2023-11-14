using System;
using System.ComponentModel.DataAnnotations;


namespace EpicCrmWebApi
{
    public class DuplicateFarmersReportDataModel
    {
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [RegularExpression(@"^[1-9][0-9]{11}$", ErrorMessage = "Enter 12 Digits")]
        [Required(ErrorMessage = "Unique Id Required")]

        [Display(Name = "UniqueId")]
        public string UniqueId { get; set; }

        [Display(Name = "Client Name")]
        public string EntityName { get; set; }

        [Display(Name = "Father/Husband Name")]
        [MaxLength(50, ErrorMessage = "Father/Husband name can be maximum 50 characters.")]
        public string FatherHusbandName { get; set; }

        [Display(Name = "Client Type")]
        public string CustomerType { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }

        [Display(Name = "Agreement Count")]
        public int AgreementCount { get; set; }

        [Display(Name = "Village Name")]
        public string HQName { get; set; }

        [Display(Name = "Cluster Name")]
        public string TerritoryName { get; set; }
        [Display(Name = "Entity Number")]
        public string EntityNumber { get; set; }
    }
}