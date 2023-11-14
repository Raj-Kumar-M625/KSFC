using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadStockBalance
    {
        public long Id { get; set; }
        public long ItemMasterId { get; set; }
        public long StockQuantity { get; set; }

        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
    }
}