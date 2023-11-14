using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class StockBalanceModel : StockTagModel
    {
        public long Id { get; set; }
        public long ItemMasterId { get; set; }

        [Display(Name = "Stock Qty.")]
        public long StockQuantity { get; set; }

        [Display(Name = "Item")]
        public string ItemCode { get; set; }

        [Display(Name = "Item")]
        public string ItemDesc { get; set; }

        [Display(Name = "Item Type")]
        public string Category { get; set; }

        [Display(Name = "UOM")]
        public string Unit { get; set; }

        public long CyclicCount { get; set; }
    }
}