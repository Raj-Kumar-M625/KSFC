using AutoMapper;
using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.UnitDetails;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Customer.Controllers.EnquirySubmission.UnitDetails
{
    [Authorize]
    public class RegistrationDetailsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        //private readonly ILogger<RegistrationDetailsController> _logger;
        private readonly IRegistrationDetails _registrationDetails;
        private readonly ILogger _logger;
        public RegistrationDetailsController(UserInfo userInfo, IRegistrationDetails registrationDetails,
                                         IMapper mapper, ILogger logger)
        {
            _userInfo = userInfo;
            _registrationDetails = registrationDetails;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost, Route("AddRegistrationDetails")]
        public async Task<IActionResult> AddRegistrationDetailsAsync(List<RegistrationNoDetailsDTO> registrationDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddRegistrationDetailsAsync method with registrationDto");
                var registration = await _registrationDetails.AddRegistrationDetails(registrationDto, token);
                if (registration == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddRegistrationDetailsAsync method with registrationDto");
                return Ok(new ApiResultResponse(200, registration, "Registration details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddRegistrationDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        
        }

        [HttpPost, Route("UpdateRegistrationDetails")]
        public async Task<IActionResult> UpdateRegistrationDetailsAsync(List<RegistrationNoDetailsDTO> registrationDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdateRegistrationDetailsAsync method with registrationDto");
                var Registration = await _registrationDetails.UpdateRegistrationDetails(registrationDto, token);

                if (Registration == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdateRegistrationDetailsAsync method with registrationDto");
                return Ok(new ApiResultResponse(200, Registration, "Registration details Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateRegistrationDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdRegistrationDetails")]
        public async Task<IActionResult> GetByIdRegistrationDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdRegistrationDetailsAsync method for Id = " + Id);
                var RegistrationDetails = await _registrationDetails.GetRegistrationNoDetailsById(Id, token);
                if (RegistrationDetails == null)
                {
                    _logger.Information("Error - 404 Registration Details Not Found");
                    return new NotFoundObjectResult(new ApiException(404, "Registration Details Not Found."));
                }
                _logger.Information("Completed - GetByIdRegistrationDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, RegistrationDetails, "Success"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdRegistrationDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeleteRegistrationDetails")]
        public async Task<IActionResult> DeleteRegistrationDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteRegistrationDetailsAsync method for Id = " + Id);
                bool isDeleted = await _registrationDetails.DeleteRegistrationDetails(Id, token);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Registration details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Registration details not exists!"));
                }
                _logger.Information("Completed - DeleteRegistrationDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, true, "Registration Details Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteRegistrationDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
