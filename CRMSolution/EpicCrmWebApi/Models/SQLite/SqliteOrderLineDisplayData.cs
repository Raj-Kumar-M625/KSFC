using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteOrderLineDisplayData
    {
        public long Id { get; set; }
        public long SqliteOrderId { get; set; }

        [Display(Name = "S.No")]
        public int SerialNumber { get; set; }

        [Display(Name = "Product")]
        public string ProductCode { get; set; }

        [Display(Name = "Quantity")]
        public int UnitQuantity { get; set; }

        [Display(Name = "Bill Price (Rs.)")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Amount (legacy)")]
        public decimal Amount { get; set; }

        [Display(Name = "Discount %")]
        public decimal DiscountPercent { get; set; }

        [Display(Name = "Discounted Price (Rs.)")]
        public decimal DiscountedPrice { get; set; }

        [Display(Name = "Item Price (Rs.)")]
        public decimal ItemPrice { get; set; }

        [Display(Name = "GST %")]
        public decimal GstPercent { get; set; }

        [Display(Name = "GST (Rs.)")]
        public decimal GstAmount { get; set; }

        [Display(Name = "Net Price (Rs.)")]
        public decimal NetPrice { get; set; }
    }
}