using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainBankDetail
    {
        public string PhoneDbId { get; set; }
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

        public DateTime TimeStamp { get; set; }  // only date part is relevant
        public string ActivityId { get; set; }

        public IEnumerable<String> Images { get; set; }
    }

    public class SqliteBankDetailData : SqliteDomainBankDetail
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsProcessed { get; set; }
        public long EntityBankDetailId { get; set; }
        public int ImageCount { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
    }
}
