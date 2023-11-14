using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class StockRequestFull : Stock
    {
        public long Id { get; set; }
        public long StockRequestTagId { get; set; }

        public string RequestNumber { get; set; }
        public System.DateTime RequestDate { get; set; }
        public string Notes { get; set; }

        public long ItemMasterId { get; set; }
        public int Quantity { get; set; }

        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }

        public string Status { get; set; }

        public long CyclicCount { get; set; }

        public int QuantityIssued { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public long StockLedgerId { get; set; }
        public string ReviewNotes { get; set; }

        public string RequestType { get; set; }

        public bool IsPendingFulfillment => Status.Equals(StockStatus.Pending.ToString(), StringComparison.OrdinalIgnoreCase);

        public string CurrentUser { get; set; }
    }
}
