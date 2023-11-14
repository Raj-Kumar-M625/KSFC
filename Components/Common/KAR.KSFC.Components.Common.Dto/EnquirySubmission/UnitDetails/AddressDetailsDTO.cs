using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails
{
    public class AddressDetailsDTO
    {

        [DisplayName("Unit Fax")]
        public int? UnitFax { get; set; }

        [DisplayName("Unit Mobile Number")]
        [Required(ErrorMessage = "The Mobile Number is required")]
        public Int64? UnitMobileNo { get; set; } 

        [DisplayName("Unit Telephone No")]
        [Required(ErrorMessage = "The Telephone Number is required")]
        public Int64? UnitTelNo { get; set; }

        [DisplayName("Unit Pincode")]
        [Required(ErrorMessage = "The Pincode is required")]
        public int? UnitPincode { get; set; }

        [DisplayName("Unit Address")]
        [Required(ErrorMessage = "Address is required")]
        public string UniitAddress { get; set; }


        [DisplayName("Address Type")]
        public int? AddtypeCd { get; set; }


        [DisplayName("Enquiry")]
        public int EnqtempId { get; set; }


        [DisplayName("Address")]
        public int? EnqAddresssId { get; set; }

        [DisplayName("Email")]
        [Required(ErrorMessage = "The Email Address is required")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}" +
         @"\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\" +
         @".)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Invalid email address")]
        public string UnitEmail { get; set; }

        public virtual AddressTypeMasterDTO AddressTypeMasterDTO { get; set; }
    }
}
