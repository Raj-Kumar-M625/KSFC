using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class StockInputTag : Stock
    {
        public long Id { get; set; }

        public string GRNNumber { get; set; }
        public System.DateTime ReceiveDate { get; set; }
        public string VendorName { get; set; }
        public string VendorBillNo { get; set; }
        public System.DateTime VendorBillDate { get; set; }
        public int TotalItemCount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }

        public string ReviewNotes { get; set; }
        public string ReviewedBy { get; set; }
        public DateTime ReviewDate { get; set; }

        public int? ItemCountFromLines { get; set; }
        public decimal? AmountTotalFromLines { get; set; }

        public long CyclicCount { get; set; }
        public bool IsEditAllowed => Status.Equals(StockStatus.Received.ToString(), StringComparison.OrdinalIgnoreCase);

        public string CurrentUser { get; set; }
    }
}
