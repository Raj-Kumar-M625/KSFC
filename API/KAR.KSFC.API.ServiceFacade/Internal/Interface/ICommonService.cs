using KAR.KSFC.Components.Common.Dto.Common;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.Components.Data.Models.DbModels;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface
{
    public interface ICommonService
    {
        public Task<IEnumerable<DropDownDTO>> GetOfficeBranchAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetLoanPurposeAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetSizeOfFirmAsync(CancellationToken token);
       
        public Task<IEnumerable<DropDownDTO>> GetDistrictAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetNatureOfPremisesAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetTalukaAsync(int DistrictId, CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetHobliAsync(int TalukaId, CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetVillageAsync(int HobliId, CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetRegistrationTypeAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetPositionDesignationAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetConstituenttypeAsync(CancellationToken token);
        public Task<IEnumerable<ConstitutionMasterDTO>> GetAllConstitutionData(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetDomicileStatusAsync(CancellationToken token);
        
        public Task<IEnumerable<DropDownDTO>> GetModeofAcquireAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetNameOFFacilityAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetFinancialYearAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetFinancialComponentAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetPojectCostComponentAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetMeansofFinanceCategoryAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetFinanceTypeAsync(int mfCategory, CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetSecurityTypeAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetSecurityDetailsAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetRelationAsync(CancellationToken token);
        public Task<VillageTalukaHobliDistDTO> GetVillageTalukaHobliDistAsync(int VillageCode, CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAddressTypeAsync(CancellationToken token);
       
        public Task<IEnumerable<DropDownDTO>> GetAllPromotorClass(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllSecurityCategoryAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllSecurityTypesAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllSubRegistrarOfficeAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllBankIfscCode(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllChargeType(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllConditionTypes(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllConditionStages(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllConditionDesc(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllConditionStageMaster(CancellationToken token);

        public Task<IEnumerable<DropDownDTO>> GetAllForm8and13(CancellationToken token);
        #region Promoter Address
        public Task<IEnumerable<DropDownDTO>> GetAllPromoterState(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllPromoterNames(CancellationToken token);

        public Task<IEnumerable<DropDownDTO>> GetAllPromoterPhNo(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllPromoterDistrict(CancellationToken token);
        #endregion

        #region  Unit Information Address Details 
        public Task<IEnumerable<DropDownDTO>> GetAllDistrictDetails(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllTalukDetails(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllHobliDetails(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllVillageDetails(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllPincodeDetails(CancellationToken token);
        #endregion
        public Task<IEnumerable<DropDownDTO>> GetAllPromotorSubClas(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllPromotorQual(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllAccountType(CancellationToken token);

        public Task<IEnumerable<DropDownDTO>> GetNameOfProductAsync(CancellationToken token);

        public Task<IEnumerable<DropDownDTO>> GetIndustryType(CancellationToken token);

        public Task<IEnumerable<DropDownDTO>> GetAssetCategoryAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAssetTypeAsync(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllStateZone(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllAllocationCode(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllOtherDebitsDetails(CancellationToken token);

        #region Creation of Security and Acquisition of Assets
        public Task<IEnumerable<DropDownDTO>> GetAllLandType(CancellationToken token);
        #endregion

        public Task<IEnumerable<DropDownDTO>> GetAllUomMaster(CancellationToken token);

        public Task<IEnumerable<DropDownDTO>> GetAllFinanceType(CancellationToken token);
        public Task<IEnumerable<DropDownDTO>> GetAllDeptMaster(CancellationToken token);
       
        public Task<IEnumerable<DropDownDTO>> GetAllDsbChargeMaping(CancellationToken token);

    }   
}
