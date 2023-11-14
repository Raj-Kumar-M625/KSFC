using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.Helpers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.LoanAllocationModule;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Admin.Controllers.IDM.LoanAllocation
{
    /// <summary>
    ///  Author: Gagana K; Module:Loan Alloaction; Date:28/09/2022
    /// </summary>
    public class LoanAllocationController : BaseApiController
    {
        private readonly ILoanAllocationService _loanAllocationService;
        private readonly ILogger _logger;

        public LoanAllocationController(ILoanAllocationService loanAllocationService, ILogger logger)
        {
            _loanAllocationService = loanAllocationService;
            _logger = logger;
        }

        [HttpGet, Route(LoanAllocationRouteName.GetAllLoanAllocationList)]
        public async Task<IActionResult> GetAllLoanAllocationList(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllLoanAllocationList + accountNumber);
                var lst = await _loanAllocationService.GetAllLoanAllocationList(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllLoanAllocationList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LoanAllocataion));
                throw;
            }
        }
        [HttpPost, Route(LoanAllocationRouteName.UpdateLoanAllocationDetails)]
        public async Task<IActionResult> UpdateLoanAllocationDetails(TblIdmDhcgAllcDTO AllocationDTO, CancellationToken token)
        {
            try
            {
                if (AllocationDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.LoanAllocataionDto,
                    AllocationDTO.DcalcCd, AllocationDTO.DcalcDetails, AllocationDTO.DcalcRqdt, AllocationDTO.DcalcComdt));

                    var basicDetail = await _loanAllocationService.UpdateLoanAllocationDetails(AllocationDTO, token).ConfigureAwait(false);
                   
                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.LoanAllocataionDto,
                    AllocationDTO.DcalcCd, AllocationDTO.DcalcDetails, AllocationDTO.DcalcRqdt, AllocationDTO.DcalcComdt));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LoanAllocataion));
                throw;
            }
        }

        [HttpPost, Route(LoanAllocationRouteName.CreateLoanAllocationDetails)]
        public async Task<IActionResult> CreateLoanAllocationDetails(TblIdmDhcgAllcDTO AllocationDTO, CancellationToken token)
        {
            try
            {
                if (AllocationDTO != null)
                {

                    _logger.Information(string.Format(LogAttribute.CreateStarted + "" + LogAttribute.LoanAllocataionDto,
                    AllocationDTO.DcalcCd, AllocationDTO.DcalcDetails, AllocationDTO.DcalcRqdt, AllocationDTO.DcalcComdt));

                    var basicDetail = await _loanAllocationService.CreateLoanAllocationDetails(AllocationDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.CreateCompleted + "" + LogAttribute.LoanAllocataionDto,
                AllocationDTO.DcalcCd, AllocationDTO.DcalcDetails, AllocationDTO.DcalcRqdt, AllocationDTO.DcalcComdt));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LoanAllocataion));
                throw;
            }
        }

        [HttpPost, Route(LoanAllocationRouteName.DeleteLoanAllocationDetails)]
        public async Task<IActionResult> DeleteLoanAllocationDetails(TblIdmDhcgAllcDTO AllocationDTO, CancellationToken token)
        {
            try
            {
                if (AllocationDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.DeleteStarted + "" + LogAttribute.LoanAllocataionDto,
              AllocationDTO.DcalcCd, AllocationDTO.DcalcDetails, AllocationDTO.DcalcRqdt, AllocationDTO.DcalcComdt));

                    var basicDetail = await _loanAllocationService.DeleteLoanAllocationDetails(AllocationDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.DeleteCompleted + "" + LogAttribute.LoanAllocataionDto,
                AllocationDTO.DcalcCd, AllocationDTO.DcalcDetails, AllocationDTO.DcalcRqdt, AllocationDTO.DcalcComdt));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LoanAllocataion));
                throw;
            }
        }
    }
}
