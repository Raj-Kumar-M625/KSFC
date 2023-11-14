using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.Helpers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.EntryOfOtherDebitsModule;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Admin.Controllers.IDM.EntryOfOtherDebits
{

    /// <summary>
    ///  Author: Gagana K; Module:EntryOfOtherDebits; Date:27/10/2022
    /// </summary>
    /// 
    
    public class EntryOfOtherDebitsController : BaseApiController
    {
        private readonly IEntryOfOtherDebitsService _entryOfOtherDebitsService;
        private readonly ILogger _logger;

        public EntryOfOtherDebitsController(IEntryOfOtherDebitsService entryOfOtherDebitsService, ILogger logger)
        {
            _entryOfOtherDebitsService = entryOfOtherDebitsService;
            _logger = logger;
        }

        [HttpGet, Route(EntryOfOtherDebitsRouteName.GetAllOtherDebitsList)]
        public async Task<IActionResult> GetAllOtherDebitsList(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllOtherDebitsList + accountNumber);
                var lst = await _entryOfOtherDebitsService.GetAllOtherDebitsList(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetGetAllOtherDebitsList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.OtherDebitsDetails));
                throw;
            }
        }

        [HttpPost, Route(EntryOfOtherDebitsRouteName.UpdateOtherDebitDetails)]
        public async Task<IActionResult> UpdateOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit, CancellationToken token)
        {
            try
            {
                if (othdebit != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.IdmOthdebitsDetailsDTO,
                    othdebit.LoanAcc, othdebit.LoanSub));

                     var basicDetail = await _entryOfOtherDebitsService.UpdateOtherDebitDetails(othdebit, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.IdmOthdebitsDetailsDTO,
                  othdebit.LoanAcc, othdebit.LoanSub));

                  return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                } 
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LoanAllocataion));
                throw;
            }
        }

        [HttpPost, Route(EntryOfOtherDebitsRouteName.DeleteOtherDebitDetails)]
        public async Task<IActionResult> DeleteOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit, CancellationToken token)
        {
            try
            {
                if (othdebit != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.IdmOthdebitsDetailsDTO,
                   othdebit.LoanAcc, othdebit.LoanSub));

                    var basicDetail = await _entryOfOtherDebitsService.DeleteOtherDebitDetails(othdebit, token).ConfigureAwait(false);
                 
                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.IdmOthdebitsDetailsDTO,
                   othdebit.LoanAcc, othdebit.LoanSub));

                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
               
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LoanAllocataion));
                throw;
            }
        }

        [HttpPost, Route(EntryOfOtherDebitsRouteName.CreateOtherDebitDetails)]
        public async Task<IActionResult> CreateOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit, CancellationToken token)
        {
            try
            {
                if (othdebit != null)
                {
                    _logger.Information(string.Format(LogAttribute.CreateStarted + " " + LogAttribute.IdmOthdebitsDetailsDTO,
                   othdebit.LoanAcc, othdebit.LoanSub));

                    var basicDetail = await _entryOfOtherDebitsService.CreateOtherDebitDetails(othdebit, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.CreateCompleted + " " + LogAttribute.IdmOthdebitsDetailsDTO,
                   othdebit.LoanAcc, othdebit.LoanSub));

                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LoanAllocataion));
                throw;
            }
        }

        [HttpPost, Route(EntryOfOtherDebitsRouteName.SubmitOtherDebitDetails)]
        public async Task<IActionResult> SubmitOtherDebitDetails(IdmOthdebitsDetailsDTO othdebit, CancellationToken token)
        {
            try
            {
                if (othdebit != null)
                {
                    _logger.Information(string.Format(LogAttribute.SubmitStarted + " " + LogAttribute.IdmOthdebitsDetailsDTO,
                     othdebit.LoanAcc, othdebit.LoanSub));

                    var basicDetail = await _entryOfOtherDebitsService.SubmitOtherDebitDetails(othdebit, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.SubmitCompleted + " " + LogAttribute.IdmOthdebitsDetailsDTO,
                     othdebit.LoanAcc, othdebit.LoanSub));

                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
              
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LoanAllocataion));
                throw;
            }
        }

    }
}
