using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class VendorSTRFilter : BaseSearchCriteria
    {
        public string STRNumber { get; set; }       
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
                
        public string VendorName { get; set; }
        public string SeasonName { get; set; }
        public string VehicleNumber { get; set; }
        public string STRPaymentStatus { get; set; }
    }
}