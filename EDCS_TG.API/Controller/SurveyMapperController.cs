using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data;
using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Authorization;
using log4net;
using System.Reflection;

namespace EDCS_TG.API.Controller
{
    [Route("/v1/SurveyMapper")]
    [ApiController]
    [Authorize]
    public class SurveyMapperController:ControllerBase
    {
        private readonly KarmaniDbContext _karmaniDbContext;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public SurveyMapperController(KarmaniDbContext karmaniDbContext)
        {
            _karmaniDbContext = karmaniDbContext;
        }

        [HttpPost("MapSurvey")]
        [Authorize(Roles = "Surveyor")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<SurveyMapper>> MapSurvey([FromBody] SurveyMapper surveyMapper)
        {
            try
            {
                var find = _karmaniDbContext.SurveyMapper.FirstOrDefault(T => T.surveyId == surveyMapper.surveyId && T.CategoryId == surveyMapper.CategoryId);

                if (find == null)
                {
                    var result = _karmaniDbContext.SurveyMapper.Add(surveyMapper);
                }
                _karmaniDbContext.SaveChanges();
                return Ok(surveyMapper);
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
           
        }

        [HttpGet("GetMappedSurveyByID")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<SurveyMapper>>> MapSurvey(string Surveyid)
        {
            try
            {
                var result = _karmaniDbContext.SurveyMapper.Where(T => T.surveyId == Surveyid);
                if (result != null)
                {
                    return Ok(result);
                }
                else
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
