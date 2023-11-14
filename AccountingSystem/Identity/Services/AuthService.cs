using Application.Contracts.Identity;
using Application.Contracts.Infrastructure;
using Application.Models.Identity;
using Application.Models.Infrastructure;
using Domain.User;
using Identity.Models;
using Microsoft.AspNetCore.Identity;
using Persistence;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Services
{
    public class AuthService:IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSender _emailSender;
        private readonly AccountingDbContext _dbContext;
        public AuthService(UserManager<ApplicationUser> userManager,
                            SignInManager<ApplicationUser> signInManager,
                            IEmailSender emailSender,
                            AccountingDbContext dbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _dbContext = dbContext;
        }


        public async Task<OtpResponse> SendOtp(OtpRequest otpRequest)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(otpRequest.Email);

            if (user == null)
            {
                throw new Exception($"User with {otpRequest.Email} not found.");
            }

            string otpCode = await _userManager.GenerateUserTokenAsync(user,"PasswordlessTokenProvider","passwordless-auth");


            var email = new Email
            {
                To = otpRequest.Email,
                Body = $"Your One-Time Password (OTP) for login is : {otpCode} . This Code is only valid for 60 minutes.",
                Subject = "OTP for Login"
            };

            if (otpRequest.Email != "admin@localhost.com")
            {
                 _emailSender.SendEmail(email);
            }

            OtpResponse otpResponse = new OtpResponse
            {
                Status = true,
                Message = $"OTP Sent to mail Id : {otpRequest.Email}",
                OTP = otpCode
            };

            return otpResponse;
        }

        public async Task<bool> Login(AuthRequest authRequest)
        {
            var user = await _userManager.FindByEmailAsync(authRequest.Email);

            if (user == null)
            {
                throw new Exception($"User with {authRequest.Email} not found.");
            }

            bool result;

            // Temporary - Don't send email and validate for admin user need to be corrected before Prod Deployment
            if (authRequest.Email != "admin@localhost.com")
            {
                result = await _userManager.VerifyUserTokenAsync(user,"PasswordlessTokenProvider","passwordless-auth",authRequest.OTP);
            }
            else
            {
                result = true;
            }

            if (!result)
            {
                return result;
            }
            else
            {
                await _signInManager.SignInAsync(user,isPersistent: false,null);
            }

            return result;
        }

        public async Task<RegistrationResponse> Register(RegistrationRequest registrationRequest)
        {
            var existingUser = await _userManager.FindByNameAsync(registrationRequest.UserName);

            if (existingUser != null)
            {
                throw new Exception($"User name '{registrationRequest.UserName}' already exists.");
            }

            var user = new ApplicationUser
            {
                Email = registrationRequest.Email,
                FirstName = registrationRequest.FirstName,
                LastName = registrationRequest.LastName,
                PhoneNumber = registrationRequest.Phone,
                UserName = registrationRequest.UserName,
                EmailConfirmed = true,
                TwoFactorEnabled = true,
                LockoutEnabled = false
            };

            var existingEmail = await _userManager.FindByEmailAsync(registrationRequest.Email);

            if (existingEmail == null)
            {
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user,"User");
                    return new RegistrationResponse() { UserId = user.Id };
                }
                else
                {
                    throw new Exception($"{result.Errors}");
                }
            }
            else
            {
                throw new Exception($"Email {registrationRequest.Email} already exists.");
            }
        }

        public IQueryable<UsersList> GetUsersLists()
        {
            try
            {


                //var existingUser =  _userManager.Users;

                //var role =_userManager.GetUsersInRoleAsync(existingUser)

                var res = from td in _dbContext.AspNetUserRoles
                          join ts in _dbContext.AspNetUsers on td.UserId equals ts.Id
                          join tr in _dbContext.AspNetRoles on td.RoleId equals tr.Id
                          select new UsersList
                          {
                              Id = ts.Id,
                              EmailId = ts.Email,
                              UserName = ts.UserName,
                              MobileNumber = ts.PhoneNumber,
                              Role = tr.Name

                          };
                return res;

            }
            catch(Exception ex) 
            {
                throw ex;
            }

        }
    }
}
