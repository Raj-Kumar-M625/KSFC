using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadCustomerModel
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string Type { get; set; } // Dealer/P.Distributor/Distributor 
        public long CreditLimit { get; set; }
        public long Outstanding { get; set; }
        public long LongOutstanding { get; set; }
        public long Target { get; set; }
        public long Sales { get; set; }
        public long Payment { get; set; }
        public string HQCode { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int SalesPercentage { get; set; }
        public int PaymentPercentage { get; set; }
    }
}