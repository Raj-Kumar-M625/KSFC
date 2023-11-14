using Microsoft.AspNetCore.Identity;

namespace EDCS_TG.API.Data.Models
{
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        public virtual Role Role { get; set; }
    }
}
