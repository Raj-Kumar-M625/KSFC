using KAR.KSFC.API.ServiceFacade.External.Interface;
using KAR.KSFC.Components.Common.Dto.Email;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;

using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace KAR.KSFC.API.Controllers
{
    public class EmailController : BaseApiController
    {
        private readonly IEmailService _mailService;
        public EmailController(IEmailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("SendEmail")]
        public async Task<IActionResult> Send([FromForm] EmailServiceRequest request)
        {
            var result=await _mailService.SendEmailAsync(request);
            if (result)
            {
                return Ok(new ApiResultResponse(true, "Success"));
            }
            return new BadRequestObjectResult(new ApiResponse(400, "Email send failed."));
        }

    }
}
