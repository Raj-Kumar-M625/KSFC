using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class VendorBankData
    {
        public Nullable<long> TransporterId { get; set; }
        public string AccountHolderName { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankIFSC { get; set; }
        public string BankBranch { get; set; }
    }
}
