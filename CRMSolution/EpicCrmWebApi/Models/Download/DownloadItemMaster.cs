using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadItemMaster
    {
        public long ItemId { get; set; }
        public string ItemCode { get; set; }
        public string ItemDesc { get; set; }
        public string Category { get; set; }
        public string Unit { get; set; }
        public string Classification { get; set; }
        public string TypeName { get; set; } // crop name

        public decimal Rate { get; set; }
        public decimal ReturnRate { get; set; }
    }
}