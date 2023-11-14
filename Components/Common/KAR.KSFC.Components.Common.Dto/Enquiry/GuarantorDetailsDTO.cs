using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class GuarantorDetailsDTO
    {
        [Display(Name = "S. No.")]
        public int? Id { get; set; }
        [Display(Name = "Guarantor Name")]
        [MaxLength(30)]
        [Required(ErrorMessage = "Please enter name")]
        public string GuarantorName { get; set; }
        [Display(Name = "Address")]
        [MaxLength(40)]
        public string Addess { get; set; }
        [Display(Name = "Pincode")]
        [MaxLength(6)]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Invalid Pin Code.")]
        public string PinCode { get; set; }
        [Display(Name = "PAN of Guarantor")]
        [MaxLength(10)]
        [RegularExpression("^([A-Za-z]){5}([0-9]){4}([A-Za-z]){1}$", ErrorMessage = "Invalid PAN Number")]
        [Required(ErrorMessage = "Please enter PAN Number")]
        public string Pan { get; set; }
        [Display(Name = "Domicile Status")]
        public string DomicileStatus { get; set; }
        [MaxLength(12)]
        [RegularExpression(@"^([0-9]{12})$", ErrorMessage = "Invalid Adhaar.")]
        public string Adhaar { get; set; }
        public BankDetailsDTO BankDetails { get; set; }
        public bool ConsentForCIBILScore { get; set; }
    }
}
