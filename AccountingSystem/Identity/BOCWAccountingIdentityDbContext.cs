using Domain.Bill;
using Domain.Payment;
using Identity.Configurations;
using Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity
{
    public class BocwAccountingIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public BocwAccountingIdentityDbContext(DbContextOptions<BocwAccountingIdentityDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());

        }
    }
}
