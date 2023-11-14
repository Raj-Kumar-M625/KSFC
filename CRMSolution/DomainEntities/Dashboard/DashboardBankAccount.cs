using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DashboardBankAccount
    {
        public long Id { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BankPhone { get; set; }
        public string AccountNumber { get; set; }
        public string IFSC { get; set; }

        public string AccountName { get; set; }
        public string AccountAddress { get; set; }
        public string AccountEmail { get; set; }

        public bool IsActive { get; set; }

        public string CurrentStaffCode { get; set; }
    }
}
