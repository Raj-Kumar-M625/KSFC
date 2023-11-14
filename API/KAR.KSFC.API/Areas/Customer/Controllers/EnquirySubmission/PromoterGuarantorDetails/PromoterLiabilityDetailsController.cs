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
    public class PromoterLiabilityDetailsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        //private readonly ILogger<PromoterLiabilityDetailsController> _logger;
        private readonly IPromoterLiabilityDetails _promoterLiabilityDetails;
        private readonly ILogger _logger;
        public PromoterLiabilityDetailsController(UserInfo userInfo, IPromoterLiabilityDetails promoterLiabilityDetails,
                                        IMapper mapper, ILogger logger)
        {
            _userInfo = userInfo;
            _promoterLiabilityDetails = promoterLiabilityDetails;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost, Route("AddPromoterLiabilityDetails")]
        public async Task<IActionResult> AddPromoterLiabilityDetailsAsync(List<PromoterLiabilityDetailsDTO> liabilityDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddPromoterLiabilityDetailsAsync method with liabilityDto");
                var promoter = await _promoterLiabilityDetails.AddPromoterLiabilityDetails(liabilityDto, token).ConfigureAwait(false);
                if (promoter == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddPromoterLiabilityDetailsAsync method with liabilityDto");
                return Ok(new ApiResultResponse(200, promoter, "Promoter details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddPromoterLiabilityDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("UpdatePromoterLiabilityDetails")]
        public async Task<IActionResult> UpdatePromoterLiabilityDetailsAsync(List<PromoterLiabilityDetailsDTO> liabilityDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdatePromoterLiabilityDetailsAsync method with liabilityDto");
                var Promoter = await _promoterLiabilityDetails.UpdatePromoterLiabilityDetails(liabilityDto, token).ConfigureAwait(false);
                if (Promoter == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdatePromoterLiabilityDetailsAsync method with liabilityDto");
                return Ok(new ApiResultResponse(200, Promoter, "Promoter details Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdatePromoterLiabilityDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdPromoterLiabilityDetails")]
        public async Task<IActionResult> GetByIdPromoterLiabilityDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdPromoterLiabilityDetailsAsync method for Id = " + Id);
                var Promoter = await _promoterLiabilityDetails.GetByIdPromoterLiabilityDetails(Id, token).ConfigureAwait(false);
                if (Promoter == null)
                {
                    return new NotFoundObjectResult(new ApiException(404, "Promoter Details Not found."));
                }
                return Ok(new ApiResultResponse(200, Promoter, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdPromoterLiabilityDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeletePromoterLiabilityDetails")]
        public async Task<IActionResult> DeletePromoterLiabilityDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeletePromoterLiabilityDetailsAsync method for Id = " + Id);
                bool isDeleted = await _promoterLiabilityDetails.DeletePromoterLiabilityDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Promoter details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Promoter details not exists!"));
                }
                _logger.Information("Completed - DeletePromoterLiabilityDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, true, "Promoter Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeletePromoterLiabilityDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
