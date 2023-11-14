using System.ComponentModel.DataAnnotations;

namespace Domain.User
{
    public class AspNetUserRoles
    {
        [Key]
        public string UserId { get; set; }

        public string RoleId { get; set; }
    }
}
