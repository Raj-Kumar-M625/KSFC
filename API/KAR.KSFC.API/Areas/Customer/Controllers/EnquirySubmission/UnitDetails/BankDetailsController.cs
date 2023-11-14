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
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Customer.Controllers.EnquirySubmission.UnitDetails
{
    [Authorize]
    public class BankDetailsController : BaseApiController
    {
         
        private readonly UserInfo _userInfo;
        //private readonly ILogger<BankDetailsController> _logger;
        private readonly IBankDetails _bankDetails;
        private readonly ILogger _logger;
        public BankDetailsController(UserInfo userInfo, IBankDetails bankDetails,
                                       ILogger logger)
        {
            _userInfo = userInfo;
            _bankDetails = bankDetails;
            _logger = logger;
        }

        [HttpPost, Route("AddBankDetails")]
        public async Task<IActionResult> AddBankDetailsAsync(BankDetailsDTO bankDto, CancellationToken token)
        {
            try
            {
                _logger.Information(String.Format("Started - AddBankDetailsAsync method for EnqBankId:{0} EnqtempId:{1} EnqAcctype:{2} EnqBankaccno:{3} EnqIfsc:{4} EnqAccName:{5} EnqBankname:{6} EnqBankbr:{7} UniqueId:{8} BankPinCode:{9}",
                    bankDto.EnqBankId, bankDto.EnqtempId, bankDto.EnqAcctype, bankDto.EnqBankaccno, bankDto.EnqIfsc, bankDto.EnqAccName, bankDto.EnqBankname, bankDto.EnqBankbr, bankDto.UniqueId, bankDto.BankPinCode));
                var Bank = await _bankDetails.AddBankDetailsAsync(bankDto, token).ConfigureAwait(false);
                if (Bank == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information(String.Format("Completed - AddBankDetailsAsync method for EnqBankId:{0} EnqtempId:{1} EnqAcctype:{2} EnqBankaccno:{3} EnqIfsc:{4} EnqAccName:{5} EnqBankname:{6} EnqBankbr:{7} UniqueId:{8} BankPinCode:{9}",
                    bankDto.EnqBankId, bankDto.EnqtempId, bankDto.EnqAcctype, bankDto.EnqBankaccno, bankDto.EnqIfsc, bankDto.EnqAccName, bankDto.EnqBankname, bankDto.EnqBankbr, bankDto.UniqueId, bankDto.BankPinCode));
                return Ok(new ApiResultResponse(200, Bank, "Bank details created Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading AddBankDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("UpdateBankDetails")]
        public async Task<IActionResult> UpdateBankDetailsAsync(BankDetailsDTO BankDto, CancellationToken token)
        {
            try
            {
                _logger.Information(String.Format("Started - UpdateBankDetailsAsync method for EnqBankId:{0} EnqtempId:{1} EnqAcctype:{2} EnqBankaccno:{3} EnqIfsc:{4} EnqAccName:{5} EnqBankname:{6} EnqBankbr:{7} UniqueId:{8} BankPinCode:{9}",
                    BankDto.EnqBankId, BankDto.EnqtempId, BankDto.EnqAcctype, BankDto.EnqBankaccno, BankDto.EnqIfsc, BankDto.EnqAccName, BankDto.EnqBankname, BankDto.EnqBankbr, BankDto.UniqueId, BankDto.BankPinCode));
                var Bank = await _bankDetails.UpdateBankDetailsAsync(BankDto, token).ConfigureAwait(false);
                if (Bank == null)
                {
                    _logger.Information("Error - 400 Something went Wrong");
                    return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
                }
                _logger.Information(String.Format("Completed - UpdateBankDetailsAsync method for EnqBankId:{0} EnqtempId:{1} EnqAcctype:{2} EnqBankaccno:{3} EnqIfsc:{4} EnqAccName:{5} EnqBankname:{6} EnqBankbr:{7} UniqueId:{8} BankPinCode:{9}",
                    BankDto.EnqBankId, BankDto.EnqtempId, BankDto.EnqAcctype, BankDto.EnqBankaccno, BankDto.EnqIfsc, BankDto.EnqAccName, BankDto.EnqBankname, BankDto.EnqBankbr, BankDto.UniqueId, BankDto.BankPinCode));
                return Ok(new ApiResultResponse(200, Bank, "Bank details Updated Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading UpdateBankDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpGet, Route("GetByIdBankDetails")]
        public async Task<IActionResult> GetByIdBankDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - GetByIdBankDetailsAsync method for Id = " + Id);
                var bankDetails = await _bankDetails.GetByIdBankDetailsAsync(Id, token).ConfigureAwait(false);
                if (bankDetails == null)
                {
                    _logger.Information("Error - 404 Bank Details not exists");
                    return new NotFoundObjectResult(new ApiException(404, "Bank Details not exists."));
                }
                _logger.Information("Completed - GetByIdBankDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, bankDetails, "Success."));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading GetByIdBankDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }

        [HttpPost, Route("DeleteBankDetails")]
        public async Task<IActionResult> DeleteBankDetailsAsync(int Id, CancellationToken token)
        {
            try
            {
                _logger.Information("Started - DeleteBankDetailsAsync method for Id = " + Id);
                bool isDeleted = await _bankDetails.DeleteBankDetailsAsync(Id, token).ConfigureAwait(false);
                if (!isDeleted)
                {
                    _logger.Information("Error - 404 Bank details not exists!");
                    return new NotFoundObjectResult(new ApiException(404, "Bank details not exists!"));
                }
                _logger.Information("Completed - DeleteBankDetailsAsync method for Id = " + Id);
                return Ok(new ApiResultResponse(200, true, "Bank Details Deleted Successfully"));
            }
            catch (System.Exception ex)
            {
                _logger.Error("Error occured while loading DeleteBankDetailsAsync page. Error message is: " + ex.Message + Environment.NewLine + "The stack trace is: " + ex.StackTrace);
                throw;
            }
        }
    }
}
