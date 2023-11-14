using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EntityAgreement
    {
        public long Id { get; set; }
        public long EntityId { get; set; }
        public long WorkflowSeasonId { get; set; }
        public string WorkflowSeasonName { get; set; }
        public string TypeName { get; set; }
        public string AgreementNumber { get; set; }
        public string Status { get; set; }
        public string UniqueId { get; set; }
        public bool IsPassbookReceived { get; set; }
        public System.DateTime PassbookReceivedDate { get; set; }
        public decimal LandSizeInAcres { get; set; }
        public decimal RatePerKg { get; set; }
        public long ActivityId { get; set; }

        public int ActivityCount { get; set; }
        public Decimal? TotalAdvanceRequested { get; set; }
        public Decimal? TotalAdvanceApproved { get; set; }

        public int DWSCount { get; set; }
        public int IssueReturnCount { get; set; }
        public int AdvanceRequestCount { get; set; }
        public bool HasWorkflow { get; set; }
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string HQCode { get; set; }
        public string HQName { get; set; }
        public long EmployeeId { get; set; }
        public string CreatedBy { get; set; }

    }

    public class EntityAgreementForIR 
    {
        public long Id { get; set; }
        public string AgreementNumber { get; set; }
        public string TypeName { get; set; }
       
    }
}
