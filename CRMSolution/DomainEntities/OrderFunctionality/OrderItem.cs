using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class OrderItem
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public int SequenceNumber { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string UOM { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public int RevisedQuantity { get; set; }
        public decimal RevisedAmount { get; set; }

        public decimal DiscountPercent { get; set; }
        public decimal DiscountedPrice { get; set; }
        public decimal ItemPrice { get; set; }
        public decimal GstPercent { get; set; }
        public decimal GstAmount { get; set; }
        public decimal NetPrice { get; set; }
        public decimal RevisedDiscountPercent { get; set; }
        public decimal RevisedDiscountedPrice { get; set; }
        public decimal RevisedItemPrice { get; set; }
        public decimal RevisedGstPercent { get; set; }
        public decimal RevisedGstAmount { get; set; }
        public decimal RevisedNetPrice { get; set; }
    }
}
