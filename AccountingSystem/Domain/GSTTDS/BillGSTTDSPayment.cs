using Domain.Bill;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.GSTTDS
{
    public class BillGsttdsPayment
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(Bill))]
        public int BillID { get; set; }

        [ForeignKey(nameof(GSTTDSPaymentChallan))]
        public int? GSTTDSPaymentID { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string ModifiedBy { get; set; }

        public virtual Bills Bill { get; set; }

        public virtual GsttdsPaymentChallan GSTTDSPaymentChallan { get; set; }
    }
}
