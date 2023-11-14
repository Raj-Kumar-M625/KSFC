using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class PaymentReferenceFilter : BaseSearchCriteria
    {
        public bool ApplyPaymentReferenceFilter { get; set; }
        public bool IsExactReferenceMatch { get; set; }
        public string PaymentReference { get; set; }

        public bool ApplyDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
