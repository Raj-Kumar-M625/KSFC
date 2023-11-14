using Application.DTOs.Bill;
using Application.DTOs.GSTTDS;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.GSTTDS
{
    public class BillGsttdsPaymentModel
    {
        [Key]
        public int ID { get; set; }

        [ForeignKey(nameof(Bill))]
        public int BillID { get; set; }

        [ForeignKey(nameof(GSTTDSPaymentChallan))]
        public int? GSTTDSPaymentID { get; set; }
        public virtual BillsDto Bill { get; set; }
        public virtual GsttdsPaymentChallanDto GSTTDSPaymentChallan { get; set; }
    }
}
