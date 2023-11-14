using Microsoft.AspNetCore.Identity;
namespace EDCS_TG.API.Data.Models
{
    public class Role : IdentityRole<Guid>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }

    }
}
