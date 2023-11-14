using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainReturnOrder
    {
        public string PhoneDbId { get; set; }
        public string CustomerCode { get; set; }  // Customer to whom this order/return belongs
        public DateTime TimeStamp { get; set; }  // only date part is relevant - as order return date
        public decimal TotalAmount { get; set; }
        public string ReferenceNum { get; set; }
        public string Comment { get; set; }
        public int ItemCount { get; set; }  // number of order/return items
        public IEnumerable<SqliteDomainReturnOrderItem> Items { get; set; }
        public string ActivityId { get; set; }
    }
}
