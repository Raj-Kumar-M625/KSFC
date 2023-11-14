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
    public class PromoterAssetsNetWorthDetailsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        //private readonly ILogger<PromoterAssetsNetWorthDetailsController> _logger;
        private readonly IPromoterAssetsNetWorthDetails _promoterNetWorthDetails;
        private readonly ILogger _logger;
        public PromoterAssetsNetWorthDetailsController(UserInfo userInfo, IPromoterAssetsNetWorthDetails promoterNetWorthDetails,
                                        IMapper mapper, ILogger logger)
        {
            _userInfo = userInfo;
            _promoterNetWorthDetails = promoterNetWorthDetails;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost, Route("AddPromoterAssetsNetWorthDetails")]
        public async Task<IActionResult> AddPromoterAssetsNetWorthDetailsAsync(List<PromoterAssetsNetWorthDTO> assetsNetWorthDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddPromoterAssetsNetWorthDetailsAsync method with assetsNetWorthDto ");
                var promoter = await _promoterNetWorthDetails.AddPromoterAssetsNetWorthDetails(assetsNetWorthDto, token).ConfigureAwait(false);
                if (promoter == null)
                {
                    _logger.Information("Error - 400 Something went Wrong ");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddPromoterAssetsNetWorthDetailsAsync method with assetsNetWorthDto ");
                return Ok(new ApiResultResponse(200, promoter, "Promoter Assets Net Worth details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddPromoterAssetsNetWorthDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("UpdatePromoterAssetsNetWorthDetails")]
        public async Task<IActionResult> UpdatePromoterAssetsNetWorthDetailsAsync(List<PromoterAssetsNetWorthDTO> assetsNetWorthDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdatePromoterAssetsNetWorthDetailsAsync method with assetsNetWorthDto");
                var Promoter = await _promoterNetWorthDetails.UpdatePromoterAssetsNetWorthDetails(assetsNetWorthDto, token).ConfigureAwait(false);
                if (Promoter == null)
                {
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdatePromoterAssetsNetWorthDetailsAsync method with assetsNetWorthDto");
                return Ok(new ApiResultResponse(200, Promoter, "Promoter Assets Net Worth details Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdatePromoterAssetsNetWorthDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
         }

        [HttpGet, Route("GetByIdPromoterAssetsNetWorthDetails")]
        public async Task<IActionResult> GetByIdPromoterAssetsNetWorthDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdPromoterAssetsNetWorthDetailsAsync method for Id =" + Id);
                var netWorth = await _promoterNetWorthDetails.GetByIdPromoterAssetsNetWorthDetails(Id, token).ConfigureAwait(false);
                if (netWorth == null)
                {
                    _logger.Information("Error - 404 Promoter Assets Net Worth Details Not found");
                    return new NotFoundObjectResult(new ApiException(404, "Promoter Assets Net Worth Details Not found."));
                }
                _logger.Information("Completed - GetByIdPromoterAssetsNetWorthDetailsAsync method for Id =" + Id);
                return Ok(new ApiResultResponse(200, netWorth, "Success."));

            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdPromoterAssetsNetWorthDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeletePromoterAssetsNetWorthDetails")]
        public async Task<IActionResult> DeletePromoterAssetsNetWorthDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeletePromoterAssetsNetWorthDetailsAsync method for Id = " + Id);
                bool isDeleted = await _promoterNetWorthDetails.DeletePromoterAssetsNetWorthDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Promoter Assets Net Worth details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Promoter Assets Net Worth details not exists!"));
                }
                _logger.Information("Completed - DeletePromoterAssetsNetWorthDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, true, "Promoter Assets Net Worth Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeletePromoterAssetsNetWorthDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
