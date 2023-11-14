using KAR.KSFC.Components.Common.Dto.IDM;
using KAR.KSFC.Components.Common.Dto.IDM.Disbursement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.IDMModule.DisbursementModule
{
    public interface IDisbursementService
    {
        Task<IEnumerable<LoanAccountNumberDTO>> GetAccountNumber(CancellationToken token);
        #region Disbursement Condition
        Task<IEnumerable<LDConditionDetailsDTO>> GetDisbursementConditionListAsync(long accountNumber, CancellationToken token);
        Task<bool> DeleteDisbursementCondtionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token);
        Task<bool> UpdateDisbursementConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token);
        Task<bool> CreateDisbursementConditionDetails(LDConditionDetailsDTO CondtionDTO, CancellationToken token);
        #endregion

        #region Addition Condition
        Task<IEnumerable<AdditionConditionDetailsDTO>> GetAdditionConditionListAsync(long accountNumber, CancellationToken token);
        Task<bool> UpdateAdditionalConditionDetails(AdditionConditionDetailsDTO CondtionDTO, CancellationToken token);
        Task<bool> CreateAdditionalConditionDetails(AdditionConditionDetailsDTO CondtionDTO, CancellationToken token);
        Task<bool> DeleteAdditionalCondtionDetails(AdditionConditionDetailsDTO CondtionDTO, CancellationToken token);
        #endregion

        #region Form8AndForm13
        Task<IEnumerable<Form8AndForm13DTO>> GetAllForm8AndForm13ListAsync(long accountNumber, CancellationToken token);

        Task<bool> UpdateForm8AndForm13Details(Form8AndForm13DTO form8and13DTO, CancellationToken token);
        Task<bool> CreateForm8AndForm13Details(Form8AndForm13DTO form8and13DTO, CancellationToken token);
        Task<bool> DeleteForm8AndForm13Details(Form8AndForm13DTO form8and13DTO, CancellationToken token);

        #endregion

        #region Sidbi Approval
        Task<IdmSidbiApprovalDTO> GetSidbiApprovalAsync(long accountNumber, CancellationToken token);
        Task<bool> UpdateSidbiApprovalDetails(IdmSidbiApprovalDTO sidbi, CancellationToken token);
        #endregion

        #region FirstInvestmentClause
        Task<IdmFirstInvestmentClauseDTO> GetFirstInvestmentClauseAsync(long accountNumber, CancellationToken token);
        Task<bool> UpdateAllFirstInvestmentClauseDetails(IdmFirstInvestmentClauseDTO idmFICDetail, CancellationToken token);
        #endregion

        #region Other Relaxation
        Task<IEnumerable<RelaxationDTO>>GetAllOtherRelaxation(long accountNumber, CancellationToken token);
        Task<List<RelaxationDTO>> UpdateOtherRelaxation(List<RelaxationDTO> relax, CancellationToken token);
        #endregion

    }
}
