using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DWSFilter
    {
        public string Zone { get; set; }
        public string Area { get; set; }
        public string Territory { get; set; }
        public string HQ { get; set; }

        public string ClientName { get; set; }
        public string AgreementNumber { get; set; }

        public string STRNumber { get; set; }
        public string DWSNumber { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public string DWSStatus { get; set; }

        public string Page { get; set; }

        public bool IsDWSApprovePage => "DWSApprove".Equals(Page, StringComparison.OrdinalIgnoreCase);
    }
}