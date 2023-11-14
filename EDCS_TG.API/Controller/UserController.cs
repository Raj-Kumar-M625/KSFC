using AutoMapper;
using EDCS_TG.API.Data;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Services.Implementation;
using EDCS_TG.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using EDCS_TG.API.Dto;
using Microsoft.AspNetCore.Authorization;
using log4net;
using System.Reflection;

namespace EDCS_TG.API.Controller
{
    [Route("/v1/users")]
    [ApiController]
    [Authorize(Roles =("Admin"))]
    public class UserController:ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public UserController(IMapper mapper,IUserService userService)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpGet("GetAllUsers")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<UserCreateDto>>> getAllUsers()
        {
            try
            {
                var Users = await _userService.GetAllUsersList();
                if (Users.Count() > 0)
                {
                    return Ok(Users);
                }
                else
                {
                    return NoContent();
                }
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            
            
        }

        [HttpGet("FindUserById")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<User>> findUserById(Guid id)
        {
            try
            {
                var result = await _userService.getUserById(id);
                return result;
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            
        }

        [HttpPut("UpdateUser")]
        public async Task<ActionResult<UserCreateDto>> UpdateUser(UserCreateDto user)
        {
            try
            {
                var result = await _userService.UpdateUserData(user);
                return result;
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
           
        }

        [HttpPost("AddUser")]
        public async Task<ActionResult<UserCreateDto>> AddUser(UserCreateDto user)
        {
            try
            {
                var result = await _userService.CreateUser(user);
                return result;
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            
        }

    }
}
