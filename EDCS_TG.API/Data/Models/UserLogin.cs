using Microsoft.AspNetCore.Identity;
namespace EDCS_TG.API.Data.Models
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public virtual User User { get; set; }
    }
}
