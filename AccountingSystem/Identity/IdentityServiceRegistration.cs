using Application.Contracts.Identity;
using Application.Models.Identity;
using Identity.Models;
using Identity.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Identity
{
    public static class IdentityServiceRegistration
    {
        public static IServiceCollection ConfigureIdentityServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            services.AddDbContext<BocwAccountingIdentityDbContext>(options =>
            options.UseSqlServer(
                configuration.GetConnectionString("AccountingIdentityConnectionString"), b => b.MigrationsAssembly(typeof(BocwAccountingIdentityDbContext).Assembly.FullName)));

            //services.AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<COASIdentityDbContext>().AddDefaultTokenProviders();

            services.AddIdentity<ApplicationUser, IdentityRole>(otps =>
            {
                otps.Password.RequireUppercase = false;
                otps.Password.RequireNonAlphanumeric = false;
                otps.Password.RequireLowercase = false;
            }
            )
            .AddUserManager<ApplicationUserManager>()
            .AddEntityFrameworkStores<BocwAccountingIdentityDbContext>()
            .AddDefaultTokenProviders()
            .AddPasswordlessTokenProvider();

            services.AddTransient<IAuthService, AuthService>();

            //services.AddAuthentication(options =>
            //{
            //    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //})
            //    .AddJwtBearer(o =>
            //    {
            //        o.TokenValidationParameters = new TokenValidationParameters
            //        {
            //            ValidateIssuerSigningKey = true,
            //            ValidateIssuer = true,
            //            ValidateAudience = true,
            //            ValidateLifetime = true,
            //            ClockSkew = TimeSpan.Zero,
            //            ValidIssuer = configuration["JwtSettings:Issuer"],
            //            ValidAudience = configuration["JwtSettings:Audience"],
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]))
            //        };
            //    });


            return services;

        }
    }
}
