using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteReturnOrderItemData
    {
        public long Id { get; set; }
        public long SqliteReturnOrderId { get; set; }
        public int SerialNumber { get; set; }
        public string ProductCode { get; set; }
        public int UnitQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}
