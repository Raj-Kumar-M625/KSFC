using EDCS_TG.API.Helpers.Kutumba;
using EDCS_TG.API.Services.Interfaces;
using log4net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace EDCS_TG.API.Controllers
{
    [Route("/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = "Surveyor")]
    [ResponseCache(Duration = 30, NoStore = true, Location = ResponseCacheLocation.Any)]
    public class KutumbaController : ControllerBase
    {
        private readonly IKutumbaService _KutumbaService;
        private static readonly ILog _log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        public KutumbaController(IKutumbaService KutumbaService)
        {
            _KutumbaService = KutumbaService;
        }

        [HttpPost("GetBeneficiaryData")]
        public async Task<ActionResult<ResultDataList>> GetKutumba(KutumbaRequestDto requestDto)
        {
            try
            {
                
                using HttpClient client = new();
                var response = await _KutumbaService.GetKutumbaData(requestDto);
                if (response != null)
                {
                    
                    return Ok(response);
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
