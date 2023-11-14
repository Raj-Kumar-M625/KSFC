using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.ProjectDetails;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
//using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Customer.Controllers.EnquirySubmission.PromoterGuarantorDetails
{
    [Authorize]
    public class WorkingCapitalDetailsController : BaseApiController
    {
        //private readonly ILogger<WorkingCapitalDetailsController> _logger;
        private readonly IWorkingCapitalDetails _workingCapitalDetails;
        private readonly ILogger _logger;
        public WorkingCapitalDetailsController(IWorkingCapitalDetails workingCapitalDetails, ILogger logger)
        {
            _workingCapitalDetails = workingCapitalDetails;
            _logger = logger;
        }

        [HttpPost, Route("AddWorkingCapitalDetails")]
        public async Task<IActionResult> AddWorkingCapitalDetailsAsync(ProjectWorkingCapitalDeatailsDTO WCapitalDto, CancellationToken token)
        {
            try
            {
                _logger.Information(string.Format("Started - AddWorkingCapitalDetailsAsync method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}", WCapitalDto.EnqWcId, WCapitalDto.EnqtempId, WCapitalDto.EnqWcIfsc, WCapitalDto.EnqWcBank, WCapitalDto.EnqWcBranch, WCapitalDto.EnqWcAmt, WCapitalDto.UniqueId, WCapitalDto.Operation));
                var workingCapital = await _workingCapitalDetails.AddWorkingCapitalDetails(WCapitalDto, token).ConfigureAwait(false);

                if (workingCapital == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information(string.Format("Completed - AddWorkingCapitalDetailsAsync method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}", WCapitalDto.EnqWcId, WCapitalDto.EnqtempId, WCapitalDto.EnqWcIfsc, WCapitalDto.EnqWcBank, WCapitalDto.EnqWcBranch, WCapitalDto.EnqWcAmt, WCapitalDto.UniqueId, WCapitalDto.Operation));
                return Ok(new ApiResultResponse(200, workingCapital, "working Capital details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddWorkingCapitalDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("UpdateWorkingCapitalDetails")]
        public async Task<IActionResult> UpdateWorkingCapitalDetailsAsync(ProjectWorkingCapitalDeatailsDTO WCapitalDto, CancellationToken token)
        {
            try
            {
                _logger.Information(string.Format("Started - UpdateWorkingCapitalDetailsAsync method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}", WCapitalDto.EnqWcId, WCapitalDto.EnqtempId, WCapitalDto.EnqWcIfsc, WCapitalDto.EnqWcBank, WCapitalDto.EnqWcBranch, WCapitalDto.EnqWcAmt, WCapitalDto.UniqueId, WCapitalDto.Operation));
                var workingCapital = await _workingCapitalDetails.UpdateWorkingCapitalDetails(WCapitalDto, token).ConfigureAwait(false);

                if (workingCapital == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information(string.Format("Completed - UpdateWorkingCapitalDetailsAsync method for EnqWcId:{0} EnqtempId:{1} EnqWcIfsc:{2} EnqWcBank:{3} EnqWcBranch:{4} EnqWcAmt:{5} UniqueId:{6} Operation:{7}", WCapitalDto.EnqWcId, WCapitalDto.EnqtempId, WCapitalDto.EnqWcIfsc, WCapitalDto.EnqWcBank, WCapitalDto.EnqWcBranch, WCapitalDto.EnqWcAmt, WCapitalDto.UniqueId, WCapitalDto.Operation));
                return Ok(new ApiResultResponse(200, workingCapital, "working Capital details Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateWorkingCapitalDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdWorkingCapitalDetails")]
        public async Task<IActionResult> GetByIdWorkingCapitalDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdWorkingCapitalDetailsAsync method for Id = " + Id);
                var workingCapital = await _workingCapitalDetails.GetByIdWorkingCapitalDetails(Id, token).ConfigureAwait(false);
                if (workingCapital == null)
                {
                    _logger.Information("Error - 404 working Capital Details Not found");
                    return new NotFoundObjectResult(new ApiException(404, "working Capital Details Not found."));
                }
                _logger.Information("Completed - GetByIdWorkingCapitalDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, workingCapital, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdWorkingCapitalDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeleteGuarantorDetails")]
        public async Task<IActionResult> DeleteGuarantorDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteGuarantorDetailsAsync method for Id = " + Id);
                bool isDeleted = await _workingCapitalDetails.DeleteWorkingCapitalDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 working Capital details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "working Capital details not exists!"));
                }
                _logger.Information("Completed - DeleteGuarantorDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, true, "working Capital Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteGuarantorDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
