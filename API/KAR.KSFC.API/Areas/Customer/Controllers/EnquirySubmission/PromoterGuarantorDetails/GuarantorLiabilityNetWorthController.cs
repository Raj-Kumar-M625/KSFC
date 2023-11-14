using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
//using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Customer.Controllers.EnquirySubmission.PromoterGuarantorDetails
{
    [Authorize]
    public class GuarantorLiabilityNetWorthController : BaseApiController
    {
        //private readonly ILogger<GuarantorLiabilityNetWorthController> _logger;
        private readonly IGuarantorLiabilityNetWorth _guarantorNetWorth;
        private readonly ILogger _logger;
        public GuarantorLiabilityNetWorthController(IGuarantorLiabilityNetWorth guarantorNetWorth, ILogger logger)
                                          
        {
            _guarantorNetWorth = guarantorNetWorth;
            _logger = logger;
        }

        [HttpPost, Route("AddGuarantorLiabilityNetWorth")]
        public async Task<IActionResult> AddGuarantorLiabilityNetWorthAsync(List<GuarantorNetWorthDetailsDTO> GuarantorDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddGuarantorLiabilityNetWorthAsync method with GuarantorDto");
                var Guarantor = await _guarantorNetWorth.AddGuarantorNetWorthDetails(GuarantorDto, token).ConfigureAwait(false);

                if (Guarantor == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddGuarantorLiabilityNetWorthAsync method with GuarantorDto");
                return Ok(new ApiResultResponse(200, Guarantor, "Guarantor Liability Net Worth created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddGuarantorLiabilityNetWorthAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("UpdateGuarantorLiabilityNetWorth")]
        public async Task<IActionResult> UpdateGuarantorLiabilityNetWorthAsync(List<GuarantorNetWorthDetailsDTO> GuarantorDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdateGuarantorLiabilityNetWorthAsync method with GuarantorDto ");
                var Guarantor = await _guarantorNetWorth.UpdateGuarantorNetWorthDetails(GuarantorDto, token).ConfigureAwait(false);

                if (Guarantor == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdateGuarantorLiabilityNetWorthAsync method with GuarantorDto ");
                return Ok(new ApiResultResponse(200, Guarantor, "Guarantor Liability Net Worth Updated Successfully"));

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateGuarantorLiabilityNetWorthAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdGuarantorLiabilityNetWorth")]
        public async Task<IActionResult> GetByIdGuarantorLiabilityNetWorthAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdGuarantorLiabilityNetWorthAsync method for Id = " + Id);
                var Guarantor = await _guarantorNetWorth.GetByIdGuarantorNetWorthDetails(Id, token).ConfigureAwait(false);
                if (Guarantor == null)
                {
                    _logger.Information("Error - 404 Guarantor Liability Net Worth  Not found");
                    return new NotFoundObjectResult(new ApiException(404, "Guarantor Liability Net Worth  Not found."));
                }
                _logger.Information("Completed - GetByIdGuarantorLiabilityNetWorthAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, Guarantor, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdGuarantorLiabilityNetWorthAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeleteGuarantorLiabilityNetWorth")]
        public async Task<IActionResult> DeleteGuarantorLiabilityNetWorthAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteGuarantorLiabilityNetWorthAsync method for Id = " + Id);
                bool isDeleted = await _guarantorNetWorth.DeleteGuarantorNetWorthDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Guarantor Liability Net Worth not exists! ");
                    return new NotFoundObjectResult(new ApiException(404, "Guarantor Liability Net Worth not exists!"));
                }
                _logger.Information("Completed - DeleteGuarantorLiabilityNetWorthAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, true, "Guarantor Liability Net Worth Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteGuarantorLiabilityNetWorthAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
