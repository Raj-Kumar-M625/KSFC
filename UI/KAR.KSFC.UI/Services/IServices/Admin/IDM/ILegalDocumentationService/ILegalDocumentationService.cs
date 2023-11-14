using KAR.KSFC.Components.Common.Dto.IDM;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Admin.IDM.ILegalDocumentationService
{
    public interface ILegalDocumentationService
    {
        #region Primary/CollateralSecurity
        // <summary>
        //  Author: Rajesh; Module: Primary/CollateralSecurity; Date:15/07/2022
        // </summary>
        Task<IEnumerable<IdmSecurityDetailsDTO>> GetAllPrimaryCollateralList(long accountNumber);
        Task<bool> UpdatePrimaryCollateralDetails(IdmSecurityDetailsDTO addr);
        #endregion



        #region CollateralSecurity

        Task<IEnumerable<IdmSecurityDetailsDTO>> GetAllCollateralList(long accountNumber);
        Task<bool> UpdateCollateralDetails(IdmSecurityDetailsDTO addr);

        Task<bool> CreateCollateralDetails(IdmSecurityDetailsDTO addr);
        #endregion


        #region Hypothecation
        // <summary>
        //  Author:Manoj; Module: Hypothecation; Date:21/07/2022
        // </summary>  
        Task<IEnumerable<HypoAssetDetailDTO>> GetAllHypothecationList(long accountNumber,string paramater);
        Task<bool> UpdateHypothecationDetails(IdmHypotheDetailsDTO addr);
        Task<bool> DeleteHypothecationDetail(IdmHypotheDetailsDTO dto);
        Task<bool> CreateHypothecationDetails(List<IdmHypotheDetailsDTO> addr);
        Task<IEnumerable<AssetRefnoDetailsDTO>> GetAllAssetRefList(long accountNumber);
        #endregion

        #region SecurityCharge
        // <summary>
        //  Author: Sandeep; Module: SecurityCharge; Date:21/07/2022
        // </summary>
        Task<IEnumerable<IdmSecurityChargeDTO>> GetAllSecurityChargeList(long accountNumber);
        Task<bool> UpdateSecurityCharge(IdmSecurityChargeDTO addr);
        #endregion

        #region CERSAI
        // <summary>
        // Author: Gagana K; Module: CERSAIRegistration; Date:03/08/2022
        // </summary>
        Task<IEnumerable<IdmCersaiRegDetailsDTO>> GetAllCERSAIList(long accountNumber, string parameter);
        Task<bool> CreateLDCersaiRegDetails(IdmCersaiRegDetailsDTO addr);
        Task<bool> DeleteLDCersaiRegDetails(IdmCersaiRegDetailsDTO dto);
        #endregion

        #region GuarantorDeed
        // <Summary>
        // Author: Akhiladevi D M; Module: GuarantorDeed; Date: 10/08/2022
        // <summary>
        Task<IEnumerable<IdmGuarantorDeedDetailsDTO>> GetAllGuarantorDeedList(long accountNumber);
        Task<bool> UpdateLDGuarantorDeedDetails(IdmGuarantorDeedDetailsDTO addr);
        #endregion

        #region Condition
        // <summary>
        //  Author: Gagana K; Module:Conditions ; Date:11/08/2022
        // </summary>
        Task<IEnumerable<LDConditionDetailsDTO>> GetAllConditionList(long accountNumber);
        Task<bool> UpdateLDConditionDetails(LDConditionDetailsDTO addr);
        Task<bool> DeleteLDConditionDetails(LDConditionDetailsDTO dto);
        Task<bool> CreateLDConditionDetails(LDConditionDetailsDTO addr);

        #endregion
    }
}
