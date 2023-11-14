using KAR.KSFC.Components.Common.Dto.Common;
using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Admin.IDM.IUnitDetailsService
{
    public interface IUnitDetailsService
    {

        #region Change Location Address
        Task<IEnumerable<IdmUnitAddressDTO>> GetAllAddressDetails(long accountNumber);
        Task<IEnumerable<TblPincodeMasterDTO>> GetAllMasterPinCodeDetails();
        Task<IEnumerable<PincodeDistrictCdtabDTO>> GetAllPinCodeDistrictDetails();
        Task<bool> UpdateAddressDetails(IdmUnitAddressDTO addr);
        Task<IEnumerable<TlqCdTabDTO>> GetAllTalukDetails();
        Task<IEnumerable<HobCdtabDTO>> GetAllHobliDetails();
        Task<IEnumerable<VilCdTabDTO>> GetAllVillageDetails();
        #endregion

        #region Name Of Unit
        Task<IdmUnitDetailDTO> GetUnitDetails(long? accountNumber);
        Task<IdmUnitDetailDTO> updateUnitName(IdmUnitDetailDTO idmUnitDetail);
        #endregion

        #region Promoter Profile
        Task<IEnumerable<TblPromcdtabDTO>> GetAllMasterPromoterProfileDetails();
        Task<IEnumerable<IdmPromoterDTO>> GetAllPromoterProfileDetails(long? accountNumber);
        Task<bool> CreatePromoterProfileDetails(IdmPromoterDTO pprofile);
        Task<bool> UpdatePromoterProfileDetails(IdmPromoterDTO pprofile);
        Task<bool> DeletePromoterProfileDetails(IdmPromoterDTO dto);

        #endregion

        #region PromoterAddress
        Task<IEnumerable<IdmPromAddressDTO>> GetAllPromoAddressDetails(long? accountNumber);        
        Task<bool> UpdatePromAddressDetails(IdmPromAddressDTO addr);
        Task<bool> CreatePromAddressDetails(IdmPromAddressDTO addr);
        Task<bool> DeletePromAddressDetails(IdmPromAddressDTO dto);

        #endregion

        #region Change Product Details 
        Task<IEnumerable<IdmUnitProductsDTO>> GetAllProductDetails(long accountNumber);
        Task<IEnumerable<TblProdCdtabDTO>> GetAllProductList();
        Task<bool> CreateProductDetails(IdmUnitProductsDTO pro);
        Task<bool> UpdateProductDetails(IdmUnitProductsDTO pro);
        Task<bool> DeleteProductDetails(IdmUnitProductsDTO pro);
        #endregion

        #region Generic dropdown
        Task<IEnumerable<DropDownDTO>> GetallProducdetailsMaster();

        Task<IEnumerable<DropDownDTO>> GetallIndustrydetailsMaster();

        Task<IEnumerable<DropDownDTO>> GetallAssetTypeMaster();
        Task<IEnumerable<DropDownDTO>> GetallAssetCategoryMaster();


        #endregion

        #region Promoter Bank
        Task<IEnumerable<IdmPromoterBankDetailsDTO>> GetAllPromoterBankInfo(long? accountNumber);
        Task<bool> CreatePromoterBankInfo(IdmPromoterBankDetailsDTO pbank);
        Task<bool> UpdatePromoterBankInfo(IdmPromoterBankDetailsDTO pbank);
        Task<bool> DeletePromoterBankInfo(IdmPromoterBankDetailsDTO dto);

        #endregion

        #region Change Bank Details
        Task<IEnumerable<IdmChangeBankDetailsDTO>> GetAllChangebankDetails(long accountNumber);
        Task<IEnumerable<IfscMasterDTO>> GetAllIfscbankDetails();
        Task<bool> CreateChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail);

        Task<bool> UpdateChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail);

        Task<bool> DeleteChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail);
        #endregion

        #region Promoter Asset
        Task<IEnumerable<IdmPromAssetDetDTO>> GetAllPromoterAssetDetails(long accountNumber);
        Task<bool> CreateAssetDetails(IdmPromAssetDetDTO pro);
        Task<bool> UpdateAssetDetails(IdmPromAssetDetDTO pro);
        Task<bool> DeleteAssetDetails(IdmPromAssetDetDTO pro);
        Task<IEnumerable<AssetTypeDetailsDTO>> GetAllAssetTypeDetails();
        Task<IEnumerable<TblAssetCatCDTabDTO>> GetAllAssetCategaryDetails();
        #endregion

        #region Promoter Liability Info
        Task<IEnumerable<TblPromoterLiabDetDTO>> GetAllPromoLiabilityInfo(long? accountNumber);
        Task<bool> UpdatePromoLiabilityInfo(TblPromoterLiabDetDTO addr);
        Task<bool> CreatePromoLiabilityInfo(TblPromoterLiabDetDTO addr);
        Task<bool> DeletePromoLiabilityInfo(TblPromoterLiabDetDTO dto);

        
        #endregion

        #region Promoter Net Worth
        Task<IEnumerable<TblPromoterNetWortDTO>> GetAllPromoterNetWorth(long? accountNumber);
        Task<bool> SaveAssetNetworthDetails(TblPromoterNetWortDTO pro);
        Task<bool> SaveLiabilityNetworthDetails(TblPromoterNetWortDTO pro);
        #endregion

        // US#05
        #region MeansOfFinance
        //Author: Swetha Date:01/09/2022
        Task<IEnumerable<IdmDchgMeansOfFinanceDTO>> GetAllMeansOfFinanceList(long accountNumber);

        Task<IEnumerable<SelectListItem>> GetFinanceTypeAsync();
        Task<bool> CreateMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance);
        Task<bool> UpdateMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance);
        Task<bool> DeleteMeansOfFinanceDetails(IdmDchgMeansOfFinanceDTO meansOfFinance);
        #endregion

        #region Project Cost
        //Author Akhila on 06/09/2022
        Task<IEnumerable<IdmDchgProjectCostDTO>> GetAllProjectCostDetailsList(long accountNumber);
        Task<bool> UpdateProjectCostDetails(IdmDchgProjectCostDTO ProjectCost);
        Task<bool> DeleteProjectCostDetails(IdmDchgProjectCostDTO dto);
        Task<bool> CreateProjectCostDetails(IdmDchgProjectCostDTO addr);
        #endregion

        //#region Letter Of Credit Details
        ////Author Manoj on 05/09/2022
        //Task<IEnumerable<IdmDsbLetterOfCreditDTO>> GetAllLetterOfCreditDetailsList(long accountNumber);
        //Task<bool> UpdateLetterOfCreditDetails(IdmDsbLetterOfCreditDTO letterOfCreditDetails);
        //Task<bool> DeleteLetterOfCreditDetails(IdmDsbLetterOfCreditDTO dto);
        //Task<bool> CreateLetterOfCreditDetails(IdmDsbLetterOfCreditDTO addr);
        //#endregion

        #region Working Capital
        Task<bool> CreateWorkingCapitalDetails(IdmDchgWorkingCapitalDTO workingCapital);

        #endregion

        #region Common DropDown
        Task<IEnumerable<DropDownDTO>> GetAllProjectCostComponentsDetails();

        #endregion

        //UC07

        #region Allocation
       
            Task<IEnumerable<TblIdmDhcgAllcDTO>> GetAllLoanAllocationList(long accountNumber);
            Task<bool> UpdateLoanAllocationDetails(TblIdmDhcgAllcDTO addr);
            Task<bool> CreateLoanAllocationDetails(TblIdmDhcgAllcDTO addr);
            Task<bool> DeleteLoanAllocationDetails(TblIdmDhcgAllcDTO dto);

        #endregion


        #region Land Assets
        Task<IEnumerable<TblIdmProjLandDTO>> GetAllProjLandDetailsList(long accountNumber);

        Task<bool> CreateLandAssets(TblIdmProjLandDTO tblIdmProjLand);
        #endregion
    }
}
