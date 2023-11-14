using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ModelOrderItem
    {
        public long Id { get; set; }
        public long OrderId { get; set; }

        [Display(Name = "S.No.")]
        public int SequenceNumber { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [Display(Name = "Orig. Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Bill Rate")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Orig. Amount (Rs.)")]
        public decimal Amount { get; set; }

        [Display(Name = "Revised Quantity")]
        public int RevisedQuantity { get; set; }

        [Display(Name = "Revised Amount (Rs.)")]
        public decimal RevisedAmount { get; set; }

        public string UOM { get; set; }

        [Display(Name = "Discount %")]
        public decimal DiscountPercent { get; set; }

        [Display(Name = "Discounted Price")]
        public decimal DiscountedPrice { get; set; }

        [Display(Name = "Item Price")]
        public decimal ItemPrice { get; set; }

        [Display(Name = "Gst %")]
        public decimal GstPercent { get; set; }

        [Display(Name = "Gst (Rs.)")]
        public decimal GstAmount { get; set; }

        [Display(Name = "Net (Rs.)")]
        public decimal NetPrice { get; set; }


        public decimal RevisedDiscountPercent { get; set; }
        public decimal RevisedDiscountedPrice { get; set; }

        [Display(Name = "Revised Item Price")]
        public decimal RevisedItemPrice { get; set; }
        public decimal RevisedGstPercent { get; set; }
        [Display(Name = "Revised Gst (Rs.)")]
        public decimal RevisedGstAmount { get; set; }
        [Display(Name = "Revised Net (Rs.)")]
        public decimal RevisedNetPrice { get; set; }
    }
}