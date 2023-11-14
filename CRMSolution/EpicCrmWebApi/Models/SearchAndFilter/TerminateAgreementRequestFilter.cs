using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class TerminateAgreementRequestFilter
    {
        public string Zone { get; set; }
        public string Area { get; set; }
        public string Territory { get; set; }
        public string HQ { get; set; }

        public string ClientName { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }

        public string AgreementNumber { get; set; }
        public string UniqueId { get; set; }
        public string AgreementStatus { get; set; }

        public string Crop { get; set; }

        public string DateFrom { get; set; }
        public string DateTo { get; set; }

        public string RedFarmerReqStatus { get; set; }
        public string RedFarmerReqReason { get; set; }
    }
}