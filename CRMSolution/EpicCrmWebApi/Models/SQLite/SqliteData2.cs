using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteData2
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
        public IEnumerable<SqliteIssueReturn> SqliteIssueReturns { get; set; }
        public IEnumerable<SqliteWorkFlowPageData> SqliteWorkFlowPageData { get; set; }

        public DeviceInfo DeviceInfo { get; set; }
    }
}