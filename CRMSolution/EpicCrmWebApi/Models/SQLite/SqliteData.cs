using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteData
    {
        public string BatchGuid { get; set; }
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }

        public string IMEI { get; set; }
        public IEnumerable<SqliteAction> SqliteActionDataCollection { get; set; }
        public SqliteExpense SqliteExpense { get; set; }
        public IEnumerable<SqliteOrder> SqliteOrders { get; set; }
        public IEnumerable<SqlitePayment> SqlitePayments { get; set; }
        public IEnumerable<SqliteReturnOrder> SqliteOrderReturns { get; set; }
        public string ActionName { get; set; }  // Upload or EndDay
        public IEnumerable<SqliteDeviceLog> DeviceLogs { get; set; }
        public IEnumerable<SqliteLeave> SqliteLeaves { get; set; }

        // list of leave ids, that user wants to cancel
        public IEnumerable<long> SqliteCancelledLeaves { get; set; }
        public IEnumerable<SqliteEntity> SqliteBusinessEntities { get; set; }
        public IEnumerable<SqliteAgreement> SqliteAgreements { get; set; }
        public IEnumerable<SqliteSurvey> SqliteSurveys { get; set; }

        public IEnumerable<SqliteBankDetail> SqliteBankDetails { get; set; }

        public IEnumerable<SqliteIssueReturn> SqliteIssueReturns { get; set; }
        public IEnumerable<SqliteAdvanceRequest> SqliteAdvanceRequests { get; set; }
        public IEnumerable<SqliteWorkFlowPageData> SqliteWorkFlowPageData { get; set; }
        public IEnumerable<SqliteTerminateAgreementData> SqliteTerminateAgreementData { get; set; }

        public IEnumerable<SqliteSTRData> SqliteSTR { get; set; }

        public IEnumerable<SqliteDayPlanTarget> SqliteDayPlan { get; set; }

        /// <summary>
        /// Author: Rajesh V; Date24-06-2021 ; Purpose: dealer Questionnaire
        /// </summary>
        public IEnumerable<SqliteQuestionnaire> SqliteQuestionnaireData { get; set; }

        public IEnumerable<SqliteFollowUpTask> SqliteFollowUpTaskData { get; set; }
        public IEnumerable<SqliteFollowUpTaskAction> SqliteFollowUpTaskActionData { get; set; }

        public DeviceInfo DeviceInfo { get; set; }

        public IEnumerable<SqliteImage> Images { get; set; }

        public bool IsDataBatch { get; set; }
        public bool IsLogsBatch { get; set; }
        public bool IsImageBatch { get; set; }
        
    }
}