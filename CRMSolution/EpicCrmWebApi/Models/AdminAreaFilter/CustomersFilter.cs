using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class CustomersFilter
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // Dealer/P.Distributor/Distributor 
        public string HQCode { get; set; }
    }
}