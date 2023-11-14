
using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Services.IServices.Admin.LoanAccounting.LoanRelatedReceipt
{
    public interface ILoanRelatedReceiptService 
    {

        Task<IEnumerable<TblLaReceiptPaymentDetDTO>> GetAllReceiptPaymentList(long? accountNumber);

        Task<IEnumerable<CodeTableDTO>> GetCodeTableList(long accountNumber);

        Task<bool> UpdateReceiptPaymentDetails(TblLaReceiptPaymentDetDTO addr);
        Task<bool> CreatePaymentDetails(TblLaPaymentDetDTO payment);
        Task<bool> DeleteReceiptPaymentDetails(TblLaReceiptPaymentDetDTO addr);

        Task<IEnumerable<TblLaReceiptDetDTO>> GetAllReceiptList();
        Task<IEnumerable<TblLaPaymentDetDTO>> GetAllPaymentList();
        Task<bool> ApproveReceiptPaymentDetails(TblLaReceiptPaymentDetDTO addr);
        Task<bool> RejectReceiptPaymentDetails(TblLaReceiptPaymentDetDTO addr);
        Task<bool> UpdateCreatePaymentDetails(List<TblLaReceiptPaymentDetDTO> addr);
        Task<bool> CreateReceiptPaymentDetails(TblLaReceiptPaymentDetDTO addr);
        Task<IEnumerable<TblLaReceiptPaymentDetDTO>> GetAllRecipetsForPayment(int PaymnetId);
    }
}
