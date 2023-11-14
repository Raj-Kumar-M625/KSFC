using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TerminateAgreementRequestFilter : BaseSearchCriteria
    {
        public bool ApplyClientNameFilter { get; set; }
        public string ClientName { get; set; }

        public bool ApplyEmployeeCodeFilter { get; set; }
        public string EmployeeCode { get; set; }

        public bool ApplyEmployeeNameFilter { get; set; }
        public string EmployeeName { get; set; }

        //For Employee Status
        public bool ApplyEmployeeStatusFilter { get; set; }
        public bool EmployeeStatus { get; set; }

        public bool ApplyAgreementNumberFilter { get; set; }
        public string AgreementNumber { get; set; }

        public bool ApplyUniqueIdFilter { get; set; }
        public string UniqueId { get; set; }

        public bool ApplyAgreementStatusFilter { get; set; }
        public string AgreementStatus { get; set; }

        public bool ApplyCropFilter { get; set; }
        public string Crop { get; set; }

        public bool ApplyRedFarmerReqStatusFilter { get; set; }
        public string RedFarmerReqStatus { get; set; }

        public bool ApplyRedFarmerReqReasonFilter { get; set; }
        public string RedFarmerReqReason { get; set; }

        public bool ApplyDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
