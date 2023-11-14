using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class AdvanceRequest
    {
        public long Id { get; set; }
        public long EntityAgreementId { get; set; }
        public long WorkFlowSeasonId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long DayId { get; set; }
        public long EntityId { get; set; }
        public System.DateTime AdvanceRequestDate { get; set; }
        public decimal Amount { get; set; }
        public long ActivityId { get; set; }
        public long SqliteAdvanceRequestId { get; set; }
        public string HQCode { get; set; }
        public string Crop { get; set; }
        public string WorkFlowSeasonName { get; set; }
        public string Notes { get; set; }
        public string EntityName { get; set; }
        public string AdvanceRequestStatus { get; set; }
        public string ReviewedBy { get; set; }
        public System.DateTime ReviewDate { get; set; }
        public string AgreementNumber { get; set; }
        public string AgreementStatus { get; set; }
        public string ApproveNotes { get; set; }
        public string RequestNotes { get; set; }
        public decimal AmountApproved { get; set; }

        public bool IsActive { get; set; }
        public bool IsActiveInSap { get; set; }
    }
}
