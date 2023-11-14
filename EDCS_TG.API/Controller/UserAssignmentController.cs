using AutoMapper;
using EDCS_TG.API.Data;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Services.Implementation;
using EDCS_TG.API.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace EDCS_TG.API.Controller
{
    [Route("/v1/AssignedUsers")]
    [ApiController]
    [Authorize]
    public class UserAssignmentController:ControllerBase
    {
        private readonly IUserAssignmentService _userAssignmentService;
        private readonly KarmaniDbContext _karmaniDbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public UserAssignmentController(IUserAssignmentService userAssignmentService, KarmaniDbContext karmaniDbContext)
        {
            _userAssignmentService = userAssignmentService;
            _karmaniDbContext = karmaniDbContext;
        }

        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<joinModel>>> getAllAssignedUsers()
        //{
        //    try
        //    {
        //        var result = await _userAssignmentService.GetAllAssignedUsers();

        //        if (result != null && result.Count() > 0)
        //        {
        //            return Ok(result);
        //        }

        //        return Ok(result);
        //    }
        //    catch(Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
           
        //}

        //[HttpGet("id")]
        //public async Task<ActionResult<UserAssignment>> getAssignedUser(int id)
        //{
        //    try
        //    {
        //        var result = await _userAssignmentService.GetAssignedUserById(id);

        //        if (result != null)
        //        {
        //            return Ok(result);
        //        }

        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
           
        //}

        //[HttpGet("taluk")]
        //public async Task<ActionResult<UserAssignment>> getAssignedByTaluk(string taluk)
        //{
        //    try
        //    {
        //        var result = await _userAssignmentService.getAssignedUserByTaluk(taluk);

        //        if (result != null)
        //        {
        //            return Ok(result);
        //        }

        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
           
        //}

        //[HttpGet("district")]
        //public async Task<ActionResult<UserAssignment>> getAssignedByDistrict(string district)
        //{
        //    try
        //    {
        //        var result = await _userAssignmentService.getAssignedUserByDistrict(district);

        //        if (result != null)
        //        {
        //            return Ok(result);
        //        }

        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
            
        //}

        //[HttpGet("hobli")]
        //public async Task<ActionResult<UserAssignment>> getAssignedByhobli(string hobli)
        //{
        //    try
        //    {
        //        var result = await _userAssignmentService.getAssignedUserByHobli(hobli);

        //        if (result != null)
        //        {
        //            return Ok(result);
        //        }

        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
            
        //}






        //[HttpPost("/assign")]
        //public async Task<ActionResult<IEnumerable<UserAssignment>>> AssignUser(IEnumerable<UserAssignment> userAssignment)
        //{
        //    try
        //    {
        //        var findUser = new UserAssignment();
        //        var list = new List<UserAssignment>();
        //        foreach (var user in userAssignment)
        //        {
        //            findUser = _karmaniDbContext.UserAssignment.FirstOrDefault(t => t.UserId == user.UserId);
        //            if (findUser == null)
        //            {
        //                var result = await _userAssignmentService.AssignUser(user);
        //                list.Add(user);
        //            }
        //            //else
        //            //{
        //            //    return BadRequest("User Already Assigned!");
        //            //}

        //        }
        //        return Ok(list);
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
            
        //}


        //[HttpDelete]
        //public async Task<ActionResult<UserAssignment>> RemoveUser(Guid id)
        //{
        //    try
        //    {
        //        var result = await _userAssignmentService.RemoveUser(id);
        //        if (result == null)
        //        {
        //            return NotFound();
        //        }
        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
            
        //}

    }
}
