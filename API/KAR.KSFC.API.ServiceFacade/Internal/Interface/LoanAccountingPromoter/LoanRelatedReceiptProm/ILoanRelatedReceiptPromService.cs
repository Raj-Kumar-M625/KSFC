using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace KAR.KSFC.API.ServiceFacade.Internal.Interface.LoanAccountingPromoter.LoanRelatedReceiptProm
{
    public interface ILoanRelatedReceiptPromService
    {
        #region GenerateReceipt
        Task<IEnumerable<TblLaReceiptPaymentDetDTO>> GetAllGenerateReceiptPaymentList(long accountNumber, CancellationToken token);
        Task<bool> UpdateCreatePromPaymentDetails(List<TblLaReceiptPaymentDetDTO> ReceiptPaymentDto, CancellationToken token);

        #endregion
        #region RecieptPayments
        Task<IEnumerable<TblLaReceiptPaymentDetDTO>> GetAllRecipetsForPayment(int PaymnetId, CancellationToken token);
        #endregion
    }
}
