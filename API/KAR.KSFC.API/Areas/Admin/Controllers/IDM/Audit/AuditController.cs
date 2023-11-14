using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.Helpers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.AuditModule;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Admin.Controllers.IDM.Audit
{
    public class AuditController : BaseApiController
    {
        private readonly IAuditService _auditService;
        private readonly ILogger _logger;

        public AuditController(IAuditService auditService, ILogger logger)
        {
            _auditService = auditService;
            _logger = logger;
        }


        #region AuditClearance
        /// <Summary>
        /// Author: Gagana K; Module:AuditClearance; Date: 18/08/2022
        /// </Summary>
        [HttpGet, Route(RouteName.GetAllAuditClearanceList)]
        public async Task<IActionResult> GetAllAuditClearanceListAsync(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllAuditClearanceListAsync + accountNumber);
                var lst = await _auditService.GetAllAuditClearanceListAsync(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllAuditClearanceListAsync + accountNumber + Constants.Result + lst);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.AuditClearance));
                throw;
            }
        }

        [HttpPost, Route(RouteName.UpdateAuditClearanceDetails)]
        public async Task<IActionResult> UpdateAuditClearanceDetails(IdmAuditDetailsDTO AuditDto, CancellationToken token)
        {
            try
            {
                if (AuditDto != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.AuditClearanceDto,
                         AuditDto.IdmAuditId, AuditDto.AuditObservation, AuditDto.AuditCompliance));

                    var basicDetail = await _auditService.UpdateAuditClearanceDetails(AuditDto, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.AuditClearanceDto,
                    AuditDto.IdmAuditId, AuditDto.AuditObservation, AuditDto.AuditCompliance));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.AuditClearance));
                throw;
            }
        }

        [HttpPost, Route(RouteName.CreateAuditClearanceDetails)]
        public async Task<IActionResult> CreateAuditClearanceDetails(IdmAuditDetailsDTO AuditDto, CancellationToken token)
        {
            try
            {
                if (AuditDto != null)
                {
                    _logger.Information(string.Format(LogAttribute.CreateStarted + " " + LogAttribute.AuditClearanceDto,
                     AuditDto.IdmAuditId, AuditDto.AuditObservation, AuditDto.AuditCompliance));

                    var basicDetail = await _auditService.CreateAuditClearanceDetails(AuditDto, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.CreateCompleted + " " + LogAttribute.AuditClearanceDto,
                     AuditDto.IdmAuditId, AuditDto.AuditObservation, AuditDto.AuditCompliance));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.AuditClearance));
                throw;
            }
        }

        [HttpPost, Route(RouteName.DeleteAuditClearanceDetails)]
        public async Task<IActionResult> DeleteAuditClearanceDetails(IdmAuditDetailsDTO AuditDto, CancellationToken token)
        {
            try
            {
                if (AuditDto != null)
                {
                    _logger.Information(string.Format(LogAttribute.DeleteStarted + " " + LogAttribute.AuditClearanceDto,
                 AuditDto.IdmAuditId, AuditDto.AuditObservation, AuditDto.AuditCompliance));

                    var basicDetail = await _auditService.DeleteAuditClearanceDetails(AuditDto, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.DeleteCompleted + " " + LogAttribute.AuditClearanceDto,
                           AuditDto.IdmAuditId, AuditDto.AuditObservation, AuditDto.AuditCompliance));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.AuditClearance));
                throw;
            }
        }
        #endregion

    }
}
