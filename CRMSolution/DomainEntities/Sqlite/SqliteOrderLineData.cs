using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteOrderLineData
    {
        public long Id { get; set; }
        public long SqliteOrderId { get; set; }
        public int SerialNumber { get; set; }
        public string ProductCode { get; set; }
        public int UnitQuantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }  // legacy apk v1.3 and lower

        public decimal DiscountPercent { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal GstPercent { get; set; }
        public decimal GstAmount { get; set; }
        public decimal NetPrice { get; set; }
    }
}
