using KAR.KSFC.API.Helpers;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.CreationOfSecurityandAquisitionAsset;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.API.Controllers;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.CreationOfSecurityandAquisitionAsset;
using KAR.KSFC.Components.Data.Models.DbModels;

namespace KAR.KSFC.API.Areas.Admin.Controllers.IDM.CreationOfSecurityandAquisitionAsset
{
    public class CreationOfSecurityandAquisitionAssetController : BaseApiController
    {
        private readonly ICreationOfSecurityandAquisitionAsset _creationOfSecurityandAquisitionAssetService;        
        private readonly ILogger _logger;

        public CreationOfSecurityandAquisitionAssetController(ICreationOfSecurityandAquisitionAsset creationOfSecurityandAquisitionAssetService, ILogger logger)
        {
            _creationOfSecurityandAquisitionAssetService = creationOfSecurityandAquisitionAssetService;
            _logger = logger;
        }

        #region Land acquition
        /// <summary>
        ///  Author: Gowtham; Module: Land Acquisition; Date:28/09/2022
        ///  Modified: Dev Patel; Added logger; Date: 20/10/2022
        /// </summary>
        [HttpGet, Route(CreationOfSecurityandAquisitionAssetServiceRouteName.GetAllCreationOfSecurityandAquisitionAssetList)]
        public async Task<IActionResult> GetAllCreationOfSecurityandAquisitionAssetList(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllCreationOfSecurityandAquisitionAssetList + accountNumber);
                var lst = await _creationOfSecurityandAquisitionAssetService.GetAllCreationOfSecurityandAquisitionAssetList(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllCreationOfSecurityandAquisitionAssetList + accountNumber + Constants.Result + lst);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LandAcquisition));
                throw;
            }
        }

        /// <summary>
        ///  Author: Dev Patel; Module: Land Acquisition; Date:30/09/2022
        ///  Modified: Dev Patel; Added logger; Date: 20/10/2022
        /// </summary>
        [HttpPost, Route(CreationOfSecurityandAquisitionAssetServiceRouteName.UpadteLandAcquisitionDetails)]
        public async Task<IActionResult> UpadteLandAcquisitionDetails(TblIdmIrLandDTO LandDTO, CancellationToken token)
        {
            try
            {
                if (LandDTO != null)
                {

                    _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.LandDTO,
                    LandDTO.IrlId, LandDTO.IrlAreaIn, LandDTO.IrlSecValue, LandDTO.IrlRelStat));

                    var basicDetail = await _creationOfSecurityandAquisitionAssetService.UpadteLandAcquisitionDetails(LandDTO, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.LandDTO,
                    LandDTO.IrlId, LandDTO.IrlAreaIn, LandDTO.IrlSecValue, LandDTO.IrlRelStat));

                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

               
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LandAcquisition));
                throw;
            }
        }

      
        #endregion

        #region Machinery Acquisition
        /// <summary>
        ///  Author: Dev Patel; Module: Machinery Acquisition; Date:28/09/2022
        ///  Modified: Dev Patel; Added logger; Date: 20/10/2022
        /// </summary>
        [HttpGet, Route(CreationOfSecurityandAquisitionAssetServiceRouteName.GetAllMachineryAcquisitionDetails)]
        public async Task<IActionResult> GetAllMachineryAcquisitionDetails(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllMachineryAcquisitionDetails + accountNumber);

                var lst = await _creationOfSecurityandAquisitionAssetService.GetAllMachineryAcquisitionDetails(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllMachineryAcquisitionDetails + accountNumber + Constants.Result + lst);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.MachineryAcquisition));
                throw;
            }
        }

        [HttpPost, Route(CreationOfSecurityandAquisitionAssetServiceRouteName.UpadteMachineryAcquisitionDetails)]
        public async Task<IActionResult> UpadteMachineryAcquisitionDetails(IdmIrPlmcDTO MachineDTO, CancellationToken token)
        {
            try
            {
                if (MachineDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.MachineDTO,
                    MachineDTO.IrPlmcId, MachineDTO.IrPlmcAmt, MachineDTO.IrPlmcTotalRelease, MachineDTO.IrPlmcSecAmt, MachineDTO.IrPlmcAcqrdStatus, MachineDTO.IrPlmcRelseStat));

                    var basicDetail = await _creationOfSecurityandAquisitionAssetService.UpadteMachineryAcquisitionDetails(MachineDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.MachineDTO,
                    MachineDTO.IrPlmcId, MachineDTO.IrPlmcAmt, MachineDTO.IrPlmcTotalRelease, MachineDTO.IrPlmcSecAmt, MachineDTO.IrPlmcAcqrdStatus, MachineDTO.IrPlmcRelseStat));

                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
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

        #region Building Acquistion
        ///<summary>
        /// Author: Raman A; Module: Building Acquisition; Date:28/09/2022
        /// Modified: Dev Patel; Added logger; Date: 20/10/2022
        ///</summary>
        [HttpGet, Route(CreationOfSecurityandAquisitionAssetServiceRouteName.GetAllBuildingAcquisitionDetails)]
        public async Task<IActionResult> GetAllBuildingAcquisitionDetails(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllBuildingAcquisitionDetails + accountNumber);
                var lst = await _creationOfSecurityandAquisitionAssetService.GetAllBuildingAcquisitionDetails(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllBuildingAcquisitionDetails + accountNumber + Constants.Result + lst);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.BuildingAcquisition));
                throw;
            }
        }

        [HttpPost, Route(CreationOfSecurityandAquisitionAssetServiceRouteName.UpdateBuildingAcquisitionDetail)]
        public async Task<IActionResult> UpdateBuildingAcquisitionDetail(TblIdmBuildingAcquisitionDetailsDTO tblidmBuildingAcquisitionDetailsDTO, CancellationToken token)
        {
            try
            {
                if (tblidmBuildingAcquisitionDetailsDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.BuildingDTO,
                    tblidmBuildingAcquisitionDetailsDTO.Irbid, tblidmBuildingAcquisitionDetailsDTO.LoanAcc));

                    var basicDetail = await _creationOfSecurityandAquisitionAssetService.UpdateBuildingAcquisitionDetail(tblidmBuildingAcquisitionDetailsDTO, token).ConfigureAwait(false);
                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.BuildingDTO,
                    tblidmBuildingAcquisitionDetailsDTO.Irbid, tblidmBuildingAcquisitionDetailsDTO.LoanAcc));

                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Updated));
                }

                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
            }

            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.BuildingAcquisition));
                throw;
            }

         }


        #endregion

        #region Furniture acquition
        /// <summary>
        ///  Author: Kiran; Module: Furniture Acquisition; Date:30-Sep-2022
        ///  Modified: Dev Patel; Added logger; Date: 20/10/2022
        /// </summary>
        [HttpGet, Route(CreationOfSecurityandAquisitionAssetServiceRouteName.GetAllFurnitureAcquisitionList)]
        public async Task<IActionResult> GetAllFurnitureAcquisitionList(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(Constants.GetAllFurnitureAcquisitionList + accountNumber);
                var lst = await _creationOfSecurityandAquisitionAssetService.GetFurnitureAcquisitionDetailsList(accountNumber, token).ConfigureAwait(false);
                _logger.Information(Constants.CompletedGetAllFurnitureAcquisitionList + accountNumber + Constants.Result + lst);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.FurnitureAcquisition));
                throw;
            }
        }

        

        [HttpPost, Route(CreationOfSecurityandAquisitionAssetServiceRouteName.UpdateFurnitureAcuisitonDetails)]
        public async Task<IActionResult> UpdateFurnitureAcquisition(TblIdmIrFurnDTO FurnDTO, CancellationToken token)
        {
            try
            {
                if (FurnDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.FurnitureDto,
                    FurnDTO.IrfId, FurnDTO.IrfItem));

                    var FurnDetail = await _creationOfSecurityandAquisitionAssetService.UpdateFurnitureAcquisition(FurnDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.FurnitureDto,
                    FurnDTO.IrfId, FurnDTO.IrfItem));

                    return Ok(new ApiResultResponse(200, FurnDetail, CommonLogHelpers.Updated));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

                
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.FurnitureAcquisition));
                throw;
            }
        }
        #endregion
    }
}