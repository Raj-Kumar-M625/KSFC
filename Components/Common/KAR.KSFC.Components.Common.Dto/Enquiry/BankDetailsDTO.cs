using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class BankDetailsDTO
    {
        [Display(Name = "Name of Bank")]
        [MaxLength(20)]
        [Required(ErrorMessage = "Name Of Bank is rquired.")]
        public string NameOfBank { get; set; }
        [Display(Name = "Name of Branch")]
        [MaxLength(20)]
        //[Required(ErrorMessage = "Name Of Branch is rquired.")]
        public string NameOfBranch { get; set; }
        [Display(Name = "Pin Code")]
        [MaxLength(6)]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Invalid Pin Code.")]
        //[Required(ErrorMessage = "PIN Code is rquired.")]
        public string PinCode { get; set; }
        [Display(Name = "Account Name")]
        [MaxLength(30)]
        public string AccountName { get; set; }
        [Display(Name = "Account Number")]
        [MaxLength(16)]
        [RegularExpression(@"^([0-9]{14,16})$", ErrorMessage = "Invalid Account Number.")]
        public string AccountNumber { get; set; }
        [Display(Name = "IFSC Code")]
        [MaxLength(11)]
        [RegularExpression(@"^[A-Z]{4}0[A-Z0-9]{6}$", ErrorMessage = "Invalid IFSC Code. Ex-AAAA01111111, AAAA0AAAAAA")]
        public string IFSCCode { get; set; }
        [Display(Name = "Account Type")]
        [MaxLength(20)]
        public string AccountType { get; set; }
        [Display(Name = "Mobile Number")]
        [MaxLength(10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string MobileNumber { get; set; }
        [Display(Name = "Email")]
        [MaxLength(40)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct Email Id.")]
        public string Email { get; set; }
    }
}
