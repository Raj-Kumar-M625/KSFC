using AutoMapper;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data;
using EDCS_TG.API.Dto;
using EDCS_TG.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using EDCS_TG.API.Services.Implementation;
using EDCS_TG.API.Data.Repository.Interfaces;
using Microsoft.Extensions.Configuration.UserSecrets;
using EDCS_TG.API.DTO;
using Microsoft.AspNetCore.Authorization;
using log4net;
using System.Reflection;

namespace EDCS_TG.API.Controller
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UserManagementController: ControllerBase
    {
        
        private readonly IUnitOfWork _repository;
        private readonly IMapper _mapper;
        private KarmaniDbContext _karmaniDbContext;
        private UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        private readonly IUserService _userServices;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        public UserManagementController(IMapper mapper, KarmaniDbContext karmaniDbContext, UserManager<User> userManager, IUserService userservices,
           RoleManager<Role> roleManager)
        {
            //_basicSurveyDetailService = basicSurveyDetailService;
            _mapper = mapper;
            _karmaniDbContext = karmaniDbContext;
            _userManager = userManager;
            _roleManager = roleManager;  
            _userServices = userservices;
        }



        //[HttpGet("users")]
        //public async Task<IQueryable<User>> GetUserDetails( )
        //{
        //    try
        //    {
        //        var User = _userManager.Users.AsQueryable();

        //        return User;
        //    }
        //    catch(Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw ex;
        //    }
        //}


        //[HttpPost("AddUser")]
        //public async Task<ActionResult<UserCreateDto>> addUsers([FromBody] UserCreateDto userDto)
        //{
        //    try
        //    {
        //        var data = _mapper.Map<User>(userDto);
        //        var generateOtp = new Random();
        //        //var otp = generateOtp.Next(0000, 9999);
        //        var otp = 1234;
        //        User newUser = new()
        //        {
        //            PhoneNumber = data.PhoneNumber,
        //            UserName = data.UserName,
        //            SecurityStamp = Guid.NewGuid().ToString(),
        //            OTP = otp,
        //            OTPValidity = DateTime.Now.AddMinutes(5),
        //            FirstName = userDto.FirstName,
        //            LastName = userDto.LastName,
        //            Age = userDto.Age,
        //            DOB = userDto.DOB,
                    

        //        };
                             

        //        var createResult = await _userManager.CreateAsync(newUser);

        //        var addUserToRole = await _userManager.AddToRoleAsync(newUser, userDto.Role);
               
        //        var role = await _roleManager.FindByNameAsync(userDto.Role);

        //        return Ok(data);

        //    }
        //    catch(Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw ex;
        //    }
            
        //}

        //[HttpGet("getUserDetails")]
        //public async Task<ActionResult<IEnumerable<UserCreateDto>>> getUserdetailsList()
        //{
        //    try
        //    {
        //        var result = await _userServices.getUserdetailsList();
        //        if (result.Count() > 0)
        //            return Ok(result);
        //        else
        //            return NoContent();
        //    }
        //    catch(Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
            
        //}
        //[HttpPut("updateuser")]
        //public async Task<ActionResult<User>> updateUser([FromBody] UserCreateDto userCreateDto)
        //{
        //    try
        //    {
        //        var surveyee = _karmaniDbContext.User
        //                     .Where(t => t.Id == userCreateDto.Id);


        //        var entity = surveyee.FirstOrDefault(e => e.Id == userCreateDto.Id);
        //        entity.Status = userCreateDto.Status;

        //        // entity.Status = basicSurveyDetailDto.Status;

        //        _karmaniDbContext.SaveChanges();
        //        return Ok(entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
            

        //}

    }
}
