using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class StockLedger : Stock
    {
        public long Id { get; set; }
        public System.DateTime TransactionDate { get; set; }
        public long ItemMasterId { get; set; }
        public string ReferenceNo { get; set; }
        public int LineNumber { get; set; }
        public int IssueQuantity { get; set; }
        public int ReceiveQuantity { get; set; }

        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
    }
}
