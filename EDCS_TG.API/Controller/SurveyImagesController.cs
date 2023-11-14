using EDCS_TG.API.Data.Models;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Implementation;
using EDCS_TG.API.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace EDCS_TG.API.Controller
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SurveyImagesController : ControllerBase
    {
        private ISurveyImagesService _surveyImagesService;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public SurveyImagesController(ISurveyImagesService surveyImagesService)
        {
            _surveyImagesService = surveyImagesService;
        }

        [HttpGet("GetAllImages")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<SurveyImages>>> getAllImages()
        {
            try
            {
                var result = await _surveyImagesService.GetAllImages();
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

        [HttpGet("GetSurveyImagesById")]
        [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
        public async Task<ActionResult<IEnumerable<SurveyImages>>> getImagesById(string surveyId)
        {
            try
            {
                var result = await _surveyImagesService.GetImagesBySurveyId(surveyId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
           
        }

        [HttpPost("addImage")]
        [Authorize(Roles = "Surveyor")]
        public async Task<ActionResult<IEnumerable<SurveyImages>>> addSurveyImage([FromBody]IEnumerable<SurveyImages> surveyImages)
        {
            try
            {
                var result = await _surveyImagesService.addSurveyImage(surveyImages);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
            
        }
    }
}
