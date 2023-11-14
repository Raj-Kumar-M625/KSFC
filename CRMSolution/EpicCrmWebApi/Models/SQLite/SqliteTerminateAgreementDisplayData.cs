using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteTerminateAgreementDisplayData
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsProcessed { get; set; }
        public long TerminateAgreementId { get; set; }

        public long EntityId { get; set; }
        public string EntityName { get; set; }
        public long AgreementId { get; set; }
        public string Agreement { get; set; }
        public DateTime TimeStamp { get; set; }

        public string TypeName { get; set; }
        public string ActivityId { get; set; }

        public string Reason { get; set; }
        public string Notes { get; set; }
    }
}