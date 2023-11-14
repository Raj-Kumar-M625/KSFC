using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EDCS_TG.API.Data.Models;
using SRS.API.Data.Models;

namespace EDCS_TG.API.Data
{
    public class KarmaniDbContext : IdentityDbContext<User, Role, Guid,
    UserClaim, UserRole, UserLogin,
    RoleClaim, UserToken>
    {

        public KarmaniDbContext(DbContextOptions<KarmaniDbContext> options) : base(options)
        {

        }

        //public DbSet<CodeTable>? CodeTable { get; set; }
        public DbSet<User>? User { get; set; }
        //public DbSet<SurveyType>? SurveyType { get; set; }
        public DbSet<PersonalDetails>? PersonalDetails { get; set; }

        public DbSet<KutumbaDL>? KutumbaDL { get; set; }
        public DbSet<Education>? Education { get; set; }
        public DbSet<Employment>? Employment { get; set; }
        public DbSet<Housing>? Housing { get; set; }
        public DbSet<Health>? Health { get; set; }
        public DbSet<SocialSecurity>? SocialSecurity { get; set; }
        public DbSet<AdditionalInformation>? AdditionalInformation { get; set; }

        public DbSet<BasicSurveyDetail>? BasicSurveyDetails { get; set; }
        public DbSet<Questions> Questions { get; set; }
        public DbSet<Survey> Survey { get; set; }
       
        public DbSet<Office> Office { get; set; }

        public DbSet<UserAssignment> UserAssignment { get; set; }

        public DbSet<SurveyMapper> SurveyMapper { get; set; }

        public DbSet<SurveyImages> SurveyImages { get; set; }

        public DbSet<QuestionPaper> QuestionPaper { get; set; }

        public DbSet<QuestionPaperAnswer> QuestionPaperAnswer { get; set; }

        public DbSet<QuestionPaperQuestion> QuestionPaperQuestion { get; set; }

        public DbSet<Answer> Answer { get; set; }

        public DbSet<SMSLog>? SMSLog { get; set; }

        public DbSet<DownloadSurveyModel> DownloadSurvey { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ModifyIdentityTables(builder);
            SeedRoles(builder);
            SeedUser(builder);
            SeedUserRole(builder);

        }



        private void ModifyIdentityTables(ModelBuilder builder)
        {
            builder.Entity<User>(entity =>

            {

                entity.ToTable("User");
                entity.HasIndex(u => u.PhoneNumber).IsUnique();

                entity.HasMany(e => e.Claims)
                .WithOne(e => e.User)
                .HasForeignKey(uc => uc.UserId)
                .IsRequired();

                entity.HasMany(e => e.Logins)
                .WithOne(e => e.User)
                .HasForeignKey(ul => ul.UserId)
                .IsRequired();


                entity.HasMany(e => e.Tokens)
               .WithOne(e => e.User)
               .HasForeignKey(ut => ut.UserId)
               .IsRequired();

                entity.HasMany(e => e.UserRoles)
                .WithOne(e => e.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            }
                );


            builder.Entity<Role>(entity =>

            {
                entity.ToTable("Role");
                entity.HasMany(e => e.UserRoles)
                .WithOne(e => e.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                entity.HasMany(e => e.RoleClaims)
                .WithOne(e => e.Role)
                .HasForeignKey(rc => rc.RoleId)
                .IsRequired();

            }

            );
            builder.Entity<UserRole>(entity => { entity.ToTable("UserRole"); });
            builder.Entity<UserClaim>(entity => { entity.ToTable("UserClaim"); });
            builder.Entity<UserLogin>(entity => { entity.ToTable("UserLogin"); });
            builder.Entity<UserToken>(entity => { entity.ToTable("UserToken"); });
            builder.Entity<RoleClaim>(entity => { entity.ToTable("RoleClaim"); });





        }

        private void SeedUserRole(ModelBuilder builder)
        {
            builder.Entity<UserRole>().HasData(
                new UserRole
                {
                    UserId = Guid.Parse("C7DF5E5B-E435-4EEC-980A-740163479711"),
                    RoleId = Guid.Parse("7AE3440A-5B41-452F-B86C-812AC4533053"),
                },

                 new UserRole
                 {
                     UserId = Guid.Parse("DBE8EB30-8410-4820-8BAA-DA489F4DBAE2"),
                     RoleId = Guid.Parse("42877DAA-E637-45DA-8ABA-DC6A7FB66C5D"),
                 }


                );
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<Role>().HasData(
              new Role
              {
                  //Id = 1,
                  Id = Guid.Parse("42877DAA-E637-45DA-8ABA-DC6A7FB66C5D"),
                  Name = "Admin",
                  ConcurrencyStamp = "10",
                  NormalizedName = "Admin"
              },
                new Role
                {

                    Id = Guid.Parse("7AE3440A-5B41-452F-B86C-812AC4533053"),
                    Name = "Surveyor",
                    ConcurrencyStamp = "11",
                    NormalizedName = "Surveyor"
                    /* //Id = 2,
                     Id = Guid.Parse("7AE3440A-5B41-452F-B86C-812AC4533053"),
                     Name = "Surveyor",
                     ConcurrencyStamp = "2",
                     NormalizedName = "Surveyor"*/
                }
            );
        }



        private void SeedUser(ModelBuilder builder)
        {
            builder.Entity<User>().HasData(
                new User
                {

                    Id = Guid.Parse("DBE8EB30-8410-4820-8BAA-DA489F4DBAE2"),
                    UserName = "Admin User",
                    NormalizedUserName = "Admin User",
                    PhoneNumber = "9999999910",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    OTP = 1234,
                    OTPValidity = DateTime.Now.AddMinutes(5),
                    FirstName = "Test 1",
                    LastName = "Test 2",
                    Age = 20,
                    DOB = DateTime.Now
                },
                new User
                {

                    Id = Guid.Parse("C7DF5E5B-E435-4EEC-980A-740163479711"),
                    UserName = "Surveyor User",
                    NormalizedUserName = "Surveyor User",
                    PhoneNumber = "9999999998",
                    SecurityStamp = Guid.NewGuid().ToString(),
                    OTP = 1234,
                    OTPValidity = DateTime.Now.AddMinutes(5),
                    FirstName = "Test 3",
                    LastName = "Test 4",
                    Age = 20,
                    DOB = DateTime.Now


                }
                // new User
                // {
                //     //Id = 3,
                //    // Id = Guid.Parse("00000000-0000-0000-0000-000000000000"),
                //     UserName = "Admin User",
                //     NormalizedUserName = "Admin User",
                //     PhoneNumber = "8792815137",
                //     SecurityStamp = Guid.NewGuid().ToString(),
                //     OTP = 1234,
                //     OTPValidity = DateTime.Now.AddMinutes(5)
                // }

                );

        }

        //private void ModifyIdentityTables(ModelBuilder builder)
        //{
        //    builder.Entity<User>(entity =>
        //    {
        //        entity.ToTable("User");
        //        entity.HasIndex(u => u.PhoneNumber).IsUnique();

        //        // Each User can have many UserClaims
        //        entity.HasMany(e => e.Claims)
        //            .WithOne(e => e.User)
        //            .HasForeignKey(uc => uc.UserId)
        //            .IsRequired();

        //        // Each User can have many UserLogins
        //        entity.HasMany(e => e.Logins)
        //            .WithOne(e => e.User)
        //            .HasForeignKey(ul => ul.UserId)
        //            .IsRequired();

        //        // Each User can have many UserTokens
        //        entity.HasMany(e => e.Tokens)
        //            .WithOne(e => e.User)
        //            .HasForeignKey(ut => ut.UserId)
        //            .IsRequired();

        //        // Each User can have many entries in the UserRole join table
        //        entity.HasMany(e => e.UserRoles)
        //            .WithOne(e => e.User)
        //            .HasForeignKey(ur => ur.UserId)
        //            .IsRequired();
        //    });

        //    builder.Entity<Role>(entity =>
        //    {
        //        entity.ToTable("Role");

        //        // Each Role can have many entries in the UserRole join table
        //        entity.HasMany(e => e.UserRoles)
        //            .WithOne(e => e.Role)
        //            .HasForeignKey(ur => ur.RoleId)
        //            .IsRequired();

        //        // Each Role can have many associated RoleClaims
        //        entity.HasMany(e => e.RoleClaims)
        //            .WithOne(e => e.Role)
        //            .HasForeignKey(rc => rc.RoleId)
        //            .IsRequired();
        //    });

        //    builder.Entity<UserRole>(entity => { entity.ToTable("UserRole"); });
        //    builder.Entity<UserClaim>(entity => { entity.ToTable("UserClaim"); });
        //    builder.Entity<UserLogin>(entity => { entity.ToTable("UserLogin"); });
        //    builder.Entity<UserToken>(entity => { entity.ToTable("UserToken"); });
        //    builder.Entity<RoleClaim>(entity => { entity.ToTable("RoleClaim"); });
        //}
        //private void SeedRoles(ModelBuilder builder)
        //{
        //    builder.Entity<Role>().HasData(
        //        new Role
        //        {
        //            Id = 1,
        //            Name = "SuperAdmin",
        //            ConcurrencyStamp = "1",
        //            NormalizedName = "SuperAdmin"
        //        },
        //        new Role
        //        {
        //            Id = 2,
        //            Name = "Surveyor",
        //            ConcurrencyStamp="2",
        //            NormalizedName = "Surveyor"
        //        }
        //    );
        //}

        //private void SeedUser(ModelBuilder builder)
        //{
        //    builder.Entity<User>().HasData(
        //        new User
        //        {
        //            Id = 1,
        //            UserName = "Admin User",
        //            NormalizedUserName = "Admin User",
        //            PhoneNumber = "9999999999",
        //            SecurityStamp = Guid.NewGuid().ToString()
        //        },
        //        new User
        //        {
        //            Id = 2,
        //            UserName = "Surveyor User",
        //            NormalizedUserName = "Surveyor User",
        //            PhoneNumber = "9999999998",
        //            SecurityStamp = Guid.NewGuid().ToString()
        //        }
        //        );
        //}

        //private void SeedUserRole(ModelBuilder builder)
        //{
        //    builder.Entity<UserRole>().HasData(
        //        new UserRole
        //        {
        //            UserId = 1,
        //            RoleId = 1
        //        },
        //        new UserRole
        //        {
        //            UserId = 2,
        //            RoleId = 2
        //        }
        //        );
        //}
    }
}
