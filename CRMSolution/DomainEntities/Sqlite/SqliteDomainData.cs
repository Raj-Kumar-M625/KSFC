using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainData
    {
        public string BatchGuid { get; set; }
        public long EmployeeId { get; set; }
        public long ImageCount { get; set; }
        public long ImageSaveCount { get; set; }
        public IEnumerable<SqliteDomainAction> DomainActions { get; set; }
        public SqliteDomainExpense DomainExpense { get; set; }
        public IEnumerable<SqliteDomainOrder> DomainOrders { get; set; }
        public IEnumerable<SqliteDomainPayment> DomainPayments { get; set; }
        public IEnumerable<SqliteDomainReturnOrder> DomainReturnOrders { get; set; }
        public IEnumerable<SqliteDomainLeave> DomainLeaves { get; set; }
        public IEnumerable<long> DomainCancelledLeaves { get; set; }
        public IEnumerable<SqliteDomainEntity> DomainEntities { get; set; }

        public IEnumerable<SqliteDomainAgreement> DomainAgreements { get; set; }
        public IEnumerable<SqliteDomainSurvey> DomainSurveys { get; set; }
        public IEnumerable<SqliteDomainBankDetail> DomainBankDetails { get; set; }

        public IEnumerable<SqliteDeviceLog> DeviceLogs { get; set; }
        public IEnumerable<SqliteDomainIssueReturn> DomainIssueReturns { get; set; }
        public IEnumerable<SqliteDomainAdvanceRequest> DomainAdvanceRequests { get; set; }

        public IEnumerable<SqliteDomainTerminateAgreementData> DomainTerminateAgreementData { get; set; }

        public IEnumerable<SqliteDomainWorkFlowPageData> DomainWorkFlowPageData { get; set; }

        public IEnumerable<SqliteDomainSTRData> DomainSTRData { get; set; }
        public IEnumerable<SqliteDomainDayPlanTarget> DomainDayPlanTarget { get; set; }

        /// <summary>
        /// Author: Rajesh V; Date24-06-2021 ; Purpose: dealer Questionnaire
        /// </summary>
        public IEnumerable<SqliteDomainQuestionnaire> DomainQuestionnaire { get; set; }
        public IEnumerable<SqliteDomainTask> DomainTask { get; set; }
        public IEnumerable<SqliteDomainTaskAction> DomainTaskAction { get; set; }

        public bool IsDataBatch { get; set; }

        public string DataFileName { get; set; }
        public long DataFileSize { get; set; }
    }
}
