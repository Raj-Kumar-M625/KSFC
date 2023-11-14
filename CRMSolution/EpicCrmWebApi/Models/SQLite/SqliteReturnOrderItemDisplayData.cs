using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteReturnOrderItemDisplayData
    {
        public long Id { get; set; }
        public long SqliteReturnOrderId { get; set; }

        [Display(Name = "S.No")]
        public int SerialNumber { get; set; }

        [Display(Name = "Product")]
        public string ProductCode { get; set; }

        [Display(Name = "Quantity")]
        public int UnitQuantity { get; set; }

        [Display(Name = "Price")]
        public decimal UnitPrice { get; set; }

        public decimal Amount { get; set; }

        public string Comment { get; set; }
    }
}