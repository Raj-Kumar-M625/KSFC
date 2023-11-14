using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto
{
    public class SmsInfoDTO
    {

        [DisplayName("Mobile")]
        [Required(ErrorMessage = "The Mobile Number is required")]
        public string Mobile { get; set; }

        [DisplayName("OTP")]
        [Required(ErrorMessage = "The Otp is required")]
        public string otp { get; set; }

        [DisplayName("Status")]
        [Required(ErrorMessage = "The Status is required")]
        public string Status { get; set; }

        [DisplayName("OTPExpDateTime")]
        public DateTime OTPExpDateTime { get; set; }
    }
}
