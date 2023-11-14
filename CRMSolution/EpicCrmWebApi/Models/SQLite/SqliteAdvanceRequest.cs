using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteAdvanceRequest
    {
        public bool IsNewEntity { get; set; }
        public bool IsNewAgreement { get; set; }

        public long EntityId { get; set; }
        public string EntityName { get; set; }

        public long AgreementId { get; set; }
        public string AgreementNumber { get; set; }

        // used only for new entries to match it up on server
        public string ParentReferenceId { get; set; }

        public double Amount { get; set; }
        public DateTime TimeStamp { get; set; }  // only date part is relevant
        public string Notes { get; set; }
        public string ActivityId { get; set; }
    }
}