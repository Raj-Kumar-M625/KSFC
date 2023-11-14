using AutoMapper;
using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using KAR.KSFC.Components.Data.Models.DbModels;
using KAR.KSFC.Components.Data.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Customer.Controllers.EnquirySubmission.AssociateSisterConcernDetails
{
    [Authorize]
    public class SisterConcernFinancialDetailsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        //private readonly ILogger<SisterConcernFinancialDetailsController> _logger;
        private readonly ISisterConcernFinancialDetails _sisterConcernDetails;
        private readonly ILogger _logger;

        /// <summary>
        /// Sister Concern Financial Details 
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="sisterConcernDetails"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public SisterConcernFinancialDetailsController(UserInfo userInfo, ISisterConcernFinancialDetails sisterConcernDetails,
                                        IMapper mapper, ILogger logger)
        {
            _userInfo = userInfo;
            _sisterConcernDetails = sisterConcernDetails;
            _mapper = mapper;
            _logger = logger;
          

        }
        /// <summary>
        /// Add Sister Concern Financial Details 
        /// </summary>
        /// <param name="sisterConcernDto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("AddSisterConcernFinancialDetails")]
        public async Task<IActionResult> AddSisterConcernFinancialDetailsAsync(List<SisterConcernFinancialDetailsDTO> sisterConcernDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - SisterConcernFinancialDetailsDTO method with sisterConcernDto ");
                var sister = await _sisterConcernDetails.AddSisterConcernFinancialDetails(sisterConcernDto, token).ConfigureAwait(false);
                if (sisterConcernDto == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - SisterConcernFinancialDetailsDTO method with sisterConcernDto ");
                return Ok(new ApiResultResponse(200, sister, "Sister Concern Financial details created Successfully"));
             }
            catch (Exception ex)
            {
                _logger.Error($"Error occured while AddSisterConcernFinancialDetailsAsync. Error message is:{ ex.Message},{Environment.NewLine},The stack trace is: {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Update Sister Concern Financial Details 
        /// </summary>
        /// <param name="sisterConcernDto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("UpdateSisterConcernFinancialDetails")]
        public async Task<IActionResult> UpdateSisterConcernFinancialDetailsAsync(List<SisterConcernFinancialDetailsDTO> sisterConcernDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - SisterConcernFinancialDetailsDTO method method with " + sisterConcernDto);
                var sister = await _sisterConcernDetails.UpdateSisterConcernFinancialDetails(sisterConcernDto, token).ConfigureAwait(false);
                if (sister == null)
                {
                    _logger.Information("Error - 400 Something Went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - SisterConcernFinancialDetailsDTO method method with " + sisterConcernDto);
                return Ok(new ApiResultResponse(200, sister, "Sister Concern Financial details Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateSisterConcernFinancialDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
         }

        /// <summary>
        /// Get Sister Concern Financial Details 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, Route("GetByIdSisterConcernFinancialDetails")]
        public async Task<IActionResult> GetByIdSisterConcernFinancialDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdSisterConcernFinancialDetailsAsync method for Id = " + Id + "CancellationToken" + token);
                var sister = await _sisterConcernDetails.GetByIdSisterConcernFinancialDetails(Id, token).ConfigureAwait(false);
                if (sister == null)
                {
                    _logger.Information("Error - 400 Sister Concern Financial Details Not found.");
                    return new NotFoundObjectResult(new ApiException(404, "Sister Concern Financial Details Not found."));
                }
                _logger.Information("Completed - GetByIdSisterConcernFinancialDetailsAsync method for Id = " + Id + "CancellationToken" + token);
                return Ok(new ApiResultResponse(200, sister, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdSisterConcernFinancialDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Soft Delete Sister Concern Financial Details 
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpDelete, Route("DeleteSisterConcernFinancialDetails")]
        public async Task<IActionResult> DeleteSisterConcernFinancialDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteSisterConcernFinancialDetailsAsync method for Id = " + Id + "CancellationToken" + token);
                bool isDeleted = await _sisterConcernDetails.DeleteSisterConcernFinancialDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Sister Concern details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Sister Concern details not exists!"));
                }
                _logger.Information("Completed - DeleteSisterConcernFinancialDetailsAsync method for Id = " + Id + "CancellationToken" + token);
                return Ok(new ApiResultResponse(200, true, "Sister Concern Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteSisterConcernFinancialDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
