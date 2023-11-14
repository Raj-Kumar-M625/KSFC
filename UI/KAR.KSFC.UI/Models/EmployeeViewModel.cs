using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.UI.Models
{
    public class EmployeeViewModel
    {
        [Required(ErrorMessage = "Employee id is required.")]
        public string EmpId { get; set; }
        public string Password { get; set; }
        public string OtpEntered { get; set; }
        public bool IsDscRequired { get; set; }
        public string DSCFileName { get; set; }

        public string  Mobile { get; set; }
        public string JwtAccessToken { get; set; }
    }
}
