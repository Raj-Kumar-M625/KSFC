using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class PaymentReferenceFilter
    {
        public string PaymentReference { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}