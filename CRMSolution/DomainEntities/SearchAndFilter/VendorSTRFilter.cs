using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class VendorSTRFilter : BaseSearchCriteria
    {
        public bool ApplySTRNumberFilter { get; set; }
        public string STRNumber { get; set; }
        public bool ApplyDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public bool ApplyVendorNameFilter { get; set; }
        public string VendorName { get; set; }
        public bool ApplySeasonNameFilter { get; set; }
        public string SeasonName { get; set; }
        public bool ApplyVehicleNumberFilter { get; set; }
        public string VehicleNumber { get; set; }
        public bool ApplySTRPaymentStatusFilter { get; set; }
        public string STRPaymentStatus { get; set; }

        public bool ApplyPaymentReferenceFilter { get; set; }
        public bool IsExactPaymentReferenceMatch { get; set; } = false;
        public string PaymentReference { get; set; }
    }
}
