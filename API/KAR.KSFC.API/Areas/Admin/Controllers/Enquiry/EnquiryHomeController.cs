using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Admin.Controllers.Enquiry
{
    public class EnquiryHomeController : BaseApiController
    {
        private readonly IEnquiryHomeService _enquiryHome;
        private readonly ILogger _logger;

        public EnquiryHomeController(IEnquiryHomeService enquiryHome, ILogger logger)
        {
            _enquiryHome = enquiryHome;
            _logger = logger;
        }
        [HttpGet, Route("GetAllEnquriesForAdmin")]
        public async Task<IActionResult> GetAllEnquiriesAsync(CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetAllEnquriesForAdmin");

                var enquiry = await _enquiryHome.GetAllEnquiriesForAdminAsync(token).ConfigureAwait(false);
                if (enquiry == null)
                {
                    return new NotFoundObjectResult(new ApiException(404, "Enquiry Details not exists!"));
                }

                _logger.Information("Completed - GetAllEnquriesForAdmin");

                return Ok(new ApiResultResponse(200, enquiry, "Success."));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured while GetAllEnquriesForAdmin. Error message is: {ex.Message} {Environment.NewLine} The stack trace is: {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet, Route("UpdateEnquiryStatus")]
        public async Task<IActionResult> UpdateEnquiryStatus(int enqId,int EnqStatus, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdateEnquiryStatus");

                bool IsUpdated = await _enquiryHome.UpdateEnquiryStatus(enqId, EnqStatus, token).ConfigureAwait(false);
                if (!IsUpdated)
                {
                    return new NotFoundObjectResult(new ApiException(404, "Enquiry details not exists!"));
                }
                _logger.Information("Completed - UpdateEnquiryStatus");

                return Ok(new ApiResultResponse(200, true, "Enquiry Details Updated Successfully"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured while UpdateEnquiryStatus. Error message is: {ex.Message} {Environment.NewLine} The stack trace is: {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }
    }
}
