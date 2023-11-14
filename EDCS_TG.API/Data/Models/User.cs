using MessagePack;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EDCS_TG.API.Data.Models
{
    public class User : IdentityUser<Guid>
    {
        public int? OTP { get; set; }
        public string? UserName { get; set; }
        [NotMapped]

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int? Sid { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public string? SurveyorId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public int? Age { get; set; }

        [DisplayFormat(DataFormatString = "{dd-MM-yyyy:0}", ApplyFormatInEditMode = true)]
        public DateTime? DOB { get; set; }
        public int? Status { get; set; }
        public DateTime? OTPValidity { get; set; }
        public string? District { get; set; }
        public string? Taluk { get; set; }
        public string? Hobli { get; set; }

        public string? District_Code { get; set; }

        public string? Taluk_Code { get; set; }

        public string? Hobli_Code { get; set; }


        public virtual ICollection<UserClaim> Claims { get; set; }
        public virtual ICollection<UserLogin> Logins { get; set; }
        public virtual ICollection<UserToken> Tokens { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }


    }
}
