using AutoMapper;
using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.EnquirySubmission.SecurityDocumentDetails;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails;
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

namespace KAR.KSFC.API.Areas.Customer.Controllers.EnquirySubmission.SecurityDocumentDetails
{
    [KSFCAuthorization]
    public class SecurityDetailsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        //private readonly ILogger<SecurityDetailsController> _logger;
        private readonly ISecurityDetails _securityDetails;
        private readonly ILogger _logger;
        public SecurityDetailsController(UserInfo userInfo, ISecurityDetails securityDetails,
                                        IMapper mapper, ILogger logger)
        {
            _userInfo = userInfo;
            _securityDetails = securityDetails;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpPost, Route("AddSecurityDetails")]
        public async Task<IActionResult> AddSecurityDetailsAsync([FromBody] List<SecurityDetailsDTO> securityDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddSecurityDetailsAsync method with securityDto");
                var security = await _securityDetails.AddSecurityDetails(securityDto, token).ConfigureAwait(false);
                if (security == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddSecurityDetailsAsync method with securityDto");
                return Ok(new ApiResultResponse(200, security, "Security Details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddSecurityDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
        [HttpGet, Route("GetByIdSecurityDetails")]
        public async Task<IActionResult> GetByIdSecurityDetailsAsync(int id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdSecurityDetailsAsync method for id = " + id);
                var security = await _securityDetails.GetByIdSecurityDetails(id, token).ConfigureAwait(false);
                if (security == null)
                {
                    _logger.Information("Error - 404 Address Details Not found");
                    return new NotFoundObjectResult(new ApiException(404, "Address Details Not found."));
                }
                _logger.Information("Completed - GetByIdSecurityDetailsAsync method for id = " + id);
                return Ok(new ApiResultResponse(200, security, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdSecurityDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
        [HttpPost, Route("DeleteSecurityDetails")]
        public async Task<IActionResult> DeleteSecurityDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteSecurityDetailsAsync method for Id = " + Id);
                bool isDeleted = await _securityDetails.DeleteSecurityDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Address details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Address details not exists!"));
                }
                _logger.Information("Completed - DeleteSecurityDetailsAsync method for id = " + Id);
                return Ok(new ApiResultResponse(200, true, "Security Details Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteSecurityDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("UpdateSecurityDetails")]
        public async Task<IActionResult> UpdateSecurityDetailsAsync(List<SecurityDetailsDTO> securityDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdateSecurityDetailsAsync method with securityDto");
                var security = await _securityDetails.UpdateSecurityDetails(securityDto, token);
                if (security == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdateSecurityDetailsAsync method with securityDto");
                return Ok(new ApiResultResponse(200, security, "security details Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateSecurityDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
