using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.Helpers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.DisbursementModule;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Update;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Admin.Controllers.IDM.Disbursement
{
    public class DisbursementController : BaseApiController
    {
        private readonly IDisbursementService _disbursementService;
        private readonly ILogger _logger;
        public DisbursementController(IDisbursementService disbursementService, ILogger logger)
        {
            _disbursementService = disbursementService;
            _logger = logger;
        }
      
        #region Disbursement Condition
        /// <summary>
        ///  Author: Manoj; Module: Disbursement Condition; Date:17/08/2022
        /// </summary>
        [HttpGet, Route(DisbursementRouteName.GetAllDisbursementList)]
        public async Task<IActionResult> GetAllDisbursementList(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllDisbursementList + accountNumber);
                var lst = await _disbursementService.GetDisbursementConditionListAsync(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllDisbursementList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.DisbursementCondition));
                throw;
            }
        }
        /// <summary>
        ///  Author: Manoj; Module: Disbursement Condition; Date:17/08/2022
        ///  Modified: Dev Patel; Added Logger; Date:21/10/2022
        /// </summary>
        [HttpPost, Route(DisbursementRouteName.DeleteDisbursementConditionDetails)]
        public async Task<IActionResult> DeleteLDConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            try
            {
                _logger.Information(LogAttribute.DeleteStarted);
                var basicDetail = await _disbursementService.DeleteDisbursementCondtionDetails(CondtionDTO, token).ConfigureAwait(false);
                if (CondtionDTO == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                _logger.Information(LogAttribute.DeleteCompleted);
                return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.DisbursementCondition));
                throw;
            }
        }
        /// <summary>
        ///  Author: Manoj; Module: Disbursement Condition; Date:17/08/2022
        /// </summary>
        [HttpPost, Route(DisbursementRouteName.UpdateDisbursementConditionDetails)]
        public async Task<IActionResult> UpdateDisbursementConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            try
            {
                if (CondtionDTO != null)
                {
                  //  _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.ConditionDTO,
                  //CondtionDTO.CondDetId, CondtionDTO.CondDetails, CondtionDTO.TblCondStageMastDTO.CondStgDets));

                    var basicDetail = await _disbursementService.UpdateDisbursementConditionDetails(CondtionDTO, token).ConfigureAwait(false);
                
                    
                //_logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.ConditionDTO,
                //          CondtionDTO.CondDetId, CondtionDTO.CondDetails, CondtionDTO.TblCondStageMastDTO.CondStgDets));

                return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                
                }
                
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.DisbursementCondition));
                throw;
            }
        }

        /// <summary>
        ///  Author: Manoj; Module: Disbursement Condition; Date:17/08/2022
        /// </summary>
        [HttpPost, Route(DisbursementRouteName.CreateDisbursementConditionDetails)]
        public async Task<IActionResult> CreateDisbursementConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            try
            {
                _logger.Information(LogAttribute.CreateStarted);
                var basicDetail = await _disbursementService.CreateDisbursementConditionDetails(CondtionDTO, token).ConfigureAwait(false);
                if (CondtionDTO == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                _logger.Information(LogAttribute.CreateCompleted);
                return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.DisbursementCondition));
                throw;
            }
        }

        #endregion

        #region Form8AndForm13
        /// <summary>
        ///  Author: Manoj; Module: Form 8 and Form 13; Date:19/08/2022
        /// </summary>
        [HttpGet, Route(DisbursementRouteName.GetAllForm8AndForm13List)]
        public async Task<IActionResult> GetAllForm8AndForm13List(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllForm8AndForm13List + accountNumber);
                var lst = await _disbursementService.GetAllForm8AndForm13ListAsync(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllForm8AndForm13List + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Form8AndForm13));
                throw;
            }
        }
        /// <summary>
        ///  Author: Gowtham; Module: Form 8 and Form 13; Date:22/08/2022
        /// </summary>
        [HttpPost, Route(DisbursementRouteName.UpdateForm8AndForm13Details)]
        public async Task<IActionResult> UpdateForm8AndForm13Details(Form8AndForm13DTO form8and13DTO, CancellationToken token)
        {
            try
            {
                if (form8and13DTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.Form8AndForm13DTO,
                 form8and13DTO.DF813Id, form8and13DTO.DF813t1, form8and13DTO.DF813Dt, form8and13DTO.DF813Sno));

                    var basicDetail = await _disbursementService.UpdateForm8AndForm13Details(form8and13DTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.Form8AndForm13DTO,
                          form8and13DTO.DF813Id, form8and13DTO.DF813t1, form8and13DTO.DF813Dt, form8and13DTO.DF813Sno));

                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

                
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Form8AndForm13));
                throw;
            }
        }

        /// <summary>
        ///  Author: Gowtham; Module: Form 8 and Form 13; Date:22/08/2022
        /// </summary>
        [HttpPost, Route(DisbursementRouteName.CreateForm8AndForm13Details)]
        public async Task<IActionResult> CreateForm8AndForm13Details(Form8AndForm13DTO form8and13DTO, CancellationToken token)
        {
            try
            {
                _logger.Information(LogAttribute.CreateStarted);
                var basicDetail = await _disbursementService.CreateForm8AndForm13Details(form8and13DTO, token).ConfigureAwait(false);
                if (form8and13DTO == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                _logger.Information(LogAttribute.CreateCompleted);
                return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Form8AndForm13));
                throw;
            }
        }
        /// <summary>
        ///  Author: Gowtham; Module: Form 8 and Form 13; Date:22/08/2022
        /// </summary>
        [HttpPost, Route(DisbursementRouteName.DeleteForm8AndForm13Details)]
        public async Task<IActionResult> DeleteForm8AndForm13Details(Form8AndForm13DTO form8and13DTO, CancellationToken token)
        {
            try
            {
                if (form8and13DTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.DeleteStarted + " " + LogAttribute.Form8AndForm13DTO,
                 form8and13DTO.DF813Id, form8and13DTO.DF813t1, form8and13DTO.DF813Dt, form8and13DTO.DF813Sno));

                    var basicDetail = await _disbursementService.DeleteForm8AndForm13Details(form8and13DTO, token).ConfigureAwait(false);
                   
                    _logger.Information(string.Format(LogAttribute.DeleteCompleted + " " + LogAttribute.Form8AndForm13DTO,
                  form8and13DTO.DF813Id, form8and13DTO.DF813t1, form8and13DTO.DF813Dt, form8and13DTO.DF813Sno));

                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

                
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Form8AndForm13));
                throw;
            }
        }
        #endregion

        #region Sidbi Approval
        ///<summary>
        /// Author: Dev Patel; Module: Sidbi Approval; Date: 17/08/2022
        /// Modified: Dev Patel; Added logger; Date: 21/10/2022
        ///</summary>
        [HttpGet, Route(DisbursementRouteName.GetAllSidbiApprovalDetails)]
        public async Task<IActionResult> GetAllSidbiApprovalDetails(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllSidbiApprovalDetails + accountNumber);
                var lst = await _disbursementService.GetSidbiApprovalAsync(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllSidbiApprovalDetails + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.SidbiApproval));
                throw;
            }
        }
        
        [HttpPost, Route(DisbursementRouteName.UpdateSidbiApprovalDetails)]
        public async Task<IActionResult> UpdateSidbiApprovalDetails(IdmSidbiApprovalDTO sidbi, CancellationToken token)
        {
            try
            {

                if (sidbi != null)
                {
                    _logger.Information(Constants.UpdateSidbiApprovalDetails);
                    var basicDetail = await _disbursementService.UpdateSidbiApprovalDetails(sidbi, token).ConfigureAwait(false);
                    _logger.Information(Constants.CompletedUpdateSidbiApprovalDetails);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                    
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

                
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.SidbiApproval));
                throw;
            }
        }
        #endregion

        #region AdditionalCondition
        [HttpGet, Route(DisbursementRouteName.GetAllAdditionalCondition)]
        public async Task<IActionResult> GetAllAdditionalConditionList(long accountNumber, CancellationToken token)
        {
            try
            {
                 _logger.Information(Constants.GetAllAdditionalConditionList + accountNumber);
                var lst = await _disbursementService.GetAdditionConditionListAsync(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllAdditionalConditionList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.AdditionalCondition));
                throw;
            }
        }

        [HttpPost, Route(DisbursementRouteName.CreateAdditionalConditionDetails)]
        public async Task<IActionResult> CreateAdditionalConditionDetails(AdditionConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            try
            {
                if (CondtionDTO != null)
                {
                    _logger.Information(Constants.CreateAdditionalConditionDetails);
                    var basicDetail = await _disbursementService.CreateAdditionalConditionDetails(CondtionDTO, token).ConfigureAwait(false);
                    _logger.Information(Constants.CompletedCreateAdditionalConditionDetails);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.AdditionalCondition));
                throw;
            }
        }

        [HttpPost, Route(DisbursementRouteName.UpdateAdditionalConditionDetails)]
        public async Task<IActionResult> UpdateAdditonalConditionDetails(AdditionConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            try
            {
                if (CondtionDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.ConditionDTO,
                    CondtionDTO.AddCondId, CondtionDTO.AddCondDetails, CondtionDTO.WhRelAllowed, CondtionDTO.AddCondStage));

                    var basicDetail = await _disbursementService.UpdateAdditionalConditionDetails(CondtionDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.ConditionDTO,
                    CondtionDTO.AddCondId, CondtionDTO.AddCondDetails, CondtionDTO.WhRelAllowed, CondtionDTO.AddCondStage));

                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));
                }
                    
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

                
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.AdditionalCondition));
                throw;
            }
        }

        [HttpPost, Route(DisbursementRouteName.DeleteAdditionalConditionDetails)]
        public async Task<IActionResult> DeleteAdditionalConditionDetails(AdditionConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            try
            {
                if (CondtionDTO != null)
                {

                    _logger.Information(string.Format(LogAttribute.DeleteStarted + "" + LogAttribute.ConditionDTO,
                 CondtionDTO.AddCondId, CondtionDTO.AddCondDetails, CondtionDTO.WhRelAllowed, CondtionDTO.AddCondStage));

                    var basicDetail = await _disbursementService.DeleteAdditionalCondtionDetails(CondtionDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.DeleteCompleted + "" + LogAttribute.ConditionDTO,
                 CondtionDTO.AddCondId, CondtionDTO.AddCondDetails, CondtionDTO.WhRelAllowed, CondtionDTO.AddCondStage));


                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Created));

                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

              
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.AdditionalCondition));
                throw;
            }
        }
        #endregion

        #region FirstInvestmentClause

        /// <summary>
        ///  Author: Akhila; Module: FirstInvestmentClause; Date:20/08/2022
        /// </summary>
        [HttpGet, Route(DisbursementRouteName.GetAllFirstInvestmentClauseDetails)]
        public async Task<IActionResult> GetAllFirstInvestmentClause(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllFirstInvestmentClause + accountNumber);
                var lst = await _disbursementService.GetFirstInvestmentClauseAsync(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllFirstInvestmentClause + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.FirstInvestmentClause));
                throw;
            }
        }
        /// <summary>
        ///  Author: Akhila; Module: FirstInvestmentClause; Date:23/08/2022
        ///  Modified By:Swetha M Date:02/09/2022
        ///  Modified By:Dev Patel Date:21/10/2022
        /// </summary>
        [HttpPost, Route(DisbursementRouteName.UpdateFirstInvestmentClauseDetails)]
        public async Task<IActionResult> UpdateFirstInvestmentClauseDetails(IdmFirstInvestmentClauseDTO FICFormData, CancellationToken token)
        {
            try
            {
                _logger.Information(LogAttribute.UpdateStarted);
                var FICBasicDetail = await _disbursementService.UpdateAllFirstInvestmentClauseDetails(FICFormData, token).ConfigureAwait(false);
                if (FICFormData == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                _logger.Information(LogAttribute.UpdateCompleted);
                return Ok(new ApiResultResponse(200, FICBasicDetail, CommonLogHelpers.Updated));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.FirstInvestmentClause));
                throw;
            }
        }
        #endregion

        #region Other Relaxation
        /// <summary>
        ///  Author: Dev Patel; Module: FirstInvestmentClause(Other Relaxation); Date:11/10/2022        
        /// </summary>
        [HttpGet, Route(DisbursementRouteName.GetAllOtherRelaxation)]
        public async Task<IActionResult> GetAllOtherRelaxation(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllOtherRelaxation + accountNumber);
                var lst = await _disbursementService.GetAllOtherRelaxation(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllOtherRelaxation + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.FirstInvestmentClause));
                throw;
            }
        }

        /// <summary>
        ///  Author: Dev Patel; Module: FirstInvestmentClause(Other Relaxation); Date:13/10/2022        
        /// </summary>
        [HttpPost, Route(DisbursementRouteName.UpdateOtherRelaxation)]
        public async Task<IActionResult> UpdateOtherRelaxation(List<RelaxationDTO> relax, CancellationToken token)
        {
            try
            {
                _logger.Information(LogAttribute.UpdateStarted);
                var basicDetail = await _disbursementService.UpdateOtherRelaxation(relax, token).ConfigureAwait(false);
                if (relax == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                _logger.Information(LogAttribute.UpdateCompleted);
                return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.FirstInvestmentClause));
                throw;
            }
        }
        #endregion
    }
}