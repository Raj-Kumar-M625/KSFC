using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class AddressDTO
    {
        [Display(Name = "S. No.")]
        public int? Id { get; set; }
        [Display(Name = "Address Type")]
        public string AddressType { get; set; }
        [Display(Name = "Address")]
        [Required(ErrorMessage = "Address is rquired.")]
        [MaxLength(400)]
        public string Address { get; set; }
        [Display(Name = "Pin Code")]
        [Required(ErrorMessage = "Pin Code is rquired.")]
        [MaxLength(6)]
        [RegularExpression(@"^([0-9]{6})$", ErrorMessage = "Invalid Pin Code.")]
        public string PinCode { get; set; }
        [Display(Name = "Telephone Number")]
        [MaxLength(12)]
        [RegularExpression(@"^([0-9]{10,12})$", ErrorMessage = "Invalid Telephone Number.")]
        public string TelePhoneNumber { get; set; }
        [Display(Name = "Mobile Number")]
        [Required(ErrorMessage = "Mobile Number is rquired.")]
        [MaxLength(10)]
        [RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
        public string MobileNumber { get; set; }
        [Display(Name = "Email Id")]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\.[a-z]{2,4}", ErrorMessage = "Please enter correct Email Id.")]//(\d+)((\.\d{1,2})?)
        [MaxLength(40)]
        [Required(ErrorMessage = "Email Id is rquired.")]
        public string EmailId { get; set; }
        [Display(Name = "Fax Number")]
        [MaxLength(12)]
        [RegularExpression(@"^([0-9]{10,12})$", ErrorMessage = "Invalid FaxNumber.")]
        public string FaxNumber { get; set; }
    }
}
