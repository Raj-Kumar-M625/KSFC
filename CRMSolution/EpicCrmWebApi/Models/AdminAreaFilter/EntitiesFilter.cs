using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EntitiesFilter
    {
        public string Zone { get; set; }
        public string Area { get; set; }
        public string Territory { get; set; }
        public string HQ { get; set; }

        public string ClientName { get; set; }
        public string ClientType { get; set; } 
        //public string HQCode { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string AgreementNumber { get; set; }
        public string UniqueId { get; set; }
        public string AgreementStatus { get; set; }
        public string BankDetailStatus { get; set; }

        public int ProfileStatus { get; set; }

        public string Crop { get; set; }
        public string EntityNumber { get; set; }
    }
}