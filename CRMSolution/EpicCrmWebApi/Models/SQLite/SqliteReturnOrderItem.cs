using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteReturnOrderItem
    {
        public string ProductCode { get; set; }
        public int Quantity { get; set; }
        public decimal BillingPrice { get; set; }  // this will be zero for return orders
        public decimal ItemPrice { get; set; }
        public string Comment { get; set; }
    }
}