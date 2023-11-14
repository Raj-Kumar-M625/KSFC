using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ModelReturnItem
    {
        public long Id { get; set; }
        public long ReturnsId { get; set; }

        [Display(Name = "S.No.")]
        public int SequenceNumber { get; set; }

        [Display(Name = "Product Name")]
        public string ProductName { get; set; }

        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }

        [Display(Name = "Quantity")]
        public int Quantity { get; set; }

        [Display(Name = "Bill Rate")]
        public decimal BillingPrice { get; set; }

        [Display(Name = "Amount (Rs.)")]
        public decimal Amount { get; set; }

        public string Comments { get; set; }
    }
}