using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class STRFilter
    {
        public string STRNumber { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public string DWSNumber { get; set; }

        public string AgreementNumber { get; set; }
        public string ClientName { get; set; }
        public string TypeName { get; set; }
    }
}