using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt
{
    public class TblLaPaymentDetDTO
    {
        public int Id { get; set; }
        public long LoanNo { get; set; }
        public int PromoterId { get; set; }
        public string PaymentRefNo { get; set; }
        public decimal? ActualAmt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DateOfInitiation { get; set; }
        public string PromoterName { get; set; }
        public string ChequeNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? ChequeDate { get; set; }
        public string IfscCode { get; set; }
        public string BranchCode { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DateOfChequeRealization { get; set; }
        public string UtrNo { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? PaidDate { get; set; }
        public int? PaymentMode { get; set; }
        public string PaymentStatus { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public List<int> ReceiptId { get; set; }
        public List<int> PaymentReceiptId { get; set; }

        public virtual ICollection<TblLaReceiptPaymentDetDTO> TblLaReceiptPaymentDet { get; set; }

    }
}
