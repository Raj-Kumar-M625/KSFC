using KAR.KSFC.Components.Common.Dto.IDM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.LegalDocumentationModule
{
    public interface ILegalDocumentationService
    {
        #region Primary Security

        /// <summary>
        ///  Author: Rajesh; Module: Primary/CollateralSecurity; Date:15/07/2022
        /// </summary>
        Task<IEnumerable<IdmSecurityDetailsDTO>> GetPrimaryCollateralListAsync(long accountNumber, CancellationToken token);
        Task<bool> UpdatePrimaryCollateralDetail(IdmSecurityDetailsDTO lDSecurityDetailsDTO, CancellationToken token);

        #endregion

        #region Colletral Security

        Task<IEnumerable<IdmSecurityDetailsDTO>> GetCollateralListAsync(long accountNumber, CancellationToken token);
        Task<bool> UpdateCollateralDetail(IdmSecurityDetailsDTO lDSecurityDetailsDTO, CancellationToken token);

        Task<bool> CreateCollateralDetail(IdmSecurityDetailsDTO lDSecurityDetailsDTO, CancellationToken token);

        #endregion

        #region Hypothecation
        /// <summary>
        ///  Author:Manoj; Module: Hypothecation; Date:21/07/2022
        /// </summary>    
        Task<IEnumerable<HypoAssetDetailDTO>> GetAllHypothecationAsync(long accountNumber, string paramater, CancellationToken token);
        Task<IEnumerable<AssetRefnoDetailsDTO>> GetAllAssetRefListAsync(long accountNumber, CancellationToken token);
        Task<bool> UpdateLDHypothecationDetails(IdmHypotheDetailsDTO lDHypotheDetailsDTO, CancellationToken token);
        Task<bool> DeleteHypothecationDetails(IdmHypotheDetailsDTO lDHypotheDetailsDTO, CancellationToken token);
        Task<bool> CreateLDHypothecationDetails(IdmHypotheDetailsDTO lDHypotheDetailsDTO,bool createHypothMap, CancellationToken token);
        #endregion

        #region CERSAI
        /// <summary>
        /// // Author: Gagana K; Module: CERSAIRegistration; Date:28/07/2022
        /// </summary>
        Task<IEnumerable<IdmCersaiRegDetailsDTO>> GetAllCERSAIRegistrationAsync(long accountNumber,string parameter, CancellationToken token);
        Task<bool> CreateLDCersaiRegDetails(IdmCersaiRegDetailsDTO CersaiRegDTO, CancellationToken token);
        Task<bool> DeleteLDCersaiDetails(IdmCersaiRegDetailsDTO CersaiRegDTO, CancellationToken token);
        #endregion

        #region Condition
        /// <summary>
        ///  Author: Gagana K; Module:Conditions ; Date:01/08/2022
        /// </summary>
        Task<IEnumerable<LDConditionDetailsDTO>> GetAllConditionListAsync(long accountNumber, CancellationToken token);
        Task<bool> DeleteLDCondtionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token);
        Task<bool> UpdateLDConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token);
        Task<bool> CreateLDConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token);
        #endregion

        #region SecurityCharge
        /// <summary>
        ///  Author: Sandeep; Module: SecurityCharge; Date:21/07/2022
        /// </summary>
        Task<IEnumerable<IdmSecurityChargeDTO>> GetAllSecurityChargeAsync(long accountNumber, CancellationToken token);
        Task<bool> UpdateSecurityChargeDetails(IdmSecurityChargeDTO lDSecurityDetailsDTO, CancellationToken token);
       
        #endregion

        #region GuarantorDeed
        // <Summary>
        // Author: Akhiladevi D M; Module: GuarantorDeed; Date: 10/08/2022
        // <summary>
        Task<bool> UpdateLDGuarantorDeedDetails(IdmGuarantorDeedDetailsDTO GuarantorListDTO, CancellationToken token);
        Task<IEnumerable<IdmGuarantorDeedDetailsDTO>> GetAllGuarantorListAsync(long accountNumber, CancellationToken token);
       
        #endregion
    }
}



