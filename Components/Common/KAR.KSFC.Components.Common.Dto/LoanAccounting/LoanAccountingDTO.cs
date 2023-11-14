using KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.LoanAccounting
{
    public class LoanAccountingDTO
    {
        public List<CodeTableDTO> TransactionType { get; set; }
        public List<CodeTableDTO> ModeOfPayment { get; set; }

        public List<TblLaReceiptPaymentDetDTO> ReceiptPaymentDetails { get; set; }

        public List<TblLaReceiptPaymentDetDTO> PaymentRecipetList { get; set; }
        public List<TblLaReceiptPaymentDetDTO> AllGenerateReceiptList { get; set; }
      


    }
}
