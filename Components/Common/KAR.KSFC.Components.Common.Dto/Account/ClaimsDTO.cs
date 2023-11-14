
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto
{
    public class ClaimsDTO
    {
        [DisplayName("Employee Id")]
        public string EmpId { get; set; }

        [DisplayName("Employee Name")]
        public string Name { get; set; }

        [DisplayName("Email")]
        
        public string Email { get; set; }

        [DisplayName("Mobile")]
      
        public string Mobile { get; set; }

        [DisplayName("Role")]
        
        public string Role { get; set; }


        [DisplayName("PAN")]
         
        public string Pan { get; set; }

        [DisplayName("IsPasswordChanged")]
        public bool IsPasswordChanged { get; set; }
        public string IpAddress { get; set; }
    }

    public class PasswordChangeDTO
    {

        [DisplayName("Employee Id")]
        [Required(ErrorMessage = "The Employee Id is required")]
        public string EmpId { get; set; }

        [DisplayName("Old Password")]
        [Required(ErrorMessage = "The Old Password is required")]
        public string OldPassword { get; set; }

        [DisplayName("New Password")]
        [Required(ErrorMessage = "The New Password is required")]
        public string NewPassword { get; set; }
    }

    public class CustomerClaimsDTO
    {
        [DisplayName("PAN")]
        [Required(ErrorMessage = "The PAN is required")]
        [RegularExpression(@"^([A Z]{5}\d{4}[A Z]{ 1})$")]
        public string Pan { get; set; }

        [DisplayName("Role")]
        [Required(ErrorMessage = "The Role is required")]
        public string Role { get; set; }
        public string Email { get; set; }
        public string MobileNumber { get; set; }
        public string IpAddress { get; set; }

    }
}

