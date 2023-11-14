using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class StockFilter
    {
        public string Zone { get; set; }
        public string Area { get; set; }
        public string Territory { get; set; }
        public string HQ { get; set; }

        public string VendorName { get; set; }
        public string GRNNumber { get; set; }

        public string InvoiceNumber { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public string Status { get; set; }

        public string CalledFrom { get; set; }
    }
}