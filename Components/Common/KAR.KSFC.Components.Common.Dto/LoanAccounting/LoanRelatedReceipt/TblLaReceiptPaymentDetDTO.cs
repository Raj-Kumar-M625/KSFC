using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt
{
    public class TblLaReceiptPaymentDetDTO
    {
        public int ReceiptPaymentId { get; set; }
        public int? PaymentId { get; set; }
        public int ReceiptId { get; set; }
        public string ReceiptPaymentStatus { get; set; }
        public string  DateofInitiation { get; set; }
        public string TransactionType { get; set; }
        public string ModeType { get; set; }
        public decimal? PaymentAmt { get; set; }
        public decimal? TotalAmt { get; set; }
        public decimal? ActualAmt { get; set; }
        public decimal? BalanceAmt { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? UniqueID { get; set; }
        public int? Action { get; set; }
        public virtual TblLaReceiptDetDTO TblLaReceiptDet { get; set; }
        public virtual TblLaPaymentDetDTO TblLaPaymentDet { get; set; }
    }
}
