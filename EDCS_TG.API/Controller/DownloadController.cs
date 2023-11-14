using AutoMapper;
using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data;
using EDCS_TG.API.DTO;
using EDCS_TG.API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using log4net;
using System.Reflection;

namespace EDCS_TG.API.Controller
{
    [Route("/v1/[controller]")]
    [ApiController]
    //[Authorize(Roles = ("Surveyor"))]
    public class DownloadController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IDownloadService _downloadService;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public DownloadController(IConfiguration configuration, IDownloadService downloadService 
            )
        {
            _configuration = configuration;
            _downloadService = downloadService;
          
        }

      

        [HttpGet("DownloadSurveys")]
        public async Task<FileContentResult> GetSurveys()
        {
            try
            {
                var data = await _downloadService.GetSurveys();
                string jsonString = JsonSerializer.Serialize(data);
                var fileName = _configuration["JSON:FileName"];
                var mimeType = "text/plain";
                var fileBytes = Encoding.ASCII.GetBytes(jsonString);
                return new FileContentResult(fileBytes, mimeType)
                {
                    FileDownloadName = fileName
                };
            }
            catch(Exception ex)
            {
                _log.Info(ex.Message);
                throw;
            }
           

        }
    }
}