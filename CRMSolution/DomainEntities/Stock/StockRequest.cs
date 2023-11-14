using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class StockRequest
    {
        public long Id { get; set; }
        public long StockRequestTagId { get; set; }
        //public int LineNumber { get; set; }
        public long ItemMasterId { get; set; }
        public int Quantity { get; set; }

        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }

        public string Status { get; set; }

        public long CyclicCount { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }

        public int QuantityIssued { get; set; }
        public Nullable<System.DateTime> IssueDate { get; set; }
        public long StockLedgerId { get; set; }
        public string ReviewNotes { get; set; }

        public long ParentCyclicCount { get; set; }

        //public bool IsEditAllowed => Status.Equals(StockStatus.Received.ToString(), StringComparison.OrdinalIgnoreCase);

        public bool DeleteMe { get; set; } = false;
        public string CurrentUser { get; set; }
    }
}
