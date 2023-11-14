using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EpicCrmWebApi
{
    public class BankAccountViewModel
    {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Area Code")]
        [MaxLength(10, ErrorMessage = "Area Code can be maximum 10 characters")]
        public string AreaCode { get; set; }

        [Display(Name = "Area Name")]
        public string AreaName { get; set; }

        [Required]
        [Display(Name = "Bank Name")]
        [MaxLength(50, ErrorMessage = "Bank Name can be maximum 50 characters")]
        public string BankName { get; set; }

        [Required]
        [Display(Name = "Branch Name")]
        [MaxLength(50, ErrorMessage = "Branch Name can be maximum 50 characters")]
        public string BranchName { get; set; }

        [Required]
        [Display(Name = "Bank Phone")]
        [MaxLength(20, ErrorMessage = "Bank Phone can be maximum 20 characters")]
        public string BankPhone { get; set; }

        [Required]
        [Display(Name = "Account Number")]
        [MaxLength(50, ErrorMessage = "Account Number can be maximum 50 characters")]
        public string AccountNumber { get; set; }

        [Required]
        [MaxLength(50, ErrorMessage = "IFSC can be maximum 50 characters")]
        public string IFSC { get; set; }

        [Required]
        [Display(Name = "Account Holder Name")]
        [MaxLength(50, ErrorMessage = "Account holder name can be maximum 50 characters")]
        public string AccountName { get; set; }

        [Required]
        [Display(Name = "Account Holder Address")]
        [MaxLength(50, ErrorMessage = "Account holder address can be maximum 50 characters")]
        public string AccountAddress { get; set; }

        [Required]
        [Display(Name = "Account Holder Email")]
        [MaxLength(50, ErrorMessage = "Account holder email can be maximum 50 characters")]
        public string AccountEmail { get; set; }


        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
} 