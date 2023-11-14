using AutoMapper;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using log4net;
using System.Reflection;

namespace EDCS_TG.API.Controller
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SurveyController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly IMapper _mapper;
        private KarmaniDbContext _karmaniDbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public SurveyController(IMapper mapper,
           ISurveyService surveyService, KarmaniDbContext karmaniDbContext)
        {
            _surveyService = surveyService;
            _mapper = mapper;
            _karmaniDbContext = karmaniDbContext;
        }

        [HttpGet("GetSurveyDetails")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<SurveyDto>>> getSurveyDetails()
        {
            try
            {
                var result = await _surveyService.GetSurveyDetailsList();
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



        //[HttpPost("AddSurveyDetailRK")]
        //public async Task<ActionResult<SurveyDto>> addSurveyDetails([FromBody] IEnumerable<SurveyDto> surveyDto)
        //{

        //    try
        //    {
        //        IEnumerable<Survey> data = _mapper.Map<IEnumerable<Survey>>(surveyDto);

        //        foreach (var dto in data)
        //        {
        //            _karmaniDbContext.Survey.Add(dto);
        //        }


        //        _karmaniDbContext.SaveChanges();
        //        return Ok(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Info(ex.Message);
        //        throw;
        //    }
            
        //}

        [HttpPost("SaveSurveyDetailAB")]
        [Authorize(Roles = "Surveyor")]
        public async Task<ActionResult<List<SurveyDto>>> AddSurveyDetails(IEnumerable<SurveyDto> surveyDto, string surveyId)
        {
            try
            {
                var result = await _surveyService.SaveSurveyDetail(surveyDto, surveyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            
        }

        [HttpGet("GetSurveyDetailByid")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<Survey>>> getSurveyDetails(string id)
        {
            try
            {
                var result = await _surveyService.GetSurveyDetailsList();
                var surveyDetails = _karmaniDbContext.Survey
                                   .Where(t => t.SurveyId == id);
                var res = surveyDetails.OrderBy(t => t.QuestionId);
                if (result.Count() > 0)
                    return Ok(res);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            
        }


        [HttpGet("surveyByCategory")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<Survey>>> getSurveyDetailsByCat(string id,int QuestionPaperId)
        {
            try
            {
                var result = await _surveyService.GetSurveyDetailsList();
                var surveyDetails = _karmaniDbContext.Survey
                                   .Where(t => t.SurveyId == id);
                var surveyDetailsList = surveyDetails.Where(t => t.QuestionPaperId == QuestionPaperId);
                var res = surveyDetails.OrderBy(t => t.QuestionId);
                if (result.Count() > 0)
                    return Ok(res);
                else
                    return NoContent();
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            
        }

        [HttpPut("UpdateSurveyDetail")]
        [Authorize(Roles = "Surveyor")]
        public async Task<ActionResult<IEnumerable<SurveyDto>>> updateSurveyDetails([FromBody] IEnumerable<SurveyDto> surveyDto, string SurveyId)
        {
            try
            {
                var result = await _surveyService.UpdateSurveyDetail(surveyDto, SurveyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            
        }

        [HttpGet("SurveyById")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<Survey>>> getSurveyById(string sueveyId){
            try
            {
                var result = _karmaniDbContext.Survey.Where(t => t.SurveyId == sueveyId);
                var res = result.OrderBy(t => t.QuestionId);
                var survey = _mapper.Map<IEnumerable<Survey>>(res);

                if (survey.Count() > 0)
                {
                    return Ok(survey);
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            
        }

    }
}
