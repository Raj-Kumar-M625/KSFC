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
    public class PromoterDetailsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        //private readonly ILogger<PromoterDetailsController> _logger;
        private readonly IPromoterDetails _promoterDetails;
        private readonly ILogger _logger;
        public PromoterDetailsController(UserInfo userInfo, IPromoterDetails promoterDetails,
                                        IMapper mapper, ILogger logger)
        {
            _userInfo = userInfo;
            _promoterDetails = promoterDetails;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost, Route("AddPromoterDetails")]
        public async Task<IActionResult> AddPromoterDetailsAsync(List<PromoterDetailsDTO> PromoterDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddPromoterDetailsAsync method with PromoterDto");
                var promoter = await _promoterDetails.AddPromoterDetails(PromoterDto, token).ConfigureAwait(false);

                if (promoter == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddPromoterDetailsAsync method with PromoterDto");
                return Ok(new ApiResultResponse(200, promoter, "Promoter details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddPromoterDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("UpdatePromoterDetails")]
        public async Task<IActionResult> UpdatePromoterDetailsAsync(List<PromoterDetailsDTO> PromoterDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdatePromoterDetailsAsync method with PromoterDto");
                var Promoter = await _promoterDetails.UpdatePromoterDetails(PromoterDto, token).ConfigureAwait(false);

                if (Promoter == null)
                {
                    _logger.Information("Error - 400 Something went Wrong ");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdatePromoterDetailsAsync method with PromoterDto");
                return Ok(new ApiResultResponse(200, Promoter, "Promoter details Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdatePromoterDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdPromoterDetails")]
        public async Task<IActionResult> GetByIdPromoterDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdPromoterDetailsAsync method for Id = " + Id);
                var Promoter = await _promoterDetails.GetByIdPromoterDetails(Id, token).ConfigureAwait(false);
                if (Promoter == null)
                {
                    _logger.Information("Error - 404 Promoter Details Not found");
                    return new NotFoundObjectResult(new ApiException(404, "Promoter Details Not found."));
                }
                _logger.Information("Completed - GetByIdPromoterDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, Promoter, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdPromoterDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeletePromoterDetails")]
        public async Task<IActionResult> DeletePromoterDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeletePromoterDetailsAsync method for Id = " + Id);
                bool isDeleted = await _promoterDetails.DeletePromoterDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Promoter details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Promoter details not exists!"));
                }
                _logger.Information("Completed - DeletePromoterDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, true, "Promoter Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeletePromoterDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
        
    }
}
