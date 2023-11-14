using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainOrder
    {
        public string PhoneDbId { get; set; }
        public string CustomerCode { get; set; }  // Customer to whom this order/return belongs
        public DateTime TimeStamp { get; set; }  // only date part is relevant - as order date

        // this is the amount on which GST is calculated
        public decimal TotalAmount { get; set; } // total amount of order - for returns it will be 0
        public decimal TotalGST { get; set; }
        public decimal NetAmount { get; set; } // TotalAmount + TotalGST

        public int ItemCount { get; set; }  // number of order/return items
        public IEnumerable<SqliteDomainOrderItem> Items { get; set; }
        public string ActivityId { get; set; }

        public decimal MaxDiscountPercentage { get; set; }
        public string DiscountType { get; set; }
        public decimal AppliedDiscountPercentage { get; set; }

        public IEnumerable<String> Images { get; set; }  // instrument images
    }
}
