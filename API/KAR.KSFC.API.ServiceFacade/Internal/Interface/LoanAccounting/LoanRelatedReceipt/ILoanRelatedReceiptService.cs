using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccounting.LoanRelatedReceipt
{
    public interface ILoanRelatedReceiptService
    {
        #region ReceiptPayment List
        Task<IEnumerable<TblLaReceiptPaymentDetDTO>> GetAllReceiptPaymentList(long accountNumber, CancellationToken token);
        Task<bool> UpdateReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token);
        Task<bool> DeleteReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token);
        Task<IEnumerable<TblLaReceiptDetDTO>> GetAllReceiptRefNum(CancellationToken token);
        Task<IEnumerable<TblLaPaymentDetDTO>> GetAllPaymentRefNum(CancellationToken token);
        Task<bool> ApproveReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token);
        Task<bool> RejectReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token);


        Task<bool> UpdateCreatePaymentDetails(List<TblLaReceiptPaymentDetDTO> ReceiptPaymentDto, CancellationToken token);
        Task<bool> CreateReceiptPaymentDetails(TblLaReceiptPaymentDetDTO ReceiptPaymentDto, CancellationToken token);
        Task<bool> CreatePaymentDetails(TblLaPaymentDetDTO PaymentDto, CancellationToken token);
        #endregion
    }
}

