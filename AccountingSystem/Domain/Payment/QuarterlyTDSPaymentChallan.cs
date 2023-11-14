using Domain.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Payment
{
    public class QuarterlyTdsPaymentChallan
    {
        public int ID { get; set; }
        public DateTime? DateOfFiling { get; set; }

        public int TracesReferenceNo { get; set; }
        public decimal TotalPaidAmount { get; set; }

        public int NoOfTrans { get; set; }

        [ForeignKey(nameof(QuarterMaster))]
        public int Quarter { get; set; }

        public int NoOfChallan { get; set; }

        public decimal TotalAmount { get; set; }

        [ForeignKey(nameof(QuarterStatusMaster))]
        public int QuarterStatus { get; set; }

        [ForeignKey(nameof(AssementYearMaster))]
        public int AssementYear { get; set; }

        public virtual CommonMaster QuarterMaster { get; set; }
        public virtual CommonMaster QuarterStatusMaster { get; set; }

        public virtual CommonMaster AssementYearMaster { get; set; }

    }
}
