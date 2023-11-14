using AutoMapper;
using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.PromoterGuarantorDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities;
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
    public class GuarantorAssetsNetWorthDetailsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        //private readonly ILogger<GuarantorAssetsNetWorthDetailsController> _logger;
        private readonly IGuarantorAssetsNetWorthDetails _guarAssetsNetWorthDetails;
        private readonly ILogger _logger;
        public GuarantorAssetsNetWorthDetailsController(UserInfo userInfo, IGuarantorAssetsNetWorthDetails guarAssetsNetWorthDetails,
                                        IMapper mapper, ILogger logger)
        {
            _userInfo = userInfo;
            _guarAssetsNetWorthDetails = guarAssetsNetWorthDetails;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost, Route("AddGuarantorAssetsNetWorthDetails")]
        public async Task<IActionResult> AddGuarantorAssetsNetWorthDetailsAsync(List<GuarantorAssetsNetWorthDTO> assetsNetWorthDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddGuarantorAssetsNetWorthDetailsAsync method with assetsNetWorthDto ");
                var guarantor = await _guarAssetsNetWorthDetails.AddGuarantorAssetsNetWorthDetails(assetsNetWorthDto, token).ConfigureAwait(false);
                if (guarantor == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddGuarantorAssetsNetWorthDetailsAsync method with assetsNetWorthDto ");
                return Ok(new ApiResultResponse(200, guarantor, "Guarantor Assets Net Worth details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddGuarantorAssetsNetWorthDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("UpdateGuarantorAssetsNetWorthDetails")]
        public async Task<IActionResult> UpdateGuarantorAssetsNetWorthDetailsAsync(List<GuarantorAssetsNetWorthDTO> assetsNetWorthDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdateGuarantorAssetsNetWorthDetailsAsync method with assetsNetWorthDto");
                var Promoter = await _guarAssetsNetWorthDetails.UpdateGuarantorAssetsNetWorthDetails(assetsNetWorthDto, token).ConfigureAwait(false);
                if (Promoter == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdateGuarantorAssetsNetWorthDetailsAsync method with assetsNetWorthDto");
                return Ok(new ApiResultResponse(200, Promoter, "Guarantor Assets Net Worth details Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateGuarantorAssetsNetWorthDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdGuarantorAssetsNetWorthDetails")]
        public async Task<IActionResult> GetByIdGuarantorAssetsNetWorthDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdGuarantorAssetsNetWorthDetailsAsync method for Id = " + Id);
                var netWorth = await _guarAssetsNetWorthDetails.GetByIdGuarantorAssetsNetWorthDetails(Id, token).ConfigureAwait(false);
                if (netWorth == null)
                {
                    _logger.Information("Error - 404 Guarantor Assets Net Worth Details Not found");
                    return new NotFoundObjectResult(new ApiException(404, "Guarantor Assets Net Worth Details Not found."));
                }
                _logger.Information("Started - GetByIdGuarantorAssetsNetWorthDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, netWorth, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdGuarantorAssetsNetWorthDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeleteGuarantorAssetsNetWorthDetails")]
        public async Task<IActionResult> DeleteGuarantorAssetsNetWorthDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteGuarantorAssetsNetWorthDetailsAsync method for Id = " + Id);
                bool isDeleted = await _guarAssetsNetWorthDetails.DeleteGuarantorAssetsNetWorthDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Promoter Assets Net Worth details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Promoter Assets Net Worth details not exists!"));
                }
                _logger.Information("Started - DeleteGuarantorAssetsNetWorthDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, true, "Promoter Assets Net Worth Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteGuarantorAssetsNetWorthDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
