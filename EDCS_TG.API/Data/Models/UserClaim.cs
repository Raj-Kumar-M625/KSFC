using Microsoft.AspNetCore.Identity;

namespace EDCS_TG.API.Data.Models
{
    public class UserClaim : IdentityUserClaim<Guid>
    {

        public virtual User User { get; set; }
    }
}
