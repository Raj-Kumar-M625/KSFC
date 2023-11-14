using KAR.KSFC.API.ServiceFacade.External.Interface;
using KAR.KSFC.Components.Common.Dto.SMS;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using System.Threading.Tasks;

namespace KAR.KSFC.API.Controllers
{
   
    public class MessageController : BaseApiController
    {
        private readonly ISmsService _smsService;
        public MessageController(ISmsService smsService)
        {
            _smsService = smsService;
        }

        [HttpPost, Route("SendSMS")]
        public async Task<IActionResult> OtpGeneration([FromBody] SmsDataModel model)
        {
            if (model == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, CustomErrorMessage.E12));
            }
            var response=await _smsService.SendSms(model);
            return Ok(new ApiResultResponse(response, "Success"));
        }

    }
}
