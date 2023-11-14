using Domain.Bill;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Payment
{
    public class BillTdsPayment
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(Bill))]
        public int BillID { get; set; }

        [ForeignKey(nameof(TDSPaymentChallan))]
        public int? TDSPaymentChallanID { get; set; }

        public virtual Bills Bill { get; set; }

        public virtual TdsPaymentChallan TDSPaymentChallan { get; set; }
    }
}
