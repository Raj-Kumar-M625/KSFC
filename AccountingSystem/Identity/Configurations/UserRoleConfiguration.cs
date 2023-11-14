using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity.Configurations
{
    public class UserRoleConfiguration : IEntityTypeConfiguration<IdentityUserRole<string>>
    {
        public void Configure(EntityTypeBuilder<IdentityUserRole<string>> builder)
        {
            builder.HasData(
                new IdentityUserRole<string>
                {
                    RoleId = "354289d9-29ac-4a1b-b37e-42a6906ef3d0",
                    UserId = "3fcf9376-a44d-4d7b-9baa-2238b5c3c8ad"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "0ae71af5-e641-4f43-b8f7-de687f6a15dc",
                    UserId = "40b314bd-64e8-4b7d-b868-679044bc4eba"
                }
            );
        }
    }
}
