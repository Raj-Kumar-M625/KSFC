using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainReturnOrderItem
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public decimal BillingPrice { get; set; }  // this will be zero for return orders
        public decimal ItemPrice { get; set; }
        public string Comment { get; set; }
    }
}
