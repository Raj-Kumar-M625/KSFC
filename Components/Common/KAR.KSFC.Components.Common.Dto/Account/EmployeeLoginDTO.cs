using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace KAR.KSFC.Components.Common.Dto
{
    public class EmployeeLoginDTO
    {
        [DisplayName("EmpId")]
        public string EmpId { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Mobile")]
        public string Mobile { get; set; }

        [DisplayName("Ip")]
        public string Ip { get; set; }

        [DisplayName("User Id")]
        public string UserId { get; set; }

        [DisplayName("Public Key")]
        public string PublicKey { get; set; }

        [DisplayName("IsDSC")]
        public bool IsDSC { get; set; }

        [DisplayName("IsForceLogin")]
        public bool IsForceLogin { get; set; }

        [DisplayName("Process")]
        public string Process { get; set; }

        [DisplayName("OTP")]
        
        public string Otp { get; set; }

        [DisplayName("Token")]
        public string Token { get; set; }
    }
}
