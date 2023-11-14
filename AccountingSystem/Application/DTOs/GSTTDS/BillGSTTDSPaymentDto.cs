using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Application.DTOs.Bill;

namespace Application.DTOs.GSTTDS
{
    public class BillGsttdsPaymentDto
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(Bill))]
        public int BillID { get; set; }

        [ForeignKey(nameof(GSTTDSPaymentChallan))]
        public int? GSTTDSPaymentID { get; set; }

        public virtual BillsDto Bill { get; set; }

        public virtual GsstPaymentChallanDto GSTTDSPaymentChallan { get; set; }
    }
}
