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
    public class GuarantorDetailsController : BaseApiController
    {
        private readonly IGuarantorDetails _guarantorDetails;
        private readonly ILogger _logger;
        public GuarantorDetailsController(ILogger logger, IGuarantorDetails guarantorDetails = null)
        {
            _guarantorDetails = guarantorDetails;
            _logger = logger;
        }

        [HttpPost, Route("AddGuarantorDetails")]
        public async Task<IActionResult> AddGuarantorDetailsAsync(List<GuarantorDetailsDTO> GuarantorDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddGuarantorDetailsAsync method with GuarantorDto");
                var guarantor = await _guarantorDetails.AddGuarantorDetails(GuarantorDto, token).ConfigureAwait(false);

                if (guarantor == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddGuarantorDetailsAsync method with GuarantorDto");
                return Ok(new ApiResultResponse(200, guarantor, "Guarantor details created Successfully"));

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddGuarantorDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("UpdateGuarantorDetails")]
        public async Task<IActionResult> UpdateGuarantorDetailsAsync(List<GuarantorDetailsDTO> PromoterDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdateGuarantorDetailsAsync method with PromoterDto");
                var Promoter = await _guarantorDetails.UpdateGuarantorDetails(PromoterDto, token).ConfigureAwait(false);

                if (Promoter == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdateGuarantorDetailsAsync method with PromoterDto");
                return Ok(new ApiResultResponse(200, Promoter, "Guarantor details Updated Successfully"));

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateGuarantorDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdGuarantorDetails")]
        public async Task<IActionResult> GetByIdGuarantorDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdGuarantorDetailsAsync method for Id = " + Id);
                var Promoter = await _guarantorDetails.GetByIdGuarantorDetails(Id, token).ConfigureAwait(false);
                if (Promoter == null)
                {
                    _logger.Information("Error - 404 Guarantor Details Not found");
                    return new NotFoundObjectResult(new ApiException(404, "Guarantor Details Not found."));
                }
                _logger.Information("Completed - GetByIdGuarantorDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, Promoter, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdGuarantorDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeleteGuarantorDetails")]
        public async Task<IActionResult> DeleteGuarantorDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteGuarantorDetailsAsync method for Id = " + Id);
                bool isDeleted = await _guarantorDetails.DeleteGuarantorDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Guarantor details not exists! ");
                    return new NotFoundObjectResult(new ApiException(404, "Guarantor details not exists!"));
                }
                _logger.Information("Started - DeleteGuarantorDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, true, "Guarantor Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteGuarantorDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
        
    }
}
