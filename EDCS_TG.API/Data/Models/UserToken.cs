using Microsoft.AspNetCore.Identity;

namespace EDCS_TG.API.Data.Models
{
    public class UserToken : IdentityUserToken<Guid>
    {
        public virtual User User { get; set; }

    }
}
