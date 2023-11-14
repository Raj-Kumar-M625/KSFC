using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadConsolidatedCustomerModel
    {
        public int CustomerCount { get; set; }
        public decimal Outstanding { get; set; }
        public decimal LongOutstanding { get; set; }
        public decimal Target { get; set; }
        public decimal Sales { get; set; }
        public decimal Payment { get; set; }
        public int SalesPercentage { get; set; }
        public int PaymentPercentage { get; set; }
    }
}