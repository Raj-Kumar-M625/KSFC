using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class EntityBankDetail
    {
        public long Id { get; set; }
        public long EntityId { get; set; }

        public bool IsSelfAccount { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountHolderPAN { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankIFSC { get; set; }
        public string BankBranch { get; set; }
        public int ImageCount { get; set; }
        public bool IsActive { get; set; }

        public string Status { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
    }
}
