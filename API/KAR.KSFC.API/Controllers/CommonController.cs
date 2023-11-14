using KAR.KSFC.API.ServiceFacade.Internal.Interface;
using KAR.KSFC.Components.Common.Utilities.Errors;
using KAR.KSFC.Components.Common.Utilities.Response;

using Microsoft.AspNetCore.Mvc;

using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.Controllers
{
    public class CommonController : BaseApiController
    {
        private readonly ICommonService _commonService;
        public CommonController(ICommonService commonService)
        {
            _commonService = commonService;
        }

        [HttpGet, Route("GetOfficeBranch")]
        public async Task<IActionResult> GetOfficeBranchAsync(CancellationToken token)
        {
            var data = await _commonService.GetOfficeBranchAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));

        }

        [HttpGet, Route("GetLoanPurpose")]
        public async Task<IActionResult> GetLoanPurposeAsync(CancellationToken token)
        {
            var data = await _commonService.GetLoanPurposeAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));

        }

        [HttpGet, Route("GetNatureOfPremises")]
        public async Task<IActionResult> GetNatureOfPremisesAsync(CancellationToken token)
        {
            var data = await _commonService.GetNatureOfPremisesAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetSizeOfFirm")]
        public async Task<IActionResult> GetSizeOfFirmAsync(CancellationToken token)
        {
            var data = await _commonService.GetSizeOfFirmAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

      

        [HttpGet, Route("GetDistrict")]
        public async Task<IActionResult> GetDistrictAsync(CancellationToken token)
        {
            var data = await _commonService.GetDistrictAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetTaluka")]
        public async Task<IActionResult> GetTalukaAsync(int DistrictId, CancellationToken token)
        {
            var data = await _commonService.GetTalukaAsync(DistrictId, token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetHobli")]
        public async Task<IActionResult> GetHobliAsync(int TalukaId, CancellationToken token)
        {
            var data = await _commonService.GetHobliAsync(TalukaId, token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetVillage")]
        public async Task<IActionResult> GetVillageAsync(int HobliId, CancellationToken token)
        {
            var data = await _commonService.GetVillageAsync(HobliId, token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }


        [HttpGet, Route("GetRegistrationType")]
        public async Task<IActionResult> GetRegistrationTypeAsync(CancellationToken token)
        {
            var data = await _commonService.GetRegistrationTypeAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetPositionDesignation")]
        public async Task<IActionResult> GetPositionDesignationAsync(CancellationToken token)
        {
            var data = await _commonService.GetPositionDesignationAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }
        [HttpGet, Route("GetDomicileStatus")]
        public async Task<IActionResult> GetDomicileStatusAsync(CancellationToken token)
        {
            var data = await _commonService.GetDomicileStatusAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }


        [HttpGet, Route("GetModeofAcquireAsync")]
        public async Task<IActionResult> GetModeofAcquireAsync(CancellationToken token)
        {
            var data = await _commonService.GetModeofAcquireAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }


        [HttpGet, Route("GetNameOFFacilityAsync")]
        public async Task<IActionResult> GetNameOFFacilityAsync(CancellationToken token)
        {
            var data = await _commonService.GetNameOFFacilityAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetFinancialYearAsync")]
        public async Task<IActionResult> GetFinancialYearAsync(CancellationToken token)
        {
            var data = await _commonService.GetFinancialYearAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetFinancialComponentAsync")]
        public async Task<IActionResult> GetFinancialComponentAsync(CancellationToken token)
        {
            var data = await _commonService.GetFinancialComponentAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetPojectCostComponentAsync")]
        public async Task<IActionResult> GetPojectCostComponentAsync(CancellationToken token)
        {
            var data = await _commonService.GetPojectCostComponentAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetMeansofFinanceCategoryAsync")]
        public async Task<IActionResult> GetMeansofFinanceCategoryAsync(CancellationToken token)
        {
            var data = await _commonService.GetMeansofFinanceCategoryAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetFinanceTypeAsync")]
        public async Task<IActionResult> GetFinanceTypeAsync(int mfCategory, CancellationToken token)
        {
            var data = await _commonService.GetFinanceTypeAsync(mfCategory,token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }
        [HttpGet, Route("GetSecurityTypeAsync")]
        public async Task<IActionResult> GetSecurityTypeAsync(CancellationToken token)
        {
            var data = await _commonService.GetSecurityTypeAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetSecurityDetailsAsync")]
        public async Task<IActionResult> GetSecurityDetailsAsync(CancellationToken token)
        {
            var data = await _commonService.GetSecurityDetailsAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetRelationAsync")]
        public async Task<IActionResult> GetRelationAsync(CancellationToken token)
        {
            var data = await _commonService.GetRelationAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }



        /// <summary>
        /// Get all constitution types list.
        /// </summary>
        /// <returns></returns>

        [HttpGet, Route("GetConstitution")]
        public async Task<IActionResult> GetConstitutionType(CancellationToken token)
        {
            var lst = await _commonService.GetConstituenttypeAsync(token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(lst, "Success"));
        }

        /// <summary>
        /// GetAllConstitutionData
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, Route("GetAllConstitutionData")]
        public async Task<IActionResult> GetAllConstitutionData(CancellationToken token)
        {
            var lst = await _commonService.GetAllConstitutionData(token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(lst, "Success"));
        }

        /// <summary>
        /// Get Address Types
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, Route("GetAllAddressType")]
        public async Task<IActionResult> GetAllAddressType(CancellationToken token)
        {
            var lst = await _commonService.GetAddressTypeAsync(token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(lst, "Success"));
        }

        [HttpGet, Route("GetAllPromotorClass")]
        public async Task<IActionResult> GetAllPromotorClass(CancellationToken token)
        {
            var lst = await _commonService.GetAllPromotorClass(token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(lst, "Success"));
        }

        /// <summary>
        /// Get all security category list.
        /// </summary>
        /// <returns></returns>

        [HttpGet, Route("GetAllSecurityCategory")]
        public async Task<IActionResult> GetAllSecurityCategory(CancellationToken token)
        {
            var lst = await _commonService.GetAllSecurityCategoryAsync(token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(lst, "Success"));
        }

        /// <summary>
        /// Get all security type list.
        /// </summary>
        /// <returns></returns>

        [HttpGet, Route("GetAllSecurityTypes")]
        public async Task<IActionResult> GetAllSecurityTypes(CancellationToken token)
        {
            var lst = await _commonService.GetAllSecurityTypesAsync(token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(lst, "Success"));
        }

        
        /// <summary>
        /// Get all sub registrar office list.
        /// </summary>
        /// <returns></returns>

        [HttpGet, Route("GetAllSubRegistrarOffice")]
        public async Task<IActionResult> GetAllSubRegistrarOffice(CancellationToken token)
        {
            var lst = await _commonService.GetAllSubRegistrarOfficeAsync(token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(lst, "Success"));
        }

        [HttpGet,Route("GetAllBankIFSCCode")]

        public async Task<IActionResult> GetAllBankIfscCode(CancellationToken token)
        {
            var lst = await _commonService.GetAllBankIfscCode(token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(lst, "Success"));
        }


        [HttpGet, Route("GetAllChargeType")]

        public async Task<IActionResult> GetAllChargeType(CancellationToken token)
        {
            var lst = await _commonService.GetAllChargeType(token).ConfigureAwait(false);            
            return Ok(new ApiResultResponse(lst, "Success"));
        }

        [HttpGet, Route("GetAllConditionTypes")]
        public async Task<IActionResult> GetAllConditionTypeAsync(CancellationToken token)
        {
            var data = await _commonService.GetAllConditionTypes(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetAllConditionStages")]
        public async Task<IActionResult> GetAllConditionStageAsync(CancellationToken token)
        {
            var data = await _commonService.GetAllConditionStages(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetAllConditionStageMaster")]
        public async Task<IActionResult> GetAllConditionStageMaster(CancellationToken token)
        {
            var data = await _commonService.GetAllConditionStageMaster(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }


        [HttpGet, Route("GetAllConditionDescriptions")]
        public async Task<IActionResult> GetAllConditionDescAsync(CancellationToken token)
        {
            var data = await _commonService.GetAllConditionDesc(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetAllForm8AndForm13Master")]
        public async Task<IActionResult> GetAllForm8AndForm13Master(CancellationToken token)
        {
            var data = await _commonService.GetAllForm8and13(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }
        #region Promoter Address
        [HttpGet, Route("GetAllPromoterState")]
        public async Task<IActionResult> GetAllPromoterStateAsync(CancellationToken token)
        {
            var data = await _commonService.GetAllPromoterState(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }
        [HttpGet, Route("GetAllPromoterNames")]
        public async Task<IActionResult> GetAllPromoterNamesAsync(CancellationToken token)
        {
            var data = await _commonService.GetAllPromoterNames(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }
        [HttpGet, Route("GetAllPromoterPhNo")]
        public async Task<IActionResult> GetAllPhNoAsync(CancellationToken token)
        {
            var data = await _commonService.GetAllPromoterPhNo(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }



        [HttpGet, Route("GetAllPromoterDistrict")]
        public async Task<IActionResult> GetAllPromoterDistrictAsync(CancellationToken token)
        {
            var data = await _commonService.GetAllPromoterDistrict(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }
        #endregion
        #region  Unit Information Address Details 

        [HttpGet, Route("GetAllDistrictDetails")]
        public async Task<IActionResult> GetAllDistrictDetails(CancellationToken token)
        {
            var data = await _commonService.GetAllDistrictDetails(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetAllTalukDetails")]
        public async Task<IActionResult> GetAllTalukDetails(CancellationToken token)
        {
            var data = await _commonService.GetAllTalukDetails(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetAllHobliDetails")]
        public async Task<IActionResult> GetAllHobliDetails(CancellationToken token)
        {
            var data = await _commonService.GetAllHobliDetails(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetAllVillageDetails")]
        public async Task<IActionResult> GetAllVillageDetails(CancellationToken token)
        {
            var data = await _commonService.GetAllVillageDetails(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }


        [HttpGet, Route("GetAllPincodeDetails")]
        public async Task<IActionResult> GetAllPincodeDetails(CancellationToken token)
        {
            var data = await _commonService.GetAllPincodeDetails(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        #endregion
        /// <summary>
        /// Get all Promoter Sub Class list.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllPromotorSubClass")]
        public async Task<IActionResult> GetPromotorSubClasAsync(CancellationToken token)
        {
            var data = await _commonService.GetAllPromotorSubClas(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }



        /// <summary>
        /// Get all Promoter Qualification list.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllPromotorQualification")]
        public async Task<IActionResult> GetPromotorQualAsync(CancellationToken token)
        {
            var data = await _commonService.GetAllPromotorQual(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetProductName")]
        public async Task<IActionResult> GetProductNameAsync(CancellationToken token)
        {
            var data = await _commonService.GetNameOfProductAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        /// <summary>
        ///  Get industry Types
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet, Route("GetAllIndustrytype")]
        public async Task<IActionResult> GetAllIndustrytype(CancellationToken token)
        {
            var lst = await _commonService.GetIndustryType(token).ConfigureAwait(false);
            return Ok(new ApiResultResponse(lst, "Success"));
        }

        [HttpGet, Route("GetAllAccountType")]
        public async Task<IActionResult> GetAccountTypeAsync(CancellationToken token)
        {
            var data = await _commonService.GetAllAccountType(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetAssetCategoryAsync")]
        public async Task<IActionResult> GetAssetCategoryAsync(CancellationToken token)
        {
            var data = await _commonService.GetAssetCategoryAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetAllAssetTypes")]
        public async Task<IActionResult> GetAssetTypeAsync(CancellationToken token)
        {
            var data = await _commonService.GetAssetTypeAsync(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetAllStateZone")]
        public async Task<IActionResult> GetAllStateZone(CancellationToken token)
        {
            var data = await _commonService.GetAllStateZone(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }


        [HttpGet, Route("GetAllAllocationCode")]
        public async Task<IActionResult> GetAllAllocationCode(CancellationToken token)
        {
            var data = await _commonService.GetAllAllocationCode(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }


        [HttpGet, Route("GetAllOtherDebitsDetails")]
        public async Task<IActionResult> GetAllOtherDebitsDetails(CancellationToken token)
        {
            var data = await _commonService.GetAllOtherDebitsDetails(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        #region Creation of Security and Acquisition of Assets
        /// <summary>
        /// Get all Land Type list.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("GetAllLandType")]
        public async Task<IActionResult> GetAllLandType(CancellationToken token)
        {
            var data = await _commonService.GetAllLandType(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }
        #endregion


        [HttpGet, Route("GetAllUomMaster")]
        public async Task<IActionResult> GetAllUomMaster(CancellationToken token)
        {
            var data = await _commonService.GetAllUomMaster(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

         [HttpGet, Route("GetAllFinanceType")]
        public async Task<IActionResult> GetAllFinanceType(CancellationToken token)
        {
            var data = await _commonService.GetAllFinanceType(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

        [HttpGet, Route("GetAllDeptMaster")]
        public async Task<IActionResult> GetAllDeptMaster(CancellationToken token)
        {
            var data = await _commonService.GetAllDeptMaster(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }
       
         [HttpGet, Route("GetAllDsbChargeMaping")]
        public async Task<IActionResult> GetAllDsbChargeMaping(CancellationToken token)
        {
            var data = await _commonService.GetAllDsbChargeMaping(token).ConfigureAwait(false);
            if (data == null)
            {
                return new BadRequestObjectResult(new ApiResponse(400, "Something went Wrong"));
            }
            return Ok(new ApiResultResponse(200, data, "Success"));
        }

    }
}
