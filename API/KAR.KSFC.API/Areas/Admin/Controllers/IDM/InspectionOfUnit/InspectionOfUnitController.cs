using KAR.KSFC.API.Controllers;
using KAR.KSFC.API.Helpers;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.InspectionOfUnitModule;
using KAR.KSFC.API.ServiceFacade.Internal.Interface.UnitDetails.EnquirySubmission;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Math.EC.Multiplier;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Areas.Admin.Controllers.IDM.InspectionOfUnit
{
    public class InspectionOfUnitController : BaseApiController
    {
        private readonly IInspectionOfUnitService _inspectionOfUnitService;
        private readonly ILogger _logger;

        public InspectionOfUnitController(IInspectionOfUnitService inspectionOfUnitService, ILogger logger)
        {
            _inspectionOfUnitService = inspectionOfUnitService;
            _logger = logger;
        }
        //Author: Manoj Date:25/08/2022
        [HttpGet, Route(InspectionOfUnitRouteName.GetAccountNumber)]
        public async Task<IActionResult> GetAccountNumber(CancellationToken token)
        {
            _logger.Information(LogDetails.GetAccountNumber);
            var list = await _inspectionOfUnitService.GetAccountNumber(token).ConfigureAwait(false);
            _logger.Information(LogDetails.CompletedGetAccountNumber);
            return Ok(new ApiResultResponse(list, CommonLogHelpers.Success));
        }

        #region LandInspection 
        //Author: Manoj Date 25/08/2022
        [HttpGet, Route(InspectionOfUnitRouteName.GetAllLandInspectionList)]
        public async Task<IActionResult> GetAllLandInspectionList(long accountNumber, long InspectionId, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllLandInspectionList + accountNumber);
                var lst = await _inspectionOfUnitService.GetAllLandInspectionListAsync(accountNumber, InspectionId, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllLandInspectionList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LandInspection));
                throw;
            }
        }

        /// <summary>
        ///  Author: Manoj; Module: Land Inspection; Date:25/08/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.UpdateLandInspectionDetails)]
        public async Task<IActionResult> UpdateLandInspectionDetails(IdmDchgLandDetDTO landInspectionDTO, CancellationToken token)
        {
            try
            {
                if (landInspectionDTO != null   )
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.landInspectionDTO,
                 landInspectionDTO.DcLndRowId, landInspectionDTO.DcLndArea, landInspectionDTO.DcLndSecCreated, landInspectionDTO.DcLndType, landInspectionDTO.DcLndStatChgDate, landInspectionDTO.DcLndAmt ,landInspectionDTO.DcLndDevCst,landInspectionDTO.DcLndDets,landInspectionDTO.DcLndAqrdIndicator));

                    var basicDetail = await _inspectionOfUnitService.UpdateLandInspectionDetails(landInspectionDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.landInspectionDTO,
                 landInspectionDTO.DcLndRowId, landInspectionDTO.DcLndArea, landInspectionDTO.DcLndSecCreated, landInspectionDTO.DcLndType, landInspectionDTO.DcLndStatChgDate, landInspectionDTO.DcLndAmt, landInspectionDTO.DcLndDevCst, landInspectionDTO.DcLndDets,landInspectionDTO.DcLndAqrdIndicator));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LandInspection));
                throw;
            }
        }
        /// <summary>
        ///  Author: Manoj; Module: Land Inspection; Date:25/08/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.CreateLandInspectionDetails)]
        public async Task<IActionResult> CreateLandInspectionDetails(IdmDchgLandDetDTO landInspectionDTO, CancellationToken token)
        {
            try
            {
                if (landInspectionDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.CreateStarted + "" + LogAttribute.landInspectionDTO,
                landInspectionDTO.DcLndRowId, landInspectionDTO.DcLndArea, landInspectionDTO.DcLndSecCreated, landInspectionDTO.DcLndType, landInspectionDTO.DcLndStatChgDate, landInspectionDTO.DcLndAmt, landInspectionDTO.DcLndDevCst, landInspectionDTO.DcLndDets, landInspectionDTO.DcLndAqrdIndicator));

                    var basicDetail = await _inspectionOfUnitService.CreateLandInspectionDetails(landInspectionDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.CreateCompleted + "" + LogAttribute.landInspectionDTO,
                 landInspectionDTO.DcLndRowId, landInspectionDTO.DcLndArea, landInspectionDTO.DcLndSecCreated, landInspectionDTO.DcLndType, landInspectionDTO.DcLndStatChgDate, landInspectionDTO.DcLndAmt, landInspectionDTO.DcLndDevCst, landInspectionDTO.DcLndDets, landInspectionDTO.DcLndAqrdIndicator));
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LandInspection));
                throw;
            }
        }
        /// <summary>
        ///  Author: Manoj; Module: Land Inspection; Date:26/08/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.DeleteLandInspectionDetails)]
        public async Task<IActionResult> DeleteLandInspectionDetails(IdmDchgLandDetDTO landInspectionDTO, CancellationToken token)
        {
            try
            {
                if (landInspectionDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.landInspectionDTO,
                 landInspectionDTO.DcLndRowId, landInspectionDTO.DcLndAreaIn, landInspectionDTO.DcLndSecCreated, landInspectionDTO.DcLndType, landInspectionDTO.DcLndStatChgDate, landInspectionDTO.DcLndAmt, landInspectionDTO.DcLndDevCst, landInspectionDTO.DcLndDets, landInspectionDTO.DcLndAqrdIndicator));

                    var basicDetail = await _inspectionOfUnitService.DeleteLandInspectionDetails(landInspectionDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.landInspectionDTO,
                landInspectionDTO.DcLndRowId, landInspectionDTO.DcLndAreaIn, landInspectionDTO.DcLndSecCreated, landInspectionDTO.DcLndType, landInspectionDTO.DcLndStatChgDate, landInspectionDTO.DcLndAmt, landInspectionDTO.DcLndDevCst, landInspectionDTO.DcLndDets, landInspectionDTO.DcLndAqrdIndicator));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LandInspection));
                throw;
            }
        }
        #endregion 

        #region inspectionDetail
        
        [HttpGet, Route(InspectionOfUnitRouteName.GetAllInspectionDetailsList)]
        public async Task<IActionResult> GetAllInspectionDetailsList(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllInspectionDetailsList + accountNumber);
                var lst = await _inspectionOfUnitService.GetAllInspectionDetailsList(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllInspectionDetailsList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.InspectionDetails));
                throw;
            }
        }

        ///<sumary>
        /// Modified By: Dev Patel; Date: 21/10/2022 
        ///</sumary>
        [HttpPost, Route(InspectionOfUnitRouteName.UpdateInspectionDetails)]
        public async Task<IActionResult> UpdateInspectionDetails(IdmDspInspDTO InspectionDTO, CancellationToken token)
        {
            try
            {
                if (InspectionDTO != null)
                {
                    _logger.Information(LogAttribute.UpdateStarted + Module.InspectionDetails);

                    var basicDetail = await _inspectionOfUnitService.UpdateInspectionDetails(InspectionDTO, token).ConfigureAwait(false);

                    _logger.Information(LogAttribute.UpdateCompleted + Module.InspectionDetails);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.InspectionDetails));
                throw;
            }
        }

        [HttpPost, Route(InspectionOfUnitRouteName.CreateInspectionDetails)]
        public async Task<IActionResult> CreateLInspectionDetails(IdmDspInspDTO InspectionDTO, CancellationToken token)
        {
            try
            {
                if (InspectionDTO != null)
                {
                    _logger.Information(LogAttribute.CreateStarted + Module.InspectionDetails);

                    var basicDetail = await _inspectionOfUnitService.CreateInspectionDetails(InspectionDTO, token).ConfigureAwait(false);

                    _logger.Information(LogAttribute.CreateCompleted + Module.InspectionDetails);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.InspectionDetails));
                throw;
            }
        }

        ///<sumary>
        /// Modified By: Dev Patel; Date: 21/10/2022 
        ///</sumary>
        [HttpPost, Route(InspectionOfUnitRouteName.DeleteInspectionDetails)]
        public async Task<IActionResult> DeleteInspectionDetails(IdmDspInspDTO InspectionDTO, CancellationToken token)
        {
            try
            {
                if (InspectionDTO != null)
                {
                    _logger.Information(LogAttribute.DeleteStarted + Module.InspectionDetails);

                    var basicDetail = await _inspectionOfUnitService.DeleteInspectionDetails(InspectionDTO, token).ConfigureAwait(false);

                    _logger.Information(LogAttribute.DeleteCompleted + Module.InspectionDetails);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.InspectionDetails));
                throw;
            }
        }
        #endregion

        #region Building Inspection
        //Author: Swetha Date 25/08/2022
        [HttpGet, Route(InspectionOfUnitRouteName.GetAllBuildingInspectionList)]
        public async Task<IActionResult> GetAllBuildingInspectionList(long accountNumber, long InspectionId, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllBuildingInspectionList + accountNumber);
                var lst = await _inspectionOfUnitService.GetAllBuildingInspectionList(accountNumber, InspectionId, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllBuildingInspectionList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.BuildingInspectionDetails));
                throw;
            }
        }
        /// <summary>
        ///  Author: Swetha; Module: Building Inspection; Date:29/08/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.CreateBuildingInspectionDetails)]
        public async Task<IActionResult> CreateBuildingInspectionDetails(IdmDchgBuildingDetDTO buildingIspection, CancellationToken token)
        {
            try
            {
                if (buildingIspection != null)
                {
                    _logger.Information(LogAttribute.CreateStarted + Module.BuildingInspectionDetails);
                    var basicDetail = await _inspectionOfUnitService.CreateBuilidngInspectionDetails(buildingIspection, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.CreateCompleted + Module.BuildingInspectionDetails);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.BuildingInspectionDetails));
                throw;
            }
        }
        /// <summary>
        ///  Author: Swetha; Module: Building Inspection; Date:29/08/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.UpdateBuildingInspectionDetails)]
        public async Task<IActionResult> UpdateBuildingInspectionDetails(IdmDchgBuildingDetDTO buildingIspection, CancellationToken token)
        {
            try
            {
                if (buildingIspection != null)
                {
                    _logger.Information(LogAttribute.UpdateStarted + Module.BuildingInspectionDetails);
                    var basicDetail = await _inspectionOfUnitService.UpdateBuilidngInspectionDetails(buildingIspection, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.UpdateCompleted + Module.BuildingInspectionDetails);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.BuildingInspectionDetails));
                throw;
            }
        }
        /// <summary>
        ///  Author: Swetha; Module: Building Inspection; Date:29/08/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.DeleteBuildingInspectionDetails)]
        public async Task<IActionResult> DeleteBuildingInspectionDetails(IdmDchgBuildingDetDTO buildingIspection, CancellationToken token)
        {
            try
            {
                if (buildingIspection != null)
                {
                    _logger.Information(LogAttribute.DeleteStarted + Module.BuildingInspectionDetails);
                    var basicDetail = await _inspectionOfUnitService.DeleteBuilidngInspectionDetails(buildingIspection, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.DeleteCompleted + Module.BuildingInspectionDetails);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
                   
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));

                }

            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.BuildingInspectionDetails));
                throw;
            }
        }

        #endregion

        #region Working Capital
        /// <summary>
        ///  Author: Swetha; Module: Working Capital Inspection; Date:29/08/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.CreateWorkingCapitalInspection)]
        public async Task<IActionResult> CreateWorkingCapitalInspection(IdmDchgWorkingCapitalDTO workingCapital, CancellationToken token)
        {
            try
            {
                if (workingCapital != null)
                {
                    _logger.Information(LogAttribute.CreateStarted + Module.CreateWorkingCapitalInspection);
                    var basicDetail = await _inspectionOfUnitService.CreateWorkingCapitalInspectionDetails(workingCapital, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.CreateCompleted + Module.CreateWorkingCapitalInspection);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.CreateWorkingCapitalInspection));
                throw;
            }
        }

        #endregion

        #region Building Material at site Inspection
        //Author: Manoj Date 25/08/2022
        [HttpGet, Route(InspectionOfUnitRouteName.GetAllBuildingMaterialInspectionList)]
        public async Task<IActionResult> GetAllBuildingMaterialInspectionList(long accountNumber, long InspectionId, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllBuildingMaterialInspectionList + accountNumber);
                var lst = await _inspectionOfUnitService.GetAllBuildingMaterialInspectionListAsync(accountNumber, InspectionId, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllBuildingMaterialInspectionList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.BuildingMaterialInspection));
                throw;
            }
        }
        /// <summary>
        ///  Author: Manoj; Module: Building Material Inspection; Date:25/08/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.UpdateBuildMatSiteInspectionDetails)]
        public async Task<IActionResult> UpdateBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO bildMatInspectionDTO, CancellationToken token)
        {
            try
            {
                if (bildMatInspectionDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.UpdateStarted + "" + LogAttribute.bildMatInspectionDTO,
                 bildMatInspectionDTO.IrbmRowId, bildMatInspectionDTO.IrbmItem, bildMatInspectionDTO.IrbmMat, bildMatInspectionDTO.IrbmRate, bildMatInspectionDTO.IrbmQty));

                    var basicDetail = await _inspectionOfUnitService.UpdateBuildMatSiteInspectionDetails(bildMatInspectionDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.UpdateCompleted + "" + LogAttribute.bildMatInspectionDTO,
                     bildMatInspectionDTO.IrbmRowId, bildMatInspectionDTO.IrbmItem, bildMatInspectionDTO.IrbmMat, bildMatInspectionDTO.IrbmRate, bildMatInspectionDTO.IrbmQty));

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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.BuildingMaterialInspection));
                throw;
            }
        }

        /// <summary>
        ///  Author: Manoj; Module: BuildingMaterial Inspection; Date:25/08/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.CreateBuildMatSiteInspectionDetails)]
        public async Task<IActionResult> CreateBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO bildMatInspectionDTO, CancellationToken token)
        {
            try
            {
                if (bildMatInspectionDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.CreateStarted + "" + LogAttribute.bildMatInspectionDTO,
                bildMatInspectionDTO.IrbmRowId, bildMatInspectionDTO.IrbmItem, bildMatInspectionDTO.IrbmMat, bildMatInspectionDTO.IrbmRate, bildMatInspectionDTO.IrbmQty));

                    var basicDetail = await _inspectionOfUnitService.CreateBuildMatSiteInspectionDetails(bildMatInspectionDTO, token).ConfigureAwait(false);

                    _logger.Information(string.Format(LogAttribute.CreateCompleted + "" + LogAttribute.bildMatInspectionDTO,
                                     bildMatInspectionDTO.IrbmRowId, bildMatInspectionDTO.IrbmItem, bildMatInspectionDTO.IrbmMat, bildMatInspectionDTO.IrbmRate, bildMatInspectionDTO.IrbmQty));
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.BuildingMaterialInspection));
                throw;
            }
        }
        /// <summary>
        ///  Author: Manoj; Module: BuildingMaterial Inspection; Date:26/08/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.DeleteBuildMatSiteInspectionDetails)]
        public async Task<IActionResult> DeleteBuildMatSiteInspectionDetails(IdmBuildingMaterialSiteInspectionDTO bildMatInspectionDTO, CancellationToken token)
        {
            try
            {
                if (bildMatInspectionDTO != null)
                {
                    _logger.Information(string.Format(LogAttribute.DeleteStarted + "" + LogAttribute.bildMatInspectionDTO,
                 bildMatInspectionDTO.IrbmRowId, bildMatInspectionDTO.IrbmItem, bildMatInspectionDTO.IrbmMat, bildMatInspectionDTO.IrbmRate, bildMatInspectionDTO.IrbmQty));

                    var basicDetail = await _inspectionOfUnitService.DeleteBuildMatSiteInspectionDetails(bildMatInspectionDTO, token).ConfigureAwait(false);
                    
                    _logger.Information(string.Format(LogAttribute.DeleteCompleted + "" + LogAttribute.bildMatInspectionDTO,
                 bildMatInspectionDTO.IrbmRowId, bildMatInspectionDTO.IrbmItem, bildMatInspectionDTO.IrbmMat, bildMatInspectionDTO.IrbmRate, bildMatInspectionDTO.IrbmQty));

                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));

                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               

            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.BuildingMaterialInspection));
                throw;
            }
        }
        #endregion

        #region Indigenous machinery Inspection
        //Author: Manoj Date 01/09/2022
        [HttpGet, Route(InspectionOfUnitRouteName.GetAllIndigenousMachineryInspectionList)]
        public async Task<IActionResult> GetAllIndigenousMachineryInspectionList(long accountNumber, int InspectionId, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllIndigenousMachineryInspectionList + accountNumber);
                var lst = await _inspectionOfUnitService.GetAllIndigenousMachineryInspectionListAsync(accountNumber, InspectionId, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllIndigenousMachineryInspectionList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.IndigenousMachineryInspection));
                throw;
            }
        }

        [HttpGet, Route(InspectionOfUnitRouteName.GetAllMachineryStatusList)]

        public async Task<IActionResult> GetAllMachineryStatusList(CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllMachineryStatus);
                var lst = await _inspectionOfUnitService.GetAllMachineryStatusList(token).ConfigureAwait(false);
                _logger.Information(LogDetails.ConmpletedGetAllMachineryStatus);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.IndigenousMachineryInspection));
                throw;
            }
        }
         [HttpGet, Route(InspectionOfUnitRouteName.GetAllProcureList)]

        public async Task<IActionResult> GetAllProcureList(CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllProcureList);
                var lst = await _inspectionOfUnitService.GetAllProcureList(token).ConfigureAwait(false);
                _logger.Information(LogDetails.ConmpletedGetAllProcureList);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.IndigenousMachineryInspection));
                throw;
            }
        }
        


        /// <summary>
        ///  Author: Manoj; Module: Indigenous Machinery Inspection; Date:25/08/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.UpdateIndigenousMachineryInspectionDetails)]
        public async Task<IActionResult> UpdateIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO indigenousInspectionDTO, CancellationToken token)
        {
            try
            {
                if (indigenousInspectionDTO != null)
                {

                    _logger.Information(LogAttribute.UpdateStarted + Module.IndigenousMachineryInspection);
                    
                    var basicDetail = await _inspectionOfUnitService.UpdateIndigenousMachineryInspectionDetails(indigenousInspectionDTO, token).ConfigureAwait(false);

                    _logger.Information(LogAttribute.UpdateCompleted + Module.IndigenousMachineryInspection);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.IndigenousMachineryInspection));
                throw;
            }
        }

        /// <summary>
        ///  Author: Manoj; Module: Indigenous Machinery Inspection; Date:25/08/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.CreateIndigenousMachineryInspectionDetails)]
        public async Task<IActionResult> CreateIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO indigenousInspectionDTO, CancellationToken token)
        {
            try
            {
                if (indigenousInspectionDTO != null)
                {
                    _logger.Information(LogAttribute.CreateStarted + Module.IndigenousMachineryInspection);
                    var basicDetail = await _inspectionOfUnitService.CreateIndigenousMachineryInspectionDetails(indigenousInspectionDTO, token).ConfigureAwait(false);

                    _logger.Information(LogAttribute.CreateCompleted + Module.IndigenousMachineryInspection);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.IndigenousMachineryInspection));
                throw;
            }
        }
        /// <summary>
        ///  Author: Manoj; Module: BuildingMaterial Inspection; Date:26/08/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.DeleteIndigenousMachineryInspectionDetails)]
        public async Task<IActionResult> DeleteIndigenousMachineryInspectionDetails(IdmDchgIndigenousInspectionDTO indigenousInspectionDTO, CancellationToken token)
        {
            try
            {
                if (indigenousInspectionDTO != null)
                {
                    _logger.Information(LogAttribute.DeleteStarted + Module.IndigenousMachineryInspection);
                    var basicDetail = await _inspectionOfUnitService.DeleteIndigenousMachineryInspectionDetails(indigenousInspectionDTO, token).ConfigureAwait(false);

                    _logger.Information(LogAttribute.DeleteCompleted);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted + Module.IndigenousMachineryInspection));

                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
              
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.IndigenousMachineryInspection));
                throw;
            }
        }
        #endregion 

        #region Funrniture Inspection
        //Author: Sandeep M Date 30/08/2022
        [HttpGet, Route(InspectionOfUnitRouteName.GetAllFurnitureInspectionDetailsList)]
        public async Task<IActionResult> GetAllFurnitureInspectionDetailsList(long accountNumber, long InspectionId, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllFurnitureInspectionDetailsList + accountNumber);
                var lst = await _inspectionOfUnitService.GetAllFurnitureInspectionList(accountNumber, InspectionId, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllFurnitureInspectionDetailsList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.FurnitureInspection));
                throw;
            }
        }

        /// <summary>
        ///  Author: Manoj; Module: Furniture Inspection; Date:25/08/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.UpdateFurnitureInspectionDetails)]
        public async Task<IActionResult> UpdateFurnitureInspectionDetails(IdmDChgFurnDTO furnitureInspectionDTO, CancellationToken token)
        {
            try
            {
                if (furnitureInspectionDTO != null)
                {
                    _logger.Information(LogAttribute.UpdateStarted + Module.FurnitureInspection);
                    var basicDetail = await _inspectionOfUnitService.UpdateFurnitureInspectionDetails(furnitureInspectionDTO, token).ConfigureAwait(false);
                   
                    _logger.Information(LogAttribute.UpdateCompleted + Module.FurnitureInspection);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.FurnitureInspection));
                throw;
            }
        }

        /// <summary>
        ///  Author: Manoj; Module: Furniture Inspection; Date:25/08/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.CreateFurnitureInspectionDetails)]
        public async Task<IActionResult> CreateFurnitureInspectionDetails(IdmDChgFurnDTO furnitureInspectionDTO, CancellationToken token)
        {
            try
            {
                if (furnitureInspectionDTO != null)
                {
                    _logger.Information(LogAttribute.CreateStarted + Module.FurnitureInspection);

                    var basicDetail = await _inspectionOfUnitService.CreateFurnitureInspectionDetails(furnitureInspectionDTO, token).ConfigureAwait(false);

                    _logger.Information(LogAttribute.CreateCompleted + Module.FurnitureInspection);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.FurnitureInspection));
                throw;
            }
        }


        /// <summary>
        ///  Author: Manoj; Module: Land Inspection; Date:26/08/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.DeleteFurnitureInspectionDetails)]
        public async Task<IActionResult> DeleteFurnitureInspectionDetails(IdmDChgFurnDTO furnitureInspectionDTO, CancellationToken token)
        {
            try
            {
                if (furnitureInspectionDTO != null)
                {
                    _logger.Information(LogAttribute.DeleteStarted + Module.FurnitureInspection);
                    var basicDetail = await _inspectionOfUnitService.DeleteFurnitureInspectionDetails(furnitureInspectionDTO, token).ConfigureAwait(false);

                    _logger.Information(LogAttribute.DeleteCompleted);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted + Module.FurnitureInspection));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
                
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.FurnitureInspection));
                throw;
            }
        }
        #endregion

        #region Import Machinery Inspection
        //Author: Swetha Date:01/09/2022
        [HttpGet, Route(InspectionOfUnitRouteName.GetAllImportMachineryInspectionList)]
        public async Task<IActionResult> GetAllImportMachinerList(long accountNumber, long InspectionId, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllImportMachineryInspectionList + accountNumber);
                var lst = await _inspectionOfUnitService.GetAllImportMachineryList(accountNumber, InspectionId, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllImportMachineryInspectionList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ImportMachineryInspection));
                throw;
            }
        }
        /// <summary>
        ///  Author: Swetha; Module: Import Machinery Inspection; Date:29/08/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.CreateImportMachineryInspectionDetails)]
        public async Task<IActionResult> CreateImportMachineryList(IdmDchgImportMachineryDTO importMachinery, CancellationToken token)
        {
            try
            {
                if (importMachinery != null)
                {
                    _logger.Information(LogAttribute.CreateStarted + Module.ImportMachineryInspection);
                    var basicDetail = await _inspectionOfUnitService.CreateImportMachineryDetails(importMachinery, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.CreateCompleted + Module.ImportMachineryInspection);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ImportMachineryInspection));
                throw;
            }
        }
        /// <summary>
        ///  Author: Swetha; Module: Import Machinery Inspection; Date:29/08/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.UpdateImportMachineryInspectionDetails)]
        public async Task<IActionResult> UpdateImportMachineryList(IdmDchgImportMachineryDTO importMachinery, CancellationToken token)
        {
            try
            {
                if (importMachinery != null)
                {
                    _logger.Information(LogAttribute.UpdateStarted + Module.ImportMachineryInspection);
                    var basicDetail = await _inspectionOfUnitService.UpdateImportMachineryDetails(importMachinery, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.UpdateCompleted + Module.ImportMachineryInspection);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ImportMachineryInspection));
                throw;
            }
        }
        /// <summary>
        ///  Author: Swetha; Module: Building Inspection; Date:29/08/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.DeleteImportMachineryInspectionDetails)]
        public async Task<IActionResult> DeleteImportMachineryList(IdmDchgImportMachineryDTO importMachinery, CancellationToken token)
        {
            try
            {
                if (importMachinery != null)
                {
                    _logger.Information(LogAttribute.DeleteStarted + Module.ImportMachineryInspection);
                    var basicDetail = await _inspectionOfUnitService.DeleteImportMachineryDetails(importMachinery, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.DeleteCompleted + Module.ImportMachineryInspection);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                
              
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ImportMachineryInspection));
                throw;
            }
        }
        [HttpGet, Route(InspectionOfUnitRouteName.GetAllCurrencyList)]

        public async Task<IActionResult> GetAllCurrencyList(CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllCurrencyList);
                var lst = await _inspectionOfUnitService.GetAllCurrencyList(token).ConfigureAwait(false);
                _logger.Information(LogDetails.ConmpletedGetAllProcureList);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.IndigenousMachineryInspection));
                throw;
            }
        }
        #endregion 

        #region Letter Of Credit
        //Author: Manoj Date 05/09/2022
        [HttpGet, Route(InspectionOfUnitRouteName.GetAllLetterOfCreditDetailList)]
        public async Task<IActionResult> GetAllLetterOfCreditDetailList(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllLetterOfCreditDetailList + accountNumber);
                var lst = await _inspectionOfUnitService.GetAllLetterOfCreditDetailListAsync(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllLetterOfCreditDetailList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LetterOfCredit));
                throw;
            }
        }

        ///<sumary>
        /// Author: Manoj; Module: Letter Of Credit; Date 05/09/2022
        /// Modified By: Dev Patel; Added logger; Date: 21/10/2022
        ///</sumary>
        [HttpPost, Route(InspectionOfUnitRouteName.CreateLetterOfCreditDetails)]
        public async Task<IActionResult> CreateLetterOfCreditDetails(IdmDsbLetterOfCreditDTO letterOfCreditDetails, CancellationToken token)
        {
            try
            {
                if (letterOfCreditDetails != null)
                {
                    _logger.Information(LogAttribute.CreateStarted + Module.LetterOfCredit);
                    var basicDetail = await _inspectionOfUnitService.CreateLetterOfCreditDetails(letterOfCreditDetails, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.CreateCompleted + Module.LetterOfCredit);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LetterOfCredit));
                throw;
            }
        }

        ///<sumary>
        /// Author: Manoj; Module: Letter Of Credit; Date 05/09/2022
        /// Modified By: Dev Patel; Added logger; Date: 21/10/2022
        ///</sumary>
        [HttpPost, Route(InspectionOfUnitRouteName.UpdateLetterOfCreditDetails)]
        public async Task<IActionResult> UpdateLetterOfCreditDetails(IdmDsbLetterOfCreditDTO letterOfCreditDetails, CancellationToken token)
        {
            try
            {
                if (letterOfCreditDetails != null)
                {
                    _logger.Information(LogAttribute.UpdateStarted + Module.LetterOfCredit);
                    var basicDetail = await _inspectionOfUnitService.UpdateLetterOfCreditDetails(letterOfCreditDetails, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.UpdateCompleted + Module.LetterOfCredit);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LetterOfCredit));
                throw;
            }
        }

        ///<sumary>
        /// Author: Manoj; Module: Letter Of Credit; Date 05/09/2022
        /// Modified By: Dev Patel; Added logger; Date: 21/10/2022
        ///</sumary>
        [HttpPost, Route(InspectionOfUnitRouteName.DeleteLetterOfCreditDetails)]
        public async Task<IActionResult> DeleteLetterOfCreditDetails(IdmDsbLetterOfCreditDTO letterOfCreditDetails, CancellationToken token)
        {
            try
            {
                if (letterOfCreditDetails != null)
                {
                    _logger.Information(LogAttribute.DeleteStarted + Module.LetterOfCredit);
                    var basicDetail = await _inspectionOfUnitService.DeleteLetterOfCreditDetails(letterOfCreditDetails, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.DeleteCompleted + Module.LetterOfCredit);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));

                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }

            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.LetterOfCredit));
                throw;
            }
        }
        #endregion 

        #region Means Of Finance
        ///<sumary>
        /// Author: Swetha; Module: Means Of Finance; Date:01/09/2022
        /// Modified By: Dev Patel; Added logger; Date: 21/10/2022
        ///</sumary>
        [HttpGet, Route(InspectionOfUnitRouteName.GetAllMeansOfFinanceList)]
        public async Task<IActionResult> GetAllMeansFinanceDetails(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllMeansOfFinanceList + accountNumber);
                var lst = await _inspectionOfUnitService.GetAllMeansOfFinanceList(accountNumber,  token).ConfigureAwait(false);
                _logger.Information(LogDetails.GetAllMeansOfFinanceList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.MeansFinanceDetails));
                throw;
            }
        }

        ///<sumary>
        /// Author: Swetha; Module: Means Of Finance; Date:01/09/2022
        /// Modified By: Dev Patel; Added logger; Date: 21/10/2022
        ///</sumary>
        [HttpPost, Route(InspectionOfUnitRouteName.CreateMeansOfFinanceDetails)]
        public async Task<IActionResult> CreateMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance, CancellationToken token)
        {
            try
            {
                if (meansOfFinance != null)
                {
                    _logger.Information(LogAttribute.CreateStarted + Module.MeansFinanceDetails);
                    var basicDetail = await _inspectionOfUnitService.CreateMeansOfFinanceDetails(meansOfFinance, token).ConfigureAwait(false);

                    _logger.Information(LogAttribute.CreateCompleted + Module.MeansFinanceDetails);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + " " + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.MeansFinanceDetails));
                throw;
            }
        }

        /// <summary>
        ///  Author: Swetha; Module: Building Inspection; Date:29/08/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.UpdateMeansOfFinanceDetails)]
        public async Task<IActionResult> UpdateMeansOfFinanceList(IdmDchgMeansOfFinanceDTO meansOfFinance, CancellationToken token)
        {
            try
            {
                if (meansOfFinance != null)
                {
                    _logger.Information(LogAttribute.UpdateStarted + Module.MeansFinanceDetails);
                    var basicDetail = await _inspectionOfUnitService.UpdateMeansOfFinanceDetails(meansOfFinance, token).ConfigureAwait(false);

                    _logger.Information(LogAttribute.UpdateCompleted + Module.MeansFinanceDetails);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.MeansFinanceDetails));
                throw;
            }
        }

        /// <summary>
        ///  Author: Swetha; Module: Building Inspection; Date:29/08/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.DeleteMeansOfFinanceDetails)]
        public async Task<IActionResult> DeleteMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance, CancellationToken token)
        {
            try
            {
                if (meansOfFinance != null)
                {
                    _logger.Information(LogAttribute.DeleteStarted + Module.MeansFinanceDetails);
                    var basicDetail = await _inspectionOfUnitService.DeleteMeansOfFinanceDetails(meansOfFinance, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.DeleteCompleted + Module.MeansFinanceDetails);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
               
               
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.MeansFinanceDetails));
                throw;
            }
        }

        #endregion 

        #region Project Cost Details
        /// <summary>
        ///  Author: Akhila; Module: Project Cost; Date:05/09/2022
        /// </summary>

        [HttpGet, Route(InspectionOfUnitRouteName.GetAllProjectCostDetailsList)]
        public async Task<IActionResult> GetAllProjectCostDetailsList(long accountNumber, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllProjectCostDetailsList + accountNumber);
                var lst = await _inspectionOfUnitService.GetAllProjectCostDetailsList(accountNumber, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllProjectCostDetailsList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ProjectCostDetails));
                throw;
            }
        }
        /// <summary>
        ///  Author: Akhila; Module: Project Cost Details; Date:06/09/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.UpdateProjectCostDetails)]
        public async Task<IActionResult> UpdateProjectCostDetails(IdmDchgProjectCostDTO PrjCostDTO, CancellationToken token)
        {
            try
            {
                if (PrjCostDTO != null)
                {
                    _logger.Information(LogAttribute.UpdateStarted + Module.ProjectCostDetails);
                    var basicDetail = await _inspectionOfUnitService.UpdateProjectCostDetails(PrjCostDTO, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.UpdateCompleted + Module.ProjectCostDetails);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ProjectCostDetails));
                throw;
            }
        }

        /// <summary>
        ///  Author: Akhila; Module: Project Cost Details; Date:06/09/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.CreateProjectCostDetails)]
        public async Task<IActionResult> CreateProjectCostDetails(IdmDchgProjectCostDTO PrjCostDTO, CancellationToken token)
        {
            try
            {
                if (PrjCostDTO != null)
                {
                    _logger.Information(LogAttribute.CreateStarted + Module.ProjectCostDetails);
                    var basicDetail = await _inspectionOfUnitService.CreateProjectCostDetails(PrjCostDTO, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.CreateCompleted + Module.ProjectCostDetails);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ProjectCostDetails));
                throw;
            }
        }
        /// <summary>
        ///  Author: Akhila; Module: Project Cost Details; Date:06/09/2022
        ///  Modified By: Dev Patel; Added logger; Date: 21/10/2022
        /// </summary>
        [HttpPost, Route(InspectionOfUnitRouteName.DeleteProjectCostDetails)]
        public async Task<IActionResult> DeleteProjectCostDetails(IdmDchgProjectCostDTO PrjCostDTO, CancellationToken token)
        {
            try
            {
                if (PrjCostDTO != null)
                {
                    _logger.Information(LogAttribute.DeleteStarted + Module.ProjectCostDetails);
                    var basicDetail = await _inspectionOfUnitService.DeleteProjectCostDetails(PrjCostDTO, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.DeleteCompleted + Module.ProjectCostDetails);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }
                
              
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ProjectCostDetails));
                throw;
            }
        }
        #endregion

        #region StatusOfImplementation
        [HttpGet, Route(InspectionOfUnitRouteName.GetAllStatusofImplementationList)]
        public async Task<IActionResult> GetAllStatusofImplementation(long accountNumber, long InspectionId, CancellationToken token)
        {
            try
            {
                _logger.Information(LogDetails.GetAllStatusOfImplementationList + accountNumber);
                var lst = await _inspectionOfUnitService.GetAllStatusofImplementation(accountNumber, InspectionId, token).ConfigureAwait(false);
                _logger.Information(LogDetails.CompletedGetAllStatusOfImplementationList + accountNumber);
                return Ok(new ApiResultResponse(lst, CommonLogHelpers.Success));
            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.ImportMachineryInspection));
                throw;
            }
        }

        [HttpPost, Route(InspectionOfUnitRouteName.CreateStatusofImplementationDetails)]
        public async Task<IActionResult> CreateStatusOfImplementaion(IdmDsbStatImpDTO statusImp, CancellationToken token)
        {
            try
            {
                if (statusImp != null)
                {
                    _logger.Information(LogAttribute.CreateStarted + Module.StatusofImplementation);
                    var basicDetail = await _inspectionOfUnitService.CreateStatusofImplementation(statusImp, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.CreateCompleted + Module.StatusofImplementation);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.StatusofImplementation));
                throw;
            }
        }
        [HttpPost, Route(InspectionOfUnitRouteName.UpdateStatusofImplementationDetaiils)]
        public async Task<IActionResult> UpdateStatusofImplementation(IdmDsbStatImpDTO statusImp, CancellationToken token)
        {
            try
            {
                if (statusImp != null)
                {
                    _logger.Information(LogAttribute.UpdateStarted + Module.StatusofImplementation);
                    var basicDetail = await _inspectionOfUnitService.UpdateStatusofImplementation(statusImp, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.UpdateCompleted + Module.StatusofImplementation);
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
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.StatusofImplementation));
                throw;
            }
        }

        [HttpPost, Route(InspectionOfUnitRouteName.DeleteStatusofImplementationDetails)]
        public async Task<IActionResult> DeleteStatusofImplementation(IdmDsbStatImpDTO statusImp, CancellationToken token)
        {
            try
            {
                if (statusImp != null)
                {
                    _logger.Information(LogAttribute.DeleteStarted + Module.StatusofImplementation);
                    var basicDetail = await _inspectionOfUnitService.DeleteStatusofImplementation(statusImp, token).ConfigureAwait(false);
                    _logger.Information(LogAttribute.DeleteCompleted + Module.StatusofImplementation);
                    return Ok(new ApiResultResponse(200, basicDetail, CommonLogHelpers.Deleted));
                }
                else
                {
                    _logger.Information(ErrorMsg.ErrorCode400);
                    return new BadRequestObjectResult(new ApiResponse(400, ErrorMsg.Message));
                }


            }
            catch (System.Exception ex)
            {
                _logger.Error(string.Format(ErrorMsg.ErrorMsg1 + "" + ErrorMsg.ErrorMsg2 + ex.Message + Environment.NewLine + ErrorMsg.Stack + ex.StackTrace, Module.StatusofImplementation));
                throw;
            }
        }






        #endregion

    }
}