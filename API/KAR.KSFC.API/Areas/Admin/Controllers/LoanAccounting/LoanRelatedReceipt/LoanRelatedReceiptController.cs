using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.Helpers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccounting.LoanRelatedReceipt;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using KAR.KSFC.Components.Common.Logging.Client;
using System;
using System.Threading;
using System.Threading.Tasks;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using System.Collections.Generic;
using System.Linq;

namespace KAR.KSFC.API.Areas.Admin.Controllers.AccountingOfficer.LoanReceipt
{
    public class LoanRelatedReceiptController : BaseApiController
    {
        private readonly ILoanRelatedReceiptService _loanreceiptService;
        private readonly ILogger _logger;

        public LoanRelatedReceiptController(ILoanRelatedReceiptService loanreceiptService, ILogger logger)
        {
            _loanreceiptService = loanreceiptService;
            _logger = logger;
        }

        #region ReceiptPayment List
        /// <Summary>
        /// Author: Gagana K; Module:ReceiptPayment; Date: 20/10/2022
        /// <summary

        [HttpGet, Route(GenerateReceipt.GetAllReceiptPaymentList)]
        public async Task<IActionResult> GetAllReceiptPaymentList(long accountNumber, CancellationToken token)
        {
            try
            {
                var lst = await _loanreceiptService.GetAllReceiptPaymentList(accountNumber, token).ConfigureAwait(false);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.GuarantorDeed));
                throw;
            }
        }
        
        [HttpPost, Route(GenerateReceipt.UpdateReceiptPaymentDetails)]
        public async Task<IActionResult> UpdateReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token)
        {
            try
            {
               _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.LoanRelatedReceiptsDto,
                  ReceiptPaymentDto.TblLaReceiptDet.ReceiptRefNo));

                var basicDetail = await _loanreceiptService.UpdateReceiptPaymentDetails(ReceiptPaymentDto, token).ConfigureAwait(false);
                if (ReceiptPaymentDto == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

                _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.LoanRelatedReceiptsDto,
                  ReceiptPaymentDto.TblLaReceiptDet.ReceiptRefNo));

                return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Condition));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.UpdateCreatePaymentDetails)]
        public async Task<IActionResult> UpdateCreatePaymentDetails(List<TblLaReceiptPaymentDetDTO> ReceiptPaymentDto, CancellationToken token)
        {
            try
            {
                _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.LoanRelatedReceiptsDto,
                     ReceiptPaymentDto.FirstOrDefault().TblLaPaymentDet.PaymentRefNo));

                var basicDetail = await _loanreceiptService.UpdateCreatePaymentDetails(ReceiptPaymentDto, token).ConfigureAwait(false);
                if (ReceiptPaymentDto == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

                _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.LoanRelatedReceiptsDto,
                     ReceiptPaymentDto.FirstOrDefault().TblLaPaymentDet.PaymentRefNo));
                return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Condition));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.DeleteReceiptPaymentDetails)]
        public async Task<IActionResult> DeleteReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token)
        {
            try
            {
                _logger.Information(string.Format(LogAttribute.DeleteStarted + "" + LogAttribute.LoanRelatedReceiptsDto,
                ReceiptPaymentDto.TblLaReceiptDet.ReceiptRefNo));

                var basicDetail = await _loanreceiptService.DeleteReceiptPaymentDetails(ReceiptPaymentDto, token).ConfigureAwait(false);
                if (ReceiptPaymentDto == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

                 _logger.Information(string.Format(LogAttribute.DeleteCompleted + "" + LogAttribute.LoanRelatedReceiptsDto,
                ReceiptPaymentDto.TblLaReceiptDet.ReceiptRefNo));

                return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Condition));
                throw;
            }
        }

        [HttpGet, Route(GenerateReceipt.GetAllReceiptRefNum)]
        public async Task<IActionResult> GetAllReceiptRefNum(CancellationToken token)
        {
            var data = await _loanreceiptService.GetAllReceiptRefNum(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));

        }

        [HttpGet, Route(GenerateReceipt.GetAllPaymentRefNum)]
        public async Task<IActionResult> GetAllPaymentRefNum(CancellationToken token)
        {
            var data = await _loanreceiptService.GetAllPaymentRefNum(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));

        }

        [HttpPost, Route(GenerateReceipt.ApproveReceiptPaymentDetails)]
        public async Task<IActionResult> ApproveReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token)
        {
            try
            {                
                var basicDetail = await _loanreceiptService.ApproveReceiptPaymentDetails(ReceiptPaymentDto, token).ConfigureAwait(false);
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

       
        [HttpPost, Route(GenerateReceipt.RejectReceiptPaymentDetails)]
        public async Task<IActionResult> RejectReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token)
        {
            try
            {                
                var basicDetail = await _loanreceiptService.RejectReceiptPaymentDetails(ReceiptPaymentDto, token).ConfigureAwait(false);
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

        [HttpPost, Route(GenerateReceipt.CreateReceiptPaymentDetails)]
        public async Task<IActionResult> CreateReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token)
        {
            try
            {
                 _logger.Information(string.Format(LogAttribute.UpdateStarted + ""+ LogAttribute.LoanRelatedReceiptsDto,
                ReceiptPaymentDto.TblLaReceiptDet.ReceiptRefNo));

                var basicDetail = await _loanreceiptService.CreateReceiptPaymentDetails(ReceiptPaymentDto, token).ConfigureAwait(false);
                if (ReceiptPaymentDto == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

                 _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.LoanRelatedReceiptsDto,
                ReceiptPaymentDto.TblLaReceiptDet.ReceiptRefNo));

                return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Condition));
                throw;
            }
        }

        [HttpPost, Route(GenerateReceipt.CreatePaymentDetails)]
        public async Task<IActionResult> CreatePaymentDetails(TblLaPaymentDetDTO ReceiptPaymentDto, CancellationToken token)
        {
            try
            {
                _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.LoanRelatedReceiptsDto,
                    ReceiptPaymentDto.PaymentRefNo));

                var basicDetail = await _loanreceiptService.CreatePaymentDetails(ReceiptPaymentDto, token).ConfigureAwait(false);
                if (ReceiptPaymentDto == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

                _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.LoanRelatedReceiptsDto,
                     ReceiptPaymentDto.PaymentRefNo));
                return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Condition));
                throw;
            }
        }

        #endregion
    }

}
