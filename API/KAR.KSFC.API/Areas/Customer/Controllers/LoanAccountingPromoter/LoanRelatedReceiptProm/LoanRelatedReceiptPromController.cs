using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.Helpers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccountingPromoter.LoanRelatedReceiptProm;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Logging.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.Components.Common.Utilities.Errors;
using System.Collections.Generic;

namespace KAR.KSFC.API.Areas.Customer.Controllers.LoanAccountingPromoter.Controllers
{
    public class LoanRelatedReceiptPromController : BaseApiController
    {
        private readonly ILoanRelatedReceiptPromService _loanreceiptService;
        private readonly ILogger _logger;

        public LoanRelatedReceiptPromController(ILoanRelatedReceiptPromService loanreceiptService, ILogger logger)
        {
            _loanreceiptService = loanreceiptService;
            _logger = logger;
        }

        #region GenerateReceipt

        [HttpGet, Route(GenerateReceipt.GetAllGenerateReceiptPaymentList)]
        public async Task<IActionResult> GetAllGenerateReceiptPaymentList(long accountNumber, CancellationToken token)
        {
            try
            {
                var lst = await _loanreceiptService.GetAllGenerateReceiptPaymentList(accountNumber, token).ConfigureAwait(false);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.GuarantorDeed));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.UpdateCreatePromPaymentDetails)]
        public async Task<IActionResult> UpdateCreatePromPaymentDetails(List<TblLaReceiptPaymentDetDTO> ReceiptPaymentDto, CancellationToken token)
        {
            try
            {
                
                var basicDetail = await _loanreceiptService.UpdateCreatePromPaymentDetails(ReceiptPaymentDto, token).ConfigureAwait(false);
                if (ReceiptPaymentDto == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                                
                return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Condition));
                throw;
            }
        }

        #endregion
        #region RecieptPayments


        [HttpGet, Route(GenerateReceipt.GetAllReceiptPayment)]
        public async Task<IActionResult> GetAllRecipetsForPayment(int PaymentId, CancellationToken token)
        {
            try
            {
                var lst = await _loanreceiptService.GetAllRecipetsForPayment(PaymentId, token).ConfigureAwait(false);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.GuarantorDeed));
                throw;
            }
        }

        #endregion
    }

}
