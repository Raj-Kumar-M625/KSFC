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
    public class GuarantorLiabilityDetailsController : BaseApiController
    {
        //private readonly ILogger<GuarantorLiabilityDetailsController> _logger;
        private readonly IGuarantorLiabilityDetails _guarantorDetails;
        private readonly ILogger _logger;
        public GuarantorLiabilityDetailsController(IGuarantorLiabilityDetails guarantorDetails, ILogger logger)
        {

            _guarantorDetails = guarantorDetails;
            _logger = logger;
        }

        [HttpPost, Route("AddGuarantorLiabilityDetails")]
        public async Task<IActionResult> AddGuarantorLiabilityDetailsAsync(List<GuarantorLiabilityDetailsDTO> GuarantorDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddGuarantorLiabilityDetailsAsync method with GuarantorDto");
                var Guarantor = await _guarantorDetails.AddGuarantorLiabilityDetails(GuarantorDto, token).ConfigureAwait(false);

                if (Guarantor == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddGuarantorLiabilityDetailsAsync method with GuarantorDto");
                return Ok(new ApiResultResponse(200, Guarantor, "Guarantor Liability details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddGuarantorLiabilityDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("UpdateGuarantorLiabilityDetails")]
        public async Task<IActionResult> UpdateGuarantorLiabilityDetailsAsync(List<GuarantorLiabilityDetailsDTO> PromoterDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdateGuarantorLiabilityDetailsAsync method with PromoterDto");
                var Guarantor = await _guarantorDetails.UpdateGuarantorLiabilityDetails(PromoterDto, token).ConfigureAwait(false);

                if (Guarantor == null)
                {
                    _logger.Information("Error -400 Something went Wrong ");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdateGuarantorLiabilityDetailsAsync method with PromoterDto");
                return Ok(new ApiResultResponse(200, Guarantor, "Guarantor Liability details Updated Successfully"));

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateGuarantorLiabilityDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdGuarantorLiabilityDetails")]
        public async Task<IActionResult> GetByIdGuarantorLiabilityDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdGuarantorLiabilityDetailsAsync method for Id = " + Id);
                var Guarantor = await _guarantorDetails.GetByIdGuarantorLiabilityDetails(Id, token).ConfigureAwait(false);
                if (Guarantor == null)
                {
                    _logger.Information("Error - 404 Promoter Details Not found ");
                    return new NotFoundObjectResult(new ApiException(404, "Promoter Details Not found."));
                }
                _logger.Information("Completed - GetByIdGuarantorLiabilityDetailsAsync method for Id =" + Id);
                return Ok(new ApiResultResponse(200, Guarantor, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdGuarantorLiabilityDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeleteGuarantorLiabilityDetails")]
        public async Task<IActionResult> DeleteGuarantorLiabilityDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteGuarantorLiabilityDetailsAsync method for Id = " + Id);
                bool isDeleted = await _guarantorDetails.DeleteGuarantorLiabilityDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Promoter details not exists! ");
                    return new NotFoundObjectResult(new ApiException(404, "Promoter details not exists!"));
                }
                _logger.Information("Completed - DeleteGuarantorLiabilityDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, true, "Promoter Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteGuarantorLiabilityDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
