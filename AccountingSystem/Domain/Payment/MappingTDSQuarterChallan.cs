using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Payment
{
    public class MappingTdsQuarterChallan
    {
        public int ID { get; set; }

        [ForeignKey(nameof(TDSPaymentChallan))]
        public int TDSPaymentChallanID { get; set; }

        public int QuarterlyTDSPaymentChallanID { get; set; }

        public virtual TdsPaymentChallan TDSPaymentChallan { get; set; }
    }
}
