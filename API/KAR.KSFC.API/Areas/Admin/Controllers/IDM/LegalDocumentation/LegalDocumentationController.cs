using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.Helpers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.LegalDocumentationModule;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Admin.Controllers.IDM.LegalDocumentation
{
    public class LegalDocumentationController : BaseApiController
    {
        private readonly ILegalDocumentationService _legalDocumentationService;
        private readonly ILogger _logger;
        public LegalDocumentationController(ILegalDocumentationService legalDocumentationService, ILogger logger)
        {
            _legalDocumentationService = legalDocumentationService;
            _logger = logger;
        }

        #region Primary Security

        /// <summary>
        ///  Author: Rajesh; Module: Primary/CollateralSecurity; Date:15/07/2022
        /// </summary>
        [HttpGet, Route(RouteName.GetAllSecurityHolders)]
        public async Task<IActionResult> GetAllSecurityHolders(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetPrimaryCollateralList + accountNumber);
                var lst = await _legalDocumentationService.GetPrimaryCollateralListAsync(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetPrimaryCollateralList + accountNumber + Constants.Result + lst);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Security));
                throw;
            }
        }
        [HttpPost, Route(RouteName.UpdateLDSecurityDetails)]
        public async Task<IActionResult> UpdateLDSecurityDetails(IdmSecurityDetailsDTO lDSecurityDetailsDTO, CancellationToken token)
        {
            try
            {
                if (lDSecurityDetailsDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.SecurityDTO,
                  lDSecurityDetailsDTO.IdmDeedDetId, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecNameHolder, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecCd, lDSecurityDetailsDTO.TblSecurityRefnoMast.PjsecCd, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecurityDetails, lDSecurityDetailsDTO.DeedNo, lDSecurityDetailsDTO.DeedDesc, lDSecurityDetailsDTO.SubregistrarCd, lDSecurityDetailsDTO.ExecutionDate, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecurityValue));

                    var basicDetail = await _legalDocumentationService.UpdatePrimaryCollateralDetail(lDSecurityDetailsDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.SecurityDTO,
                  lDSecurityDetailsDTO.IdmDeedDetId, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecNameHolder, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecCd, lDSecurityDetailsDTO.TblSecurityRefnoMast.PjsecCd, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecurityDetails, lDSecurityDetailsDTO.DeedNo, lDSecurityDetailsDTO.DeedDesc, lDSecurityDetailsDTO.SubregistrarCd, lDSecurityDetailsDTO.ExecutionDate, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecurityValue));
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Security));
                throw;
            }
        }


        #endregion


        #region Collet Security

        [HttpGet, Route(RouteName.GetAllSecurityHolder)]
        public async Task<IActionResult> GetAllSecurityHolder(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetCollateralList + accountNumber);
                var lst = await _legalDocumentationService.GetCollateralListAsync(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetCollateralList + accountNumber + Constants.Result + lst);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Security));
                throw;
            }
        }
        [HttpPost, Route(RouteName.UpdateLDSecurityDetail)]
        public async Task<IActionResult> UpdateLDSecurityDetail(IdmSecurityDetailsDTO lDSecurityDetailsDTO, CancellationToken token)
        {
            try
            {
                if (lDSecurityDetailsDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.SecurityDTO,
                  lDSecurityDetailsDTO.IdmDeedDetId, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecNameHolder, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecCd, lDSecurityDetailsDTO.TblSecurityRefnoMast.PjsecCd, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecurityDetails, lDSecurityDetailsDTO.DeedNo, lDSecurityDetailsDTO.DeedDesc, lDSecurityDetailsDTO.SubregistrarCd, lDSecurityDetailsDTO.ExecutionDate, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecurityValue));

                    var basicDetail = await _legalDocumentationService.UpdateCollateralDetail(lDSecurityDetailsDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.SecurityDTO,
                  lDSecurityDetailsDTO.IdmDeedDetId, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecNameHolder, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecCd, lDSecurityDetailsDTO.TblSecurityRefnoMast.PjsecCd, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecurityDetails, lDSecurityDetailsDTO.DeedNo, lDSecurityDetailsDTO.DeedDesc, lDSecurityDetailsDTO.SubregistrarCd, lDSecurityDetailsDTO.ExecutionDate, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecurityValue));
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Security));
                throw;
            }
        }

        [HttpPost, Route(RouteName.CreateLDSecurityDetail)]
        public async Task<IActionResult> CreateLDSecurityDetail(IdmSecurityDetailsDTO lDSecurityDetailsDTO, CancellationToken token)
        {
            try
            {
                if (lDSecurityDetailsDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.CreateStarted + " " + LogAttribute.SecurityDTO,
                  lDSecurityDetailsDTO.IdmDeedDetId, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecNameHolder, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecCd, lDSecurityDetailsDTO.TblSecurityRefnoMast.PjsecCd, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecurityDetails, lDSecurityDetailsDTO.DeedNo, lDSecurityDetailsDTO.DeedDesc, lDSecurityDetailsDTO.SubregistrarCd, lDSecurityDetailsDTO.ExecutionDate, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecurityValue));

                    var basicDetail = await _legalDocumentationService.UpdateCollateralDetail(lDSecurityDetailsDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.CreateCompleted + " " + LogAttribute.SecurityDTO,
                  lDSecurityDetailsDTO.IdmDeedDetId, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecNameHolder, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecCd, lDSecurityDetailsDTO.TblSecurityRefnoMast.PjsecCd, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecurityDetails, lDSecurityDetailsDTO.DeedNo, lDSecurityDetailsDTO.DeedDesc, lDSecurityDetailsDTO.SubregistrarCd, lDSecurityDetailsDTO.ExecutionDate, lDSecurityDetailsDTO.TblSecurityRefnoMast.SecurityValue));
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Security));
                throw;
            }
        }





        #endregion


        #region CERSAI
        /// <summary>
        ///  Author: Gagana K; Module: CERSAI Registration; Date:28/07/2022
        /// </summary>
        [HttpGet, Route(RouteName.GetAllCERSAIRegistrationList)]
        public async Task<IActionResult> GetAllCERSAIRegistrationAsync(long accountNumber,string parameter, CancellationToken token)

        {
            try
            {
                _logger.Information(Constants.GetAllCERSAIRegistrationAsync + accountNumber);
                var lst = await _legalDocumentationService.GetAllCERSAIRegistrationAsync(accountNumber,parameter, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllCERSAIRegistrationAsync + accountNumber + Constants.Result + lst);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Cersai));
                throw;
            }
        }

        [HttpPost, Route(RouteName.CreateLDCersaiRegDetails)]
        public async Task<IActionResult> CreateLDCersaiRegDetails(IdmCersaiRegDetailsDTO CersaiRegDTO, CancellationToken token)
        {
            try
            {
                if (CersaiRegDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.CersaiDTO,
                    CersaiRegDTO.IdmDsbChargeId, CersaiRegDTO.CersaiRegNo, CersaiRegDTO.CersaiRegDate, CersaiRegDTO.CersaiRemarks));

                    var basicDetail = await _legalDocumentationService.CreateLDCersaiRegDetails(CersaiRegDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.CersaiDTO,
                  CersaiRegDTO.IdmDsbChargeId, CersaiRegDTO.CersaiRegNo, CersaiRegDTO.CersaiRegDate, CersaiRegDTO.CersaiRemarks));
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Cersai));
                throw;
            }
        }

        [HttpPost, Route(RouteName.DeleteLDCersaiRegDetails)]
        public async Task<IActionResult> DeleteLDCersaiRegDetails(IdmCersaiRegDetailsDTO CersaiRegDTO, CancellationToken token)
        {
            try
            {
                if (CersaiRegDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.DeleteStarted + " " + LogAttribute.CersaiDTO,
                    CersaiRegDTO.IdmDsbChargeId, CersaiRegDTO.CersaiRegNo, CersaiRegDTO.CersaiRegDate, CersaiRegDTO.CersaiRemarks, CersaiRegDTO.AssetDet));

                    var basicDetail = await _legalDocumentationService.DeleteLDCersaiDetails(CersaiRegDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.DeleteCompleted + " " + LogAttribute.CersaiDTO,
                    CersaiRegDTO.IdmDsbChargeId, CersaiRegDTO.CersaiRegNo, CersaiRegDTO.CersaiRegDate, CersaiRegDTO.CersaiRemarks,  CersaiRegDTO.AssetDet));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Condition));
                throw;
            }
        }

        #endregion 

        #region SecurityCharge

        /// <summary>
        ///  Author: Sandeep; Module: SecurityCharge; Date:21/07/2022
        /// </summary>
        [HttpGet, Route(RouteName.GetAllSecurityChargeList)]
        public async Task<IActionResult> GetAllSecurityChargeAsync(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllSecurityChargeAsync + accountNumber);
                var lst = await _legalDocumentationService.GetAllSecurityChargeAsync(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllSecurityChargeAsync + accountNumber + Constants.Result + lst);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.SecurityCharge));
                throw;
            }
        }

        [HttpPost, Route(RouteName.UpdateSecurityChargeDetails)]
        public async Task<IActionResult> UpdateSecurityChargeDetails(IdmSecurityChargeDTO lDSecurityDetailsDTO, CancellationToken token)
        {
            try
            {
                if (lDSecurityDetailsDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.SecurityChargeDTO,
                lDSecurityDetailsDTO.IdmDsbChargeId, lDSecurityDetailsDTO.ChargeTypeCd, lDSecurityDetailsDTO.RequestLtrNo, lDSecurityDetailsDTO.RequestLtrDate, lDSecurityDetailsDTO.BankIfscCd, lDSecurityDetailsDTO.BankRequestLtrNo, lDSecurityDetailsDTO.BankRequestLtrDate, lDSecurityDetailsDTO.NocIssueBy, lDSecurityDetailsDTO.NocIssueTo, lDSecurityDetailsDTO.SecurityDets,
                lDSecurityDetailsDTO.NocDate, lDSecurityDetailsDTO.ChargeDetails, lDSecurityDetailsDTO.ChargeConditions, lDSecurityDetailsDTO.AuthLetterBy, lDSecurityDetailsDTO.BoardResolutionDate, lDSecurityDetailsDTO.MoeInsuredDate, lDSecurityDetailsDTO.ChargePurpose));
                    var basicDetail = await _legalDocumentationService.UpdateSecurityChargeDetails(lDSecurityDetailsDTO, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.SecurityChargeDTO,
                 lDSecurityDetailsDTO.IdmDsbChargeId, lDSecurityDetailsDTO.ChargeTypeCd, lDSecurityDetailsDTO.RequestLtrNo, lDSecurityDetailsDTO.RequestLtrDate, lDSecurityDetailsDTO.BankIfscCd, lDSecurityDetailsDTO.BankRequestLtrNo, lDSecurityDetailsDTO.BankRequestLtrDate, lDSecurityDetailsDTO.NocIssueBy, lDSecurityDetailsDTO.NocIssueTo, lDSecurityDetailsDTO.SecurityDets,
                 lDSecurityDetailsDTO.NocDate, lDSecurityDetailsDTO.ChargeDetails, lDSecurityDetailsDTO.ChargeConditions, lDSecurityDetailsDTO.AuthLetterBy, lDSecurityDetailsDTO.BoardResolutionDate, lDSecurityDetailsDTO.MoeInsuredDate, lDSecurityDetailsDTO.ChargePurpose));
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Security));
                throw;
            }
        }

      
        #endregion

        #region  Hypothecation
        /// <summary>
        ///  Author: Manoj; Module: Hypothecation; Date:21/07/2022
        /// </summary>

        [HttpGet, Route(RouteName.GetAllHypothecationList)]
        public async Task<IActionResult> GetAllHypothecationList(long accountNumber, string paramater, CancellationToken token)

        {
            try
            {
                _logger.Information(Constants.GetAllHypothecationList + accountNumber);
                var lst = await _legalDocumentationService.GetAllHypothecationAsync(accountNumber, paramater, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllHypothecationList + accountNumber + Constants.Result + lst);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Security));
                throw;
            }
        }
        [HttpGet, Route(RouteName.GetAllAssetRefList)]
        public async Task<IActionResult> GetAllAssetRefListAsync(long accountNumber, CancellationToken token)

        {
            try
            {
                _logger.Information(Constants.GetAllAssetRefListAsync + accountNumber);
                var lst = await _legalDocumentationService.GetAllAssetRefListAsync(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllAssetRefListAsync + accountNumber + Constants.Result + lst);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Hypothecation));
                throw;
            }
        }
        [HttpPost, Route(RouteName.CreateLDHypothecationDetails)]
        public async Task<IActionResult> CreateLDHypothecationDetails(List<IdmHypotheDetailsDTO> IDHypotheDetailsDTO, CancellationToken token)
        {
            try
            {
                if (IDHypotheDetailsDTO != null)
                {
                    bool createHypothMap = true;
                    foreach (var lDHypotheDetailsDTO in IDHypotheDetailsDTO)
                    {
                        _logger.Information(string.Format(LogAttribute.CreateStarted + " " + LogAttribute.HypoDTO,
                    lDHypotheDetailsDTO.IdmHypothDetId, lDHypotheDetailsDTO.LoanAcc, lDHypotheDetailsDTO.LoanSub, lDHypotheDetailsDTO.OffcCd, lDHypotheDetailsDTO.AssetCd, lDHypotheDetailsDTO.HypothNo, lDHypotheDetailsDTO.HypothDesc, lDHypotheDetailsDTO.AssetValue, lDHypotheDetailsDTO.ExecutionDate,
                    lDHypotheDetailsDTO.HypothUpload, lDHypotheDetailsDTO.ApprovedEmpId, lDHypotheDetailsDTO.HypothValue, lDHypotheDetailsDTO.Action));
                        var basicDetail = await _legalDocumentationService.CreateLDHypothecationDetails(lDHypotheDetailsDTO, createHypothMap, token).ConfigureAwait(false);
                        _logger.Information(string.Format(LogAttribute.CreateCompleted + " " + LogAttribute.HypoDTO,
                    lDHypotheDetailsDTO.IdmHypothDetId, lDHypotheDetailsDTO.HypothValue, lDHypotheDetailsDTO.LoanAcc, lDHypotheDetailsDTO.LoanSub, lDHypotheDetailsDTO.OffcCd, lDHypotheDetailsDTO.AssetCd, lDHypotheDetailsDTO.HypothNo, lDHypotheDetailsDTO.HypothDesc, lDHypotheDetailsDTO.AssetValue, lDHypotheDetailsDTO.ExecutionDate,
                    lDHypotheDetailsDTO.HypothUpload, lDHypotheDetailsDTO.ApprovedEmpId, lDHypotheDetailsDTO.Action));
                        createHypothMap = false;
                    }
                        return Ok(new ApiResultResponse(200, IDHypotheDetailsDTO, CommonLogHelpers.Created));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
              
              
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Hypothecation));
                throw;
            }
        }
        [HttpPost, Route(RouteName.UpdateLDHypothecationDetails)]
        public async Task<IActionResult> UpdateLDHypothecationDetails(IdmHypotheDetailsDTO lDHypotheDetailsDTO, CancellationToken token)
        {
            try
            {
                if (lDHypotheDetailsDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.HypoDTO,
                lDHypotheDetailsDTO.IdmHypothDetId, lDHypotheDetailsDTO.LoanAcc, lDHypotheDetailsDTO.LoanSub, lDHypotheDetailsDTO.OffcCd, lDHypotheDetailsDTO.AssetCd, lDHypotheDetailsDTO.HypothNo, lDHypotheDetailsDTO.HypothDesc, lDHypotheDetailsDTO.AssetValue, lDHypotheDetailsDTO.ExecutionDate,
                lDHypotheDetailsDTO.HypothUpload, lDHypotheDetailsDTO.ApprovedEmpId, lDHypotheDetailsDTO.Action));
                    var basicDetail = await _legalDocumentationService.UpdateLDHypothecationDetails(lDHypotheDetailsDTO, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.HypoDTO,
               lDHypotheDetailsDTO.IdmHypothDetId, lDHypotheDetailsDTO.LoanAcc, lDHypotheDetailsDTO.LoanSub, lDHypotheDetailsDTO.OffcCd, lDHypotheDetailsDTO.AssetCd, lDHypotheDetailsDTO.HypothNo, lDHypotheDetailsDTO.HypothDesc, lDHypotheDetailsDTO.AssetValue, lDHypotheDetailsDTO.ExecutionDate,
               lDHypotheDetailsDTO.HypothUpload, lDHypotheDetailsDTO.ApprovedEmpId, lDHypotheDetailsDTO.Action));
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Hypothecation));
                throw;
            }
        }

        [HttpPost, Route(RouteName.DeleteLDHypothecationDetails)]
        public async Task<IActionResult> DeleteLDHypothecationDetails(IdmHypotheDetailsDTO lDHypotheDetailsDTO, CancellationToken token)
        {
            try
            {
                if (lDHypotheDetailsDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.DeleteStarted + " " + LogAttribute.HypoDTO,
                 lDHypotheDetailsDTO.IdmHypothDetId, lDHypotheDetailsDTO.LoanAcc, lDHypotheDetailsDTO.LoanSub, lDHypotheDetailsDTO.OffcCd, lDHypotheDetailsDTO.AssetCd, lDHypotheDetailsDTO.HypothNo, lDHypotheDetailsDTO.HypothDesc, lDHypotheDetailsDTO.AssetValue, lDHypotheDetailsDTO.ExecutionDate,
                 lDHypotheDetailsDTO.HypothUpload, lDHypotheDetailsDTO.ApprovedEmpId, lDHypotheDetailsDTO.Action));
                    var basicDetail = await _legalDocumentationService.DeleteHypothecationDetails(lDHypotheDetailsDTO, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.DeleteCompleted + " " + LogAttribute.HypoDTO,
               lDHypotheDetailsDTO.IdmHypothDetId, lDHypotheDetailsDTO.LoanAcc, lDHypotheDetailsDTO.LoanSub, lDHypotheDetailsDTO.OffcCd, lDHypotheDetailsDTO.AssetCd, lDHypotheDetailsDTO.HypothNo, lDHypotheDetailsDTO.HypothDesc, lDHypotheDetailsDTO.AssetValue, lDHypotheDetailsDTO.ExecutionDate,
               lDHypotheDetailsDTO.HypothUpload, lDHypotheDetailsDTO.ApprovedEmpId, lDHypotheDetailsDTO.Action));
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Hypothecation));
                throw;
            }
        }

        #endregion

        #region Condition

        // <summary>
        //Author:Gagana; Module: condition ; Date: 13/08/2022
        // </summary>

        [HttpGet, Route(RouteName.GetAllConditionList)]
        public async Task<IActionResult> GetAllConditionListAsync(long accountNumber, CancellationToken token)

        {
            try
            {
                _logger.Information(Constants.GetAllConditionListAsync + accountNumber);
                var lst = await _legalDocumentationService.GetAllConditionListAsync(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllConditionListAsync + accountNumber + Constants.Result + lst);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Condition));
                throw;
            }
        }

        [HttpPost, Route(RouteName.CreateLDConditionDetails)]
        public async Task<IActionResult> CreateLDConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            try
            {
                if (CondtionDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.CreateStarted + " " + LogAttribute.ConditionDTO,
                                   CondtionDTO.CondDetId, CondtionDTO.ConditionType, CondtionDTO.CondDetails, CondtionDTO.ConditionStage));

                    var basicDetail = await _legalDocumentationService.CreateLDConditionDetails(CondtionDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.CreateCompleted + " " + LogAttribute.ConditionDTO,
                        CondtionDTO.CondDetId, CondtionDTO.ConditionType, CondtionDTO.CondDetails, CondtionDTO.ConditionStage));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Condition));
                throw;
            }
        }

        [HttpPost, Route(RouteName.DeleteLDConditionDetails)]
        public async Task<IActionResult> DeleteLDConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            try
            {
                if (CondtionDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.DeleteStarted + " " + LogAttribute.ConditionDTO,
                      CondtionDTO.CondDetId, CondtionDTO.ConditionType, CondtionDTO.CondDetails, CondtionDTO.ConditionStage));

                    var basicDetail = await _legalDocumentationService.DeleteLDCondtionDetails(CondtionDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.DeleteCompleted + " " + LogAttribute.ConditionDTO,
                       CondtionDTO.CondDetId, CondtionDTO.ConditionType, CondtionDTO.CondDetails, CondtionDTO.ConditionStage));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Condition));
                throw;
            }
        }

        [HttpPost, Route(RouteName.UpdateLDConditionDetails)]
        public async Task<IActionResult> UpdateLDConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token)
        {
            try
            {
                if (CondtionDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.ConditionDTO,
                   CondtionDTO.CondDetId, CondtionDTO.ConditionType, CondtionDTO.CondDetails, CondtionDTO.ConditionStage));

                    var basicDetail = await _legalDocumentationService.UpdateLDConditionDetails(CondtionDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.ConditionDTO,
                     CondtionDTO.CondDetId, CondtionDTO.ConditionType, CondtionDTO.CondDetails, CondtionDTO.ConditionStage));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.Condition));
                throw;
            }
        }
        #endregion 

        #region GuarantorDeed
        // <Summary>
        // Author: Akhiladevi D M; Module: GuarantorDeed; Date: 10/08/2022
        // <summary>
        [HttpGet, Route(RouteName.GetAllGuarantorList)]
        public async Task<IActionResult> GetAllGuarantorListAsync(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllGuarantorListAsync + accountNumber);
                var lst = await _legalDocumentationService.GetAllGuarantorListAsync(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllGuarantorListAsync + accountNumber + Constants.Result + lst);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.GuarantorDeed));
                throw;
            }
        }

        [HttpPost, Route(RouteName.UpdateLDGuarantorDeedDetails)]
        public async Task<IActionResult> UpdateLDGuarantorDeedDetails(IdmGuarantorDeedDetailsDTO lDGuarantorListDTO, CancellationToken token)
        {
            try
            {
                if (lDGuarantorListDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.GuarantorDeedDTO,
                 lDGuarantorListDTO.IdmGuarDeedId, lDGuarantorListDTO.ValueAsset, lDGuarantorListDTO.DeedDesc,  lDGuarantorListDTO.ExcecutionDate, lDGuarantorListDTO.DeedNo,
                 lDGuarantorListDTO.ValueLiab, lDGuarantorListDTO.ValueNetworth, lDGuarantorListDTO.LoanAcc));
                    var basicDetail = await _legalDocumentationService.UpdateLDGuarantorDeedDetails(lDGuarantorListDTO, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.GuarantorDeedDTO,
                 lDGuarantorListDTO.IdmGuarDeedId, lDGuarantorListDTO.ValueAsset, lDGuarantorListDTO.DeedDesc, lDGuarantorListDTO.ExcecutionDate, lDGuarantorListDTO.DeedNo,
                 lDGuarantorListDTO.ValueLiab, lDGuarantorListDTO.ValueNetworth, lDGuarantorListDTO.LoanAcc));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.GuarantorDeed));
                throw;
            }
        }
        
        #endregion
    }
}
