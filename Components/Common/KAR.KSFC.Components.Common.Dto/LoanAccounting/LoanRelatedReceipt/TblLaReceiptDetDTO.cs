using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.LoanAccounting.LoanRelatedReceipt
{
    public class TblLaReceiptDetDTO
    {
        public int Id { get; set; }
        public string? ReceiptRefNo { get; set; }
        public string? ReceiptStatus { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DateOfGeneration { get; set; }
        public decimal? AmountDue { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? DueDatePayment { get; set; }
        public int TransTypeId { get; set; }
        public long LoanNo { get; set; }
        public string? Remarks { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual CodeTableDTO CodeTable { get; set; }
        public virtual TblLaReceiptPaymentDetDTO TblLaReceiptPaymentDet { get; set; }
    }
}
