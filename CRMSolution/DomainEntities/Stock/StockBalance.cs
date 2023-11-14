using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class StockBalance : Stock
    {
        public long Id { get; set; }
        public long ItemMasterId { get; set; }
        public long StockQuantity { get; set; }

        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }

        public long CyclicCount { get; set; }

        public int IssueQuantity { get; set; }
    }
}
