using AutoMapper;
using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.AssociateSisterConcernDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Customer.Controllers.EnquirySubmission.AssociateSisterConcernDetails
{
    [Authorize]
    public class SisterConcernDetailsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        //private readonly ILogger<SisterConcernDetailsController> _logger;
        private readonly ISisterConcernDetails _sisterConcernDetails;
        private readonly IEnquiryHomeService _enquiryHome;
        private readonly ILogger _logger;
        /// <summary>
        /// Sister Concern Details
        /// </summary>
        /// <param name="userInfo"></param>
        /// <param name="sisterConcernDetails"></param>
        /// <param name="mapper"></param>
        /// <param name="logger"></param>
        public SisterConcernDetailsController(UserInfo userInfo, ISisterConcernDetails sisterConcernDetails,
                                        IMapper mapper, IEnquiryHomeService enquiryHome, ILogger logger)
        {
            _userInfo = userInfo;
            _sisterConcernDetails = sisterConcernDetails;
            _mapper = mapper;
            _logger = logger;
            _enquiryHome = enquiryHome;
        }
        /// <summary>
        /// Add Sister Concern Details
        /// </summary>
        /// <param name="sisterConcernDto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("AddSisterConcernDetails")]
        public async Task<IActionResult> AddSisterConcernDetailsAsync(List<SisterConcernDetailsDTO> sisterConcernDto, CancellationToken token)
        {
            try
            {
                /*foreach (var item in sisterConcernDto)
                {
                    _logger.Information("Started - SisterConcernDetailsDTO method for EnqSisId:{0} EnqtempId:{1} EnqSisName:{2} EnqSisIfsc:{3} BfacilityCode:{4} EnqOutamt:{5} EnqDeftamt:{6} EnqOts:{7} EnqRelief:{8} bankName:{9} EnqSiscibil:{10} UniqueId:{11}",
                item.EnqSisId, item.EnqtempId, item.EnqSisName, item.EnqSisIfsc, item.BfacilityCode, item.EnqOutamt, item.EnqDeftamt, item.EnqOts, item.EnqRelief, item.bankName, item.EnqSiscibil, item.UniqueId);
                }*/
                _logger.Information("Started - SisterConcernDetailsDTO method with sisterConcernDto");
                var sister = await _sisterConcernDetails.AddSisterConcernDetails(sisterConcernDto, token).ConfigureAwait(false);
                if (sister == null)
                {
                    _logger.Information("Error - ApiResponse 400");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                /*foreach (var item in sisterConcernDto)
                {
                    _logger.Information(string.Format("Started - AddSisterConcernDetailsAsync method for EnqSisId:{0} EnqtempId:{1} EnqSisName:{2} EnqSisIfsc:{3} BfacilityCode:{4} EnqOutamt:{5} EnqDeftamt:{6} EnqOts:{7} EnqRelief:{8} bankName:{9} EnqSiscibil:{10} UniqueId:{11}",
                item.EnqSisId, item.EnqtempId, item.EnqSisName, item.EnqSisIfsc, item.BfacilityCode, item.EnqOutamt, item.EnqDeftamt, item.EnqOts, item.EnqRelief, item.bankName, item.EnqSiscibil, item.UniqueId));
                }*/
                _logger.Information("Completed - SisterConcernDetailsDTO method with sisterConcernDto");
                return Ok(new ApiResultResponse(200, sister, "Sister Concern details created Successfully"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured while AddSisterConcernDetailsAsync. Error message is:{ ex.Message},{Environment.NewLine},The stack trace is: {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Update Sister Concern Details
        /// </summary>
        /// <param name="sisterConcernDto"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("UpdateSisterConcernDetails")]
        public async Task<IActionResult> UpdateSisterConcernDetailsAsync(List<SisterConcernDetailsDTO> sisterConcernDto, CancellationToken token)
        {
            try
            {
                /*foreach (var item in sisterConcernDto)
                {
                    _logger.Information(string.Format("Started - UpdateSisterConcernDetailsAsync method for EnqSisId:{0} EnqtempId:{1} EnqSisName:{2} EnqSisIfsc:{3} BfacilityCode:{4} EnqOutamt:{5} EnqDeftamt:{6} EnqOts:{7} EnqRelief:{8} bankName:{9} EnqSiscibil:{10} UniqueId:{11}",
                item.EnqSisId, item.EnqtempId, item.EnqSisName, item.EnqSisIfsc, item.BfacilityCode, item.EnqOutamt, item.EnqDeftamt, item.EnqOts, item.EnqRelief, item.bankName, item.EnqSiscibil, item.UniqueId));
                }*/
                _logger.Information("Started - SisterConcernDetailsDTO method with sisterConcernDto");
                var sister = await _sisterConcernDetails.UpdateSisterConcernDetails(sisterConcernDto, token).ConfigureAwait(false);
                if (sister == null)
                {
                    _logger.Information("Error - ApiResponse 400");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                /*foreach (var item in sisterConcernDto)
                {
                    _logger.Information(string.Format("Completed - UpdateSisterConcernDetailsAsync method for EnqSisId:{0} EnqtempId:{1} EnqSisName:{2} EnqSisIfsc:{3} BfacilityCode:{4} EnqOutamt:{5} EnqDeftamt:{6} EnqOts:{7} EnqRelief:{8} bankName:{9} EnqSiscibil:{10} UniqueId:{11}",
                item.EnqSisId, item.EnqtempId, item.EnqSisName, item.EnqSisIfsc, item.BfacilityCode, item.EnqOutamt, item.EnqDeftamt, item.EnqOts, item.EnqRelief, item.bankName, item.EnqSiscibil, item.UniqueId));
                }*/
                _logger.Information("Started - SisterConcernDetailsDTO method with sisterConcernDto");
                return Ok(new ApiResultResponse(200, sister, "Sister Concern  details Updated Successfully"));
            }
            catch (Exception ex)
            {
                _logger.Error($"Error occured while UpdateSisterConcernDetailsAsync. Error message is:{ ex.Message},{Environment.NewLine},The stack trace is: {ex.StackTrace}");
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Get By Id Sister Concern Details
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, Route("GetByIdSisterConcernDetails")]
        public async Task<IActionResult> GetByIdSisterConcernDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdSisterConcernDetailsAsync Method Id =" + Id + "CancellationToken" + token);
                var sister = await _sisterConcernDetails.GetByIdSisterConcernDetails(Id, token).ConfigureAwait(false);
                if (sister == null)
                {
                    _logger.Information("Error - ApiException 400");
                    return new NotFoundObjectResult(new ApiException(404, "Sister Concern Details Not found."));
                }
                _logger.Information("Completed - GetByIdSisterConcernDetailsAsync Method Id =" + Id + "CancellationToken" + token);
                return Ok(new ApiResultResponse(200, sister, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdSisterConcernDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        /// <summary>
        /// Soft Delete Sister Concern Details
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost, Route("DeleteSisterConcernDetails")]
        public async Task<IActionResult> DeleteSisterConcernDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteSisterConcernDetailsAsync Method Id =" + Id + "CancellationToken" + token);
                bool isDeleted = await _sisterConcernDetails.DeleteSisterConcernDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - ApiException 400 Sister Concern details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Sister Concern details not exists!"));
                }
                _logger.Information("Completed - DeleteSisterConcernDetailsAsync Method Id =" + Id + "CancellationToken" + token);
                return Ok(new ApiResultResponse(200, true, "Sister Concern Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteSisterConcernDetailsAsync  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
        [HttpGet, Route("UpdateAssociateSisterConcernStatus")]
        public async Task<IActionResult> UpdateAssociateSisterConcernStatus(int enqId, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdateAssociateSisterConcernStatus Method enqId =" + enqId + "CancellationToken =" + token);
                enqId = await _enquiryHome.UpdateEnquiryAssociateSisterConcernStatus(enqId, true, token).ConfigureAwait(false);
                await _sisterConcernDetails.DeleteSisterConcernDetailsByEnqId(enqId, token).ConfigureAwait(false);
                _logger.Information("Completed - UpdateAssociateSisterConcernStatus Method enqId =" + enqId + "CancellationToken =" + token);
                return Ok(new ApiResultResponse(200, enqId, "Enquiry Details Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateAssociateSisterConcernStatus  page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
