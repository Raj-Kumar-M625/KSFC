using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto
{
    public class RegistrationDTO
    {

        [DisplayName("Mobile Number")]
        public string MobileNum { get; set; }

        [DisplayName("OTP")]
        public string Otp { get; set; }

        [DisplayName("Process")]
        public string  Process { get; set; }


        [DisplayName("Pan")]
       
        public string PanNum { get; set; }
    }
}
