using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ReturnItem
    {
        public long Id { get; set; }
        public long ReturnsId { get; set; }
        public int SequenceNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public decimal BillingPrice { get; set; }
        public decimal Amount { get; set; }
        public string Comments { get; set; }
    }
}
