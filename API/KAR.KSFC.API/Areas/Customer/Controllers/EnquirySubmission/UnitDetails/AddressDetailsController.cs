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
using System;
//using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Customer.Controllers.EnquirySubmission.UnitDetails
{
    [Authorize]
    public class AddressDetailsController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly UserInfo _userInfo;
        //private readonly ILogger<AddressDetailsController> _logger;
        private readonly IAddressDetails _addressDetails;
        private readonly ILogger _logger;
        public AddressDetailsController(UserInfo userInfo, IAddressDetails addressDetails,
                                        IMapper mapper, ILogger logger)
        {
            _userInfo = userInfo;
            _addressDetails = addressDetails;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpPost, Route("AddAddressDetails")]
        public async Task<IActionResult> AddAddressDetailsAsync([FromBody] List<AddressDetailsDTO> addressDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - AddAddressDetailsAsync method with addressDto");
                var address = await _addressDetails.AddAddressDetails(addressDto, token).ConfigureAwait(false);

                if (address == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - AddAddressDetailsAsync method with addressDto");
                return Ok(new ApiResultResponse(200, address, "Address details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddAddressDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("UpdateAddressDetails")]
        public async Task<IActionResult> UpdateAddressDetailsAsync(List<AddressDetailsDTO> addressDto, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - UpdateAddressDetailsAsync method with addressDto");
                var address = await _addressDetails.UpdateAddressDetails(addressDto, token).ConfigureAwait(false); ;
                if (address == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information("Completed - UpdateAddressDetailsAsync method with addressDto");
                return Ok(new ApiResultResponse(200, address, "Address details Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateAddressDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdAddressDetails")]
        public async Task<IActionResult> GetByIdAddressDetailsAsync(int id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdAddressDetailsAsync method for Id = " + id);
                var address = await _addressDetails.GetByIdAddressDetails(id, token).ConfigureAwait(false);
                if (address == null)
                {
                    _logger.Information("Error - 404 Address Details Not found");
                    return new NotFoundObjectResult(new ApiException(404, "Address Details Not found."));
                }
                _logger.Information("Completed - GetByIdAddressDetailsAsync method for Id = " + id);
                return Ok(new ApiResultResponse(200, address, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdAddressDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetAddressDetailsEnquiryId")]
        public async Task<IActionResult> GetAddressDetailsByEquiryAsync(int enquiryId, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetAddressDetailsByEquiryAsync method for enquiryId =" + enquiryId);
                var address = await _addressDetails.GetByIdAddressDetails(enquiryId, token).ConfigureAwait(false);
                if (address == null)
                {
                    _logger.Information("Error - 404 Address Details Not found");
                    return new NotFoundObjectResult(new ApiException(404, "Address Details Not found."));
                }
                _logger.Information("Completed - GetAddressDetailsByEquiryAsync method for enquiryId =" + enquiryId);
                return Ok(new ApiResultResponse(200, address, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetAddressDetailsByEquiryAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeleteAddressDetails")]
        public async Task<IActionResult> DeleteAddressDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteAddressDetailsAsync method for Id = " + Id);
                bool isDeleted = await _addressDetails.DeleteAddressDetails(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Address details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Address details not exists!"));
                }
                _logger.Information("Completed - DeleteAddressDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, true, "Address Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteAddressDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
