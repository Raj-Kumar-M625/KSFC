using AutoMapper;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Authorization;
using log4net;
using System.Reflection;

namespace EDCS_TG.API.Controller
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    
    public class BasicSurveyDetailController:ControllerBase
    {

        private readonly IBasicSurveyDetailService _basicSurveyDetailService;
        private readonly IMapper _mapper;
        private KarmaniDbContext _karmaniDbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public BasicSurveyDetailController(IMapper mapper,
           IBasicSurveyDetailService basicSurveyDetailService, KarmaniDbContext karmaniDbContext)
        {
            _basicSurveyDetailService = basicSurveyDetailService;
            _mapper = mapper;
            _karmaniDbContext = karmaniDbContext;
        }

        [HttpGet("getBasicSurveyDetailsByUser")]
        [Authorize(Roles = "Surveyor")]
        [ResponseCache(Duration = 30,NoStore = true,Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<BasicSurveyDetail>>> getBasicSurveyDetails(Guid UserId)
        {
            try
            {
                var result = await _basicSurveyDetailService.GetBasicSurveyDetailListByUser(UserId);
                if (result.Count() > 0)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
           
        }

        [HttpGet("GetAllSurveyList")]
        [Authorize(Roles = "Admin")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<BasicSurveyDetail>>> GetAllSurveyList()
        {
            try
            {
                var result = await _basicSurveyDetailService.GetAllBasicSurveyDetailList();
                if (result.Count() > 0)
                    return Ok(result);
                else
                    return NoContent();
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            
        }

        [HttpGet("getBasicSurveyById")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<BasicSurveyDetail>> getBasicSurveyDetailById(string id)
        {
            try
            {
                var result = _karmaniDbContext.BasicSurveyDetails.FirstOrDefault(T => T.SurveyId == id);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            
        }



        [HttpPost("AddBasicSurvey")]
        [Authorize(Roles = "Surveyor")]
        public async Task<ActionResult<BasicSurveyDetailDto>> addBasicSurveyDetails(BasicSurveyDetailDto basicSurveyDetailDto)
        {
            try
            {
                var surveyee = _karmaniDbContext.BasicSurveyDetails
                           .Where(t => t.SurveyId == basicSurveyDetailDto.SurveyId);
                var entity = _karmaniDbContext.BasicSurveyDetails.FirstOrDefault(e => e.SurveyId == basicSurveyDetailDto.SurveyId);

                if (entity != null)
                {
                    entity.number = basicSurveyDetailDto.number;
                    entity.Name = basicSurveyDetailDto.Name;
                    entity.DOB = basicSurveyDetailDto.DOB;
                    entity.Age = basicSurveyDetailDto.Age;
                    entity.Address = basicSurveyDetailDto.Address;
                    entity.District = basicSurveyDetailDto.District;
                    entity.Taluk = basicSurveyDetailDto.Taluk;
                    entity.Hobli = basicSurveyDetailDto.Hobli;
                    entity.VillageOrWard = basicSurveyDetailDto.VillageOrWard;
                    entity.PinCode = basicSurveyDetailDto.PinCode;
                    entity.GenderByBirth = basicSurveyDetailDto.GenderByBirth;
                    entity.Email = basicSurveyDetailDto.Email;
                    entity.PresentAddress = basicSurveyDetailDto.PresentAddress;
                    entity.PresentDistrict = basicSurveyDetailDto.PresentDistrict;
                    entity.PresentHobli = basicSurveyDetailDto.PresentHobli;
                    entity.PresentPinCode = basicSurveyDetailDto.PresentPinCode;
                    entity.PresentTaluk = basicSurveyDetailDto.PresentTaluk;
                    entity.PresentVillageOrWard = basicSurveyDetailDto.PresentVillageOrWard;
                    //_karmaniDbContext.Update(entity);
                    _karmaniDbContext.SaveChanges();
                    return Ok(entity);

                }

                var data = _mapper.Map<BasicSurveyDetail>(basicSurveyDetailDto);
                data.Created_Date = DateTime.Now;
                _karmaniDbContext.BasicSurveyDetails.Add(data);
                _karmaniDbContext.SaveChanges();
                return Ok(data);
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            
        }

        [HttpPut("updateBasicSurvey")]
        [Authorize(Roles = ("Surveyor"))]
        public async Task<ActionResult<BasicSurveyDetail>> updateSurveyDetails([FromBody] BasicSurveyDetailDto basicSurveyDetailDto)
        {
            try
            {
                var surveyee = _karmaniDbContext.BasicSurveyDetails
                             .Where(t => t.SurveyId == basicSurveyDetailDto.SurveyId);


                var entity = surveyee.FirstOrDefault(e => e.SurveyId == basicSurveyDetailDto.SurveyId);
                if (entity != null)
                    entity.Status = basicSurveyDetailDto.Status;

                // entity.Status = basicSurveyDetailDto.Status;

                _karmaniDbContext.SaveChanges();
                return Ok(entity);
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            

        }

        [HttpGet("GetSurveyByDistrict")]
        [Authorize(Roles = "Admin")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<BasicSurveyDetailDto>>> GetSurveyByDistrict(string district)
        {
            try
            {
                var result = await _basicSurveyDetailService.GetSurveyDetailByDistrict(district);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            
        }

        [HttpGet("getSurveyByTaluk")]
        [Authorize(Roles = "Admin")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<BasicSurveyDetailDto>>> getSurveyByTaluk(string taluk)
        {
            try
            {
                var result = await _basicSurveyDetailService.GetSurveyDetailByTaluk(taluk);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
        }

     


    }
}
