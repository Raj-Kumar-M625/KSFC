using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using KAR.KSFC.Components.Data.Models.DbModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Admin.IDM.IDisbursementService
{
    public interface IDisbursementService
    {

        #region Disbursement Condition
        // <summary>
        //  Author: Manoj; Module:Disbursement Conditions ; Date:18/08/2022
        // </summary>
        Task<IEnumerable<LDConditionDetailsDTO>> GetAllDisbursementConditionList(long accountNumber);
        Task<bool> UpdateDisbursementConditionDetails(LDConditionDetailsDTO addr);
        Task<bool> DeleteDisbursementConditionDetails(LDConditionDetailsDTO dto);
        Task<bool> CreateDisbursementConditionDetails(LDConditionDetailsDTO addr);
        #endregion

        #region Form 8 And Form 13 
        Task<IEnumerable<Form8AndForm13DTO>> GetAllForm8AndForm13List(long accountNumber);
        Task<bool> UpdateForm8AndForm13Details(Form8AndForm13DTO addr);
        Task<bool> DeleteForm8AndForm13Details(Form8AndForm13DTO dto);
        Task<bool> CreateForm8AndForm13Details(Form8AndForm13DTO addr);
        #endregion
 
        #region Sidbi Approval
        Task<IdmSidbiApprovalDTO> GetAllSidbiApprovalDetails(long? accountNumber);
        Task<bool> UpdateSidbiApprovalDetails(IdmSidbiApprovalDTO addr);
        #endregion

        #region additionalCondition
        Task<IEnumerable<AdditionConditionDetailsDTO>> GetAllAdditionalConditonlist(long accountNumber);
        Task<bool> UpdateAdditionalConditionDetails(AdditionConditionDetailsDTO addr);
        Task<bool> CreateAdditionalConditionDetails(AdditionConditionDetailsDTO addr);
        Task<bool> DeleteAdditionalConditionDetails(AdditionConditionDetailsDTO dto);
        #endregion

        #region FirstInvestmentClause
        Task<IdmFirstInvestmentClauseDTO> GetAllFirstInvestmentClauseDetails(long? accountNumber);
        Task<bool> UpdateFirstInvestmentClauseDetails(IdmFirstInvestmentClauseDTO addr);
        #endregion

        #region Other Relaxation
        Task<IEnumerable<RelaxationDTO>>GetAllOtherRelaxation(long accountNumber);
        Task<List<RelaxationDTO>> UpdateOtherRelaxation(List<RelaxationDTO> addr);
        #endregion
    }
}
