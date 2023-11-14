using AutoMapper;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace EDCS_TG.API.Controller
{
    [Route("/v1/Download")]
    [ApiController]
    [Authorize(Roles = ("Admin"))]
    [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
    public class DownloadSurveyController : ControllerBase
    {
        private readonly IDownloadSurveyService _downloadSurveyService;
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment environment;
        private readonly IMapper _mapper;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public DownloadSurveyController(IDownloadSurveyService downloadSurveyService, IConfiguration configuration, IMapper mapper)
        {
            _downloadSurveyService = downloadSurveyService;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet("DownloadAllSurvey")]
        public  async Task<ActionResult<IEnumerable<DownloadSurveyModel>>> GetAllDownload()
        {
            try
            {
                var result = await _downloadSurveyService.GetAllDownload();
                return Ok(result);
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
           
           
        }

        [HttpGet("DownloadIndividualSurvey")]
        public async Task<ActionResult<IEnumerable<DownloadSurveyModel>>> GetSurveyDownloadById(string SurveyId)
        {
            try
            {
                var result = await _downloadSurveyService.GetIndividualSurveyDownload(SurveyId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
           
            
        }

        [HttpPost("DownloadFilteredSurvey")]
        public async Task<ActionResult<IEnumerable<DownloadSurveyModel>>> DownloadFilteredSurvey(SearchFilter filter)
        {
            try
            {
                var result = await _downloadSurveyService.FilterSurveyDownload(filter);
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
