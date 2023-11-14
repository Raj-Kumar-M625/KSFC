using Domain.Bill;
using Domain.Payment;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Payment
{
    public class BillTdsPaymentDto
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
