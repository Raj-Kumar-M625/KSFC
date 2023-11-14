using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteOrderItem
    {
        public string ProductCode { get; set; }
        public decimal BillingPrice { get; set; }  // this will be zero for return orders
        public decimal DiscountPercent { get; set; }
        public decimal DiscountedPrice { get; set; } // subtract discount % from Billing price
        public int Quantity { get; set; }
        public decimal ItemPrice { get; set; }  // DiscountedPrice * Quantity
        public decimal GstPercent { get; set; } 
        public decimal GstAmount { get; set; } // Gst % applied on Item Price
        public decimal NetPrice { get; set; }  // Item Price + Gst Amount
    }
}