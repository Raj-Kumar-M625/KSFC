using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteBankDetailDisplayData
    {
        public long Id { get; set; }
        public bool IsProcessed { get; set; }
        public long EntityBankDetailId { get; set; }

        public bool IsNewEntity { get; set; }
        public long EntityId { get; set; }
        public string EntityName { get; set; }

        // used only for new entries to match it up on server
        public string ParentReferenceId { get; set; }

        public bool IsSelfAccount { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountHolderPAN { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankIFSC { get; set; }
        public string BankBranch { get; set; }
        public int ImageCount { get; set; }

        public DateTime TimeStamp { get; set; }  // only date part is relevant
        public string ActivityId { get; set; }

        public string PhoneDbId { get; set; }
    }
}