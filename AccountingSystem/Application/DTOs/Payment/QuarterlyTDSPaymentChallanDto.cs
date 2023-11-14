using AutoMapper.Configuration.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Payment
{
    public class QuarterlyTdsPaymentChallanDto
    {
        public int ID { get; set; }
        public DateTime? DateOfFiling { get; set; }

        public int TracesReferenceNo { get; set; }
        public decimal TotalPaidAmount { get; set; }

        public int NoOfTrans { get; set; }

        public int Quarter { get; set; }

        public int NoOfChallan { get; set; }

        public decimal? TotalAmount { get; set; }

        public int QuarterStatus { get; set; }

        public int AssementYear { get; set; }

    }
}
