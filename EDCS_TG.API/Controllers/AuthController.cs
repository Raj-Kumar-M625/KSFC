using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;
using System.Text;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using EDCS_TG.API.Helpers;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
//using System.Web.Http.Cors;





namespace EDCS_TG.API.Controllers

{
    [Route("/v1/[controller]")]
    //[EnableCors(origins: "http://localhost:3000", headers: "*", methods: "*")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;
        private readonly RoleManager<Role> _roleManager;
        private readonly SmsService _smsService;
        private readonly UserManager<User> _userManager;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public AuthController(IUnitOfWork repository, UserManager<User> userManager, RoleManager<Role> roleManager,
       IConfiguration configuration, SmsService smsService)
        {
            _repository = repository;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _smsService = smsService;
        }

        [HttpGet("phone")]
        [AllowAnonymous]
    public async Task<IActionResult> Login([FromQuery] string phone)
    {
            
        try
        {
                var user = await _repository.UserRepository.FindByPhoneAsync(phone);

                if (user == null)
                    return StatusCode(StatusCodes.Status404NotFound, "Invalid phone number, user does not exist!");

                var generateOtp = new Random();
                user.OTP = generateOtp.Next(100000, 999999);
                //user.OTP = 123456;
                user.OTPValidity = DateTime.Now.AddMinutes(5);


                await _repository.UserRepository.Update(user);

            // Send OTP 
            var templateId = _configuration.GetValue<string>("SmsSettings:OtpTemplate");
            var message = "Your OTP is " + user.OTP + ". Directorate of EDCS.";
            var response = _smsService.SendOtp(phone, message, templateId,user.OTP.ToString()!);
            //_smsService.SendOtp(userLogin.PhoneNumber, message, templateId);
            
            if (response != null)
                return Ok("OTP generated successfully!");

            return StatusCode(StatusCodes.Status304NotModified, "Failed to send OTP");
        }
        catch (Exception ex)
        {
                
                _log.Info(ex.Message);
                throw;
                //return StatusCode(StatusCodes.Status304NotModified, "Failed to generate OTP");
        }
    }


    [HttpPost("phone/verify")]
    [AllowAnonymous]
    public async Task<IActionResult> VerifyPhone([FromBody] Dto.VerifyPhoneDto verifyPhone)
    {
            try
            {
                var user = await _repository.UserRepository.FindByPhoneAsync(verifyPhone.PhoneNumber);

                if (user == null)
                    return StatusCode(StatusCodes.Status404NotFound, "Invalid phone number, user does not exist!");

                if (user.OTPValidity < DateTime.Now)
                    return StatusCode(StatusCodes.Status401Unauthorized, "OTP Expired, Please create again");

                if (user.OTP != verifyPhone.OTP)
                    return StatusCode(StatusCodes.Status401Unauthorized, "OTP Invalid, Please create again");

                var userRoles = await _userManager.GetRolesAsync(user);
                var authClaims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                foreach (var userRole in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                var token = GetToken(authClaims);
                user.OTP = null;
                await _repository.UserRepository.Update(user);
                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userRoles = userRoles.FirstOrDefault(),
                    userId = user.Id,

                });
            }
            catch(Exception ex)
            {
                throw;
            }
        
    }

    private JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Secret"]));

        var token = new JwtSecurityToken(
            _configuration["JwtSettings:ValidIssuer"],
            _configuration["JwtSettings:ValidAudience"],
            expires: DateTime.Now.AddDays(365),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );

        return token;
    }

    }
}
