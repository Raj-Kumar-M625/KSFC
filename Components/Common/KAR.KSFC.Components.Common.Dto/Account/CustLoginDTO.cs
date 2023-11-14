
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace KAR.KSFC.Components.Common.Dto
{
    public class CustLoginDTO
    {
        [DisplayName("Pan")]
        public string PanNum { get; set; }


        [DisplayName("Process")]
        public string Process { get; set; }

        [DisplayName("IsForceLogin")]
        public bool IsForceLogin { get; set; }

        [DisplayName("MobileNum")]
        public string MobileNum { get; set; }

        [DisplayName("OTP")]
        public string Otp { get; set; }

        [DisplayName("Token")]
        public string Token { get; set; }
    }
}
