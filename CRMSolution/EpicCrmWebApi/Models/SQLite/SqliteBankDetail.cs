using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteBankDetail : SqliteBase
    {
        public string Id { get; set; }
        public bool IsNewEntity { get; set; }
        public long EntityId { get; set; }
        public string EntityName { get; set; }

        public string ParentReferenceId { get; set; }  // fk to ServerEntity.Id or 

        // if entityId is zero, then use ReferenceId as fk in Entity table
        // else entityId is fk in ServerEntity

        public bool IsSelfAccount { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountHolderPAN { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankIFSC { get; set; }
        public string BankBranch { get; set; }

        public DateTime TimeStamp { get; set; }
        public string ActivityId { get; set; }
    }
}