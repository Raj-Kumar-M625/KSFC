using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class FarmersBankAccountReport
    {
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long EntityId { get; set; }
        public bool IsSelfAccount { get; set; }
        public string AccountHolderName { get; set; }
        public string AccountHolderPAN { get; set; }
        public string BankName { get; set; }
        public string BankAccount { get; set; }
        public string BankIFSC { get; set; }
        public string BankBranch { get; set; }
        public string Status { get; set; }
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        public string HQCode { get; set; }
        public string HQName { get; set; }
        public string EntityNumber { get; set; }
        public string EntityName { get; set; }
    }
}
