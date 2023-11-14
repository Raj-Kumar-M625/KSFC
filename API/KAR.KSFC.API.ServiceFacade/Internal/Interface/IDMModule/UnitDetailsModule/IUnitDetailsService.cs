using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.UnitDetails;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.UnitDetailsModule
{
    public interface IUnitDetailsService
    {
        #region Name Of Unit
        Task<IdmUnitDetailDTO> GetNameOfUnit(long accountNumber, CancellationToken token);
        Task<IdmUnitDetailDTO> UpdateNameOfUnit(IdmUnitDetailDTO idmUnitDetail, CancellationToken token);
        #endregion

        #region Change Location Address
        Task<IEnumerable<IdmUnitAddressDTO>> GetAllAddressDetails(long accountNumber, CancellationToken token);
        Task<IEnumerable<TblPincodeMasterDTO>> GetAllMasterPinCodeDetails(CancellationToken token);
        Task<IEnumerable<PincodeDistrictCdtabDTO>> GetAllPinCodeDistrictDetails(CancellationToken token);
        Task<bool> UpdateAddressDetails(IdmUnitAddressDTO PromAddressDto, CancellationToken token);

        Task<IEnumerable<TlqCdTabDTO>> GetAllTalukDetails(CancellationToken token);
        Task<IEnumerable<HobCdtabDTO>> GetAllHobliDetails(CancellationToken token);
        Task<IEnumerable<VilCdTabDTO>> GetAllVillageDetails(CancellationToken token);
        #endregion

        #region Promoter Profile
        Task<IEnumerable<IdmPromoterDTO>> GetAllPromoterProfileDetails(long? accountNumber, CancellationToken token); 
        Task<IEnumerable<TblPromcdtabDTO>> GetAllMasterPromoterProfileDetails(CancellationToken token); 
        Task<bool> CreatePromoterProfileDetails(IdmPromoterDTO PromprofileDto, CancellationToken token);
        Task<bool> UpdatePromoterProfileDetails(IdmPromoterDTO PromprofileDto, CancellationToken token);
        Task<bool> DeletePromoterProfileDetails(IdmPromoterDTO PromprofileDto, CancellationToken token);
        #endregion

        #region Promoter Bank
        Task<IEnumerable<IdmPromoterBankDetailsDTO>> GetAllPromoterBankInfo(long? accountNumber, CancellationToken token);
        Task<bool> CreatePromoterBankInfo(IdmPromoterBankDetailsDTO changePromoterBankDetail, CancellationToken token);
        Task<bool> UpdatePromoterBankInfo(IdmPromoterBankDetailsDTO changePromoterBankDetail, CancellationToken token);
        Task<bool> DeletePromoterBankInfo(IdmPromoterBankDetailsDTO changePromoterBankDetail, CancellationToken token);
        #endregion

        #region Promoter Address
        Task<IEnumerable<IdmPromAddressDTO>> GetAllPromoterAddressDetails(long? accountNumber, CancellationToken token);       
        Task<bool> UpdatePromAddressDetails(IdmPromAddressDTO PromAddressDto, CancellationToken token);
        Task<bool> CreatePromAddressDetails(IdmPromAddressDTO PromAddressDto, CancellationToken token);
        Task<bool> DeletePromAddressDetails(IdmPromAddressDTO PromAddressDto, CancellationToken token);
        #endregion

        #region ProductDetails
        Task<IEnumerable<IdmUnitProductsDTO>> GetAllProductDetails(long accountNumber, CancellationToken token);
        Task<IEnumerable<TblProdCdtabDTO>> GetAllProductList(CancellationToken token);
        Task<bool> CreateProductDetails(IdmUnitProductsDTO ProductDTO, CancellationToken token);
        Task<bool> UpdateProductDetails(IdmUnitProductsDTO ProductDTO, CancellationToken token);
        Task<bool> DeleteProductDetails(IdmUnitProductsDTO ProDetailsDto, CancellationToken token);
        #endregion

        #region Change Bank Details        
        Task<IEnumerable<IdmChangeBankDetailsDTO>> GetAllChangeBankDetailsList(long? accountNumber, CancellationToken token); 
        Task<IEnumerable<IfscMasterDTO>> GetAllIfscBankDetails( CancellationToken token);
        Task<bool> CreateChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail, CancellationToken token);
        Task<bool> UpdateChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail, CancellationToken token);
        Task<bool> DeleteChangeBankDetails(IdmChangeBankDetailsDTO changeBankDetail, CancellationToken token);
        #endregion

        #region AssetInformation
        Task<IEnumerable<IdmPromAssetDetDTO>> GetAllPromoterAssetDetails(long accountNumber, CancellationToken token);
        Task<IEnumerable<AssetTypeDetailsDTO>> GetAllAssetTypeDetails(CancellationToken token);
        Task<IEnumerable<TblAssetCatCDTabDTO>> GetAllAssetCategaryDetails(CancellationToken token);
        Task<bool> CreateAssetDetails(IdmPromAssetDetDTO AssetDTO, CancellationToken token);
        Task<bool> UpdateAssetDetails(IdmPromAssetDetDTO AssetDTO, CancellationToken token);
        Task<bool> DeleteAssetDetails(IdmPromAssetDetDTO AssetdetailsDTO, CancellationToken token);
        #endregion

        #region Promoter Liability Info
        Task<IEnumerable<TblPromoterLiabDetDTO>> GetAllPromoterLiabiltyInfo(long? accountNumber, CancellationToken token);
        Task<bool> CreatePromoterLiabiltyInfo(TblPromoterLiabDetDTO PromLiabDto, CancellationToken token);
        Task<bool> UpdatePromoterLiabiltyInfo(TblPromoterLiabDetDTO PromLiabDto, CancellationToken token);
        Task<bool> DeletePromoterLiabiltyInfo(TblPromoterLiabDetDTO PromLiabDto, CancellationToken token);
        #endregion

        #region Promoter NetWorth
        Task<IEnumerable<TblPromoterNetWortDTO>> GetAllPromoterNetWorth(long? accountNumber, CancellationToken token);
        Task<bool> SaveAssetNetworthDetails(TblPromoterNetWortDTO PromoterNWDetail, CancellationToken token);
        Task<bool> SaveLiabilityNetworthDetails(TblPromoterNetWortDTO PromoterNWDetail, CancellationToken token);
        #endregion

        #region Land Assets

        Task<IEnumerable<TblIdmProjLandDTO>> GetAllLandDetails(long accountNumber, CancellationToken token);

        Task<bool> CreateLandAssets(TblIdmProjLandDTO tblIdmProjLandDTO, CancellationToken token);


        #endregion

        #region Indegenous Asset
        Task<IEnumerable<TblIdmProjPlmcDTO>> GetAllIndegenousMachinaryAssets(long accountNumber, CancellationToken token);

        #endregion
    }
}
