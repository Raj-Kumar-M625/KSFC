using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblLaReceiptPaymentDet
    {
        public int ReceiptPaymentId { get; set; }
        public int? PaymentId { get; set; }
        public int ReceiptId { get; set; }
        public string ReceiptPaymentStatus { get; set; }
        public string DateofInitiation { get; set; }
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
        public virtual TblLaReceiptDet TblLaReceiptDet { get; set; }
        public virtual TblLaPaymentDet TblLaPaymentDet { get; set; }

    }
}
