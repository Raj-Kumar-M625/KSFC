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
    public class PromoterLiabilityNetWorthController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        //private readonly ILogger<PromoterLiabilityNetWorthController> _logger;
        private readonly IPromoterLiabilityNetWorth _promoterLiabilityNetWorth;
        private readonly ILogger _logger;
        public PromoterLiabilityNetWorthController(UserInfo userInfo, IPromoterLiabilityNetWorth promoterLiabilityNetWorth,
                                        IMapper mapper, ILogger logger)
        {
            _userInfo = userInfo;
            _promoterLiabilityNetWorth = promoterLiabilityNetWorth;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost, Route("AddPromoterLiabilityNetWorth")]
        public async Task<IActionResult> AddPromoterLiabilityNetWorthAsync(List<PromoterNetWorthDetailsDTO> networthDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddPromoterLiabilityNetWorthAsync method with networthDto ");
                var promoter = await _promoterLiabilityNetWorth.AddPromoterLiabilityNetWorthDetails(networthDto, token).ConfigureAwait(false);
                if (promoter == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddPromoterLiabilityNetWorthAsync method with networthDto ");
                return Ok(new ApiResultResponse(200, promoter, "Promoter Liability Net Worth details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddPromoterLiabilityNetWorthAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("UpdatePromoterLiabilityNetWorth")]
        public async Task<IActionResult> UpdatePromoterLiabilityNetWorthAsync(List<PromoterNetWorthDetailsDTO> networthDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdatePromoterLiabilityNetWorthAsync method with networthDto");
                var Promoter = await _promoterLiabilityNetWorth.UpdatePromoterLiabilityNetWorthDetails(networthDto, token).ConfigureAwait(false);
                if (Promoter == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdatePromoterLiabilityNetWorthAsync method with networthDto");
                return Ok(new ApiResultResponse(200, Promoter, "Promoter Liability Net Worth details Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdatePromoterLiabilityNetWorthAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdPromoterLiabilityNetWorth")]
        public async Task<IActionResult> GetByIdPromoterLiabilityNetWorthAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdPromoterLiabilityNetWorthAsync method for Id = " + Id);
                var Promoter = await _promoterLiabilityNetWorth.GetByIdPromoterLiabilityNetWorthDetails(Id, token).ConfigureAwait(false);
                if (Promoter == null)
                {
                    _logger.Information("Error - 404 Promoter Liability Net Worth Details Not found");
                    return new NotFoundObjectResult(new ApiException(404, "Promoter Liability Net Worth Details Not found."));
                }
                _logger.Information("Completed - GetByIdPromoterLiabilityNetWorthAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, Promoter, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdPromoterLiabilityNetWorthAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeletePromoterLiabilityNetWorth")]
        public async Task<IActionResult> DeletePromoterLiabilityNetWorthAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeletePromoterLiabilityNetWorthAsync method for Id = " + Id);
                bool isDeleted = await _promoterLiabilityNetWorth.DeletePromoterLiabilityNetWorthDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Promoter Liability Net Worth details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Promoter Liability Net Worth details not exists!"));
                }
                _logger.Information("Completed - DeletePromoterLiabilityNetWorthAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, true, "Promoter Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeletePromoterLiabilityNetWorthAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
