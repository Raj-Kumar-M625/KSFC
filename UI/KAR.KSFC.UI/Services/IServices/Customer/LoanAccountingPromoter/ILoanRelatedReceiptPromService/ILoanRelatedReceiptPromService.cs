
using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Customer.LoanAccountingPromoter.LoanRelatedReceipt
{
    public interface ILoanRelatedReceiptPromService 
    {
        Task<IEnumerable<TblLaReceiptPaymentDetDTO>> GetAllGenerateReceiptPaymentList(long accountNumber);
        Task<IEnumerable<CodeTableDTO>> GetCodeTableList(long accountNumber);
        Task<bool> UpdateCreatePromPaymentDetails(List<TblLaReceiptPaymentDetDTO> addr);
     

    }
}
