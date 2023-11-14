using KAR.KSFC.API.Helpers;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.CreationOfDisbursmentProposalModule;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.API.Controllers;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal;
using KAR.KSFC.Components.Data.Repository.Interface;
using KAR.KSFC.Components.Data.Models.DbModels;
using AutoMapper;
using KAR.KSFC.Components.Data.Repository.UoW;
using System.Linq;

namespace KAR.KSFC.API.Areas.Admin.Controllers.IDM.CreationOfDisbursmentProposal
{
    public class CreationOfDisbursmentProposalController : BaseApiController
    {                
       private readonly ILogger _logger;        
       private readonly ICreationOfDisbursmentProposalService _creationOfDisbursmentProposalService;

        public CreationOfDisbursmentProposalController(ILogger logger, ICreationOfDisbursmentProposalService creationOfDisbursmentProposalService)
        {   
            _logger = logger;
            _creationOfDisbursmentProposalService = creationOfDisbursmentProposalService;
            
        }

        #region Recommended Disbursement Details
        ///<summary>
        /// Author: Raman A; Module: Recommended Disbursement Details; Date: 07/10/2022;
        /// Modified: Dev Patel; Added logger; Date: 21/10/2022;
        ///</summary>
        [HttpGet, Route(CreationOfDisbursmentProposalRouteName.GetAllRecomDisbursementDetails)]
        public async Task<IActionResult> GetAllRecomDisbursementDetails(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllRecomDisbursementDetails + accountNumber);
                var lst = await _creationOfDisbursmentProposalService.GetAllRecomDisbursementDetails(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllRecomDisbursementDetails + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.RecommendedDisbursement));
                throw;
            }
        }

        [HttpGet, Route(CreationOfDisbursmentProposalRouteName.GetAllocationCodeDetails)]
        public async Task<IActionResult> GetAllAllocationCodeDetails(CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllAllocationCodeDetails);
                var lst = await _creationOfDisbursmentProposalService.GetAllocationCodeDetails(token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllAllocationCodeDetails);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.RecommendedDisbursement));
                throw;
            }
        }

        [HttpGet, Route(CreationOfDisbursmentProposalRouteName.GetRecomDisbursementReleaseDetails)]
        public async Task<IActionResult> GetRecomDisbursementReleaseDetails(CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetRecomDisbursementReleaseDetails);  
                var lst = await _creationOfDisbursmentProposalService.GetRecomDisbursementReleaseDetails(token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetRecomDisbursementReleaseDetails);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));

            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.RecommendedDisbursement));
                throw;
            }
        }

        [HttpPost, Route(CreationOfDisbursmentProposalRouteName.UpdateRecomDisbursementDetail)]
        public async Task<IActionResult> UpdateRecomDisbursementDetail(IdmDsbdetsDTO idmDsbdetsDTO, CancellationToken token)
        {
            try
            {
                if (idmDsbdetsDTO != null)
                {

                    _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.RecomDisbursementDTO,
                 idmDsbdetsDTO.DsbdetsID, idmDsbdetsDTO.LoanAcc));

                    var basicDetail = await _creationOfDisbursmentProposalService.UpdateRecomDisbursementDetail(idmDsbdetsDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.RecomDisbursementDTO,
               idmDsbdetsDTO.DsbdetsID, idmDsbdetsDTO.LoanAcc));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.RecommendedDisbursement));
                throw;
            }
        }

        
        #endregion 

        #region disbursement proposal details 
        /// <summary>
        ///  Author: Gowtham; Module: Disburesment Proposal Details; Date:07/10/2022
        ///  Modified: Dev Patel; Added logger; Date:21/10/2022
        /// </summary>
        [HttpGet, Route(CreationOfDisbursmentProposalRouteName.GetAllProposalDetails)]
        public async Task<IActionResult> GetAllProposalDetails(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllProposalDetails + accountNumber);
                var lst = await _creationOfDisbursmentProposalService.GetAllProposalDetails(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllProposalDetails + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.DisbursementProposal));
                throw;
            }
        }
        [HttpPost, Route(CreationOfDisbursmentProposalRouteName.CreateProposalDetail)]
        public async Task<IActionResult> CreateProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsDTO, CancellationToken token)
        {

            try
            {
                if (tblIdmReleDetlsDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + " " + LogAttribute.RecomDisbursementDTO,
                tblIdmReleDetlsDTO.ReleId, tblIdmReleDetlsDTO.LoanAcc));

                    var basicDetail = await _creationOfDisbursmentProposalService.CreateProposalDetail(tblIdmReleDetlsDTO, token).ConfigureAwait(false);


                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + " " + LogAttribute.RecomDisbursementDTO,
                    tblIdmReleDetlsDTO.ReleId, tblIdmReleDetlsDTO.LoanAcc));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.DisbursementProposal));
                throw;
            }
        }

        [HttpPost, Route(CreationOfDisbursmentProposalRouteName.UpdateProposalDetail)]
        public async Task<IActionResult> UpdateProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsDTO, CancellationToken token)
        {
            try
            {
                if (tblIdmReleDetlsDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.RecomDisbursementDTO,
                tblIdmReleDetlsDTO.ReleId, tblIdmReleDetlsDTO.LoanAcc));

                    var basicDetail = await _creationOfDisbursmentProposalService.UpdateProposalDetail(tblIdmReleDetlsDTO, token).ConfigureAwait(false);


                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.RecomDisbursementDTO,
                    tblIdmReleDetlsDTO.ReleId, tblIdmReleDetlsDTO.LoanAcc));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.DisbursementProposal));
                throw;
            }
        }

        [HttpPost, Route(CreationOfDisbursmentProposalRouteName.DeleteProposalDetail)]
        public async Task<IActionResult> DeleteProposalDetail(TblIdmReleDetlsDTO tblIdmReleDetlsDTO, CancellationToken token)
        {
            try
            {
                if (tblIdmReleDetlsDTO != null)
                {

                    _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.RecomDisbursementDTO,
                 tblIdmReleDetlsDTO.ReleId, tblIdmReleDetlsDTO.LoanAcc));

                    var basicDetail = await _creationOfDisbursmentProposalService.DeleteProposalDetail(tblIdmReleDetlsDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.RecomDisbursementDTO,
                tblIdmReleDetlsDTO.ReleId, tblIdmReleDetlsDTO.LoanAcc));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.MachineryAcquisition));
                throw;
            }
        }
        #endregion

        #region Beneficiary Details
        /// <summary>
        ///  Author: Dev Patel; Module: Beneficiary Details; Date:07/10/2022
        ///  Modified: Dev Patel; Added logger; Date:21/10/2022
        /// </summary>
        [HttpGet, Route(CreationOfDisbursmentProposalRouteName.GetAllBeneficiaryDetails)]
        public async Task<IActionResult> GetAllBeneficiaryDetails(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllBeneficiaryDetails +accountNumber);
                var lst = await _creationOfDisbursmentProposalService.GetAllBeneficiaryDetails(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllBeneficiaryDetails + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.BeneficiaryDetails));
                throw;
            }
        }

        [HttpPost, Route(CreationOfDisbursmentProposalRouteName.UpdateBeneficiaryDetails)]
        public async Task<IActionResult> UpdateBeneficiaryDetails(TblIdmBenfDetDTO beneficiaryDet, CancellationToken token)
        {
            try
            {
                _logger.Information(LogAttribute.UpdateStarted);
                var basicDetails = await _creationOfDisbursmentProposalService.UpdateBeneficiaryDetails(beneficiaryDet, token).ConfigureAwait(false);
                if (beneficiaryDet == null)
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                _logger.Information(LogAttribute.UpdateCompleted);
                return Ok(new ApiResultResponse(200, basicDetails, CommonLogHelpers.Updated));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.BeneficiaryDetails));
                throw;
            }
        }
        #endregion

    }
}
