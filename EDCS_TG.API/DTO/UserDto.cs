using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.Dto
{

    public class UserLoginDto
    {
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Phone number is not valid.")]
        public string? PhoneNumber { get; set; }
    }

    public class UserCreateDto
    {
        
        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        public int? OTP { get; set; }
       
        public int? Sid { get; set; }
       
        public string? SurveyorId { get; set; }

        public int? Status { get; set; }
        public DateTime? OTPValidity { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Phone number is not valid.")]
        public string? PhoneNumber { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }
        public bool? TwoFactorEnabled { get; set; }

        public bool? LockoutEnabled { get; set; }
        public bool? EmailConfirmed { get; set; }
        public Guid Id { get; set; }

        public int? AccessFailedCount { get; set; }

        public DateTime? DOB { get; set; }

        [NotMapped]
        public string? Role { get; set; }
        public string? District { get; set; }
        public string? Taluk { get; set; }
        public string? Hobli { get; set; }

        public string? District_Code { get; set; }

        public string? Taluk_Code { get; set; }

        public string? Hobli_Code { get; set; }


    }


    public class VerifyPhoneDto
    {
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Phone number is not valid.")]
        [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "OTP is required")]
        public int? OTP { get; set; }
    }
}
