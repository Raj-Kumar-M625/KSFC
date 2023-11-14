using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class AgreementReportData
    {
        public long EntityId { get; set; }    
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public int ContactCount { get; set; }
        public int ImageCount { get; set; }
        public string WorkflowSeasonName { get; set; }
        public string CropName { get; set; }
        public string EntityName { get; set; }
        public string UniqueIdType { get; set; }
        public string UniqueId { get; set; }
        public string FatherHusbandName { get; set; }
        public bool IsActive { get; set; }
        public string AgreementNumber { get; set; }
        public string Status { get; set; }
        public decimal LandSize { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public bool AtBusiness { get; set; }
        public int BankDetailCount { get; set; }
        public DateTime EntityDate { get; set; }
        public string ZoneName { get; set; }
        public string ZoneCode { get; set; }
        public decimal RatePerKg { get; set; }

        public string AreaName { get; set; }
        public string AreaCode { get; set; }

        public string TerritoryName { get; set; }
        public string TerritoryCode { get; set; }

        public string HQName { get; set; }
        public string HQCode{ get; set; }

    }
}
