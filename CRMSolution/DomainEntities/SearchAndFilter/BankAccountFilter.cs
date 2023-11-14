using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class BankAccountFilter
    {
        public bool ApplyAreaCodeFilter { get; set; }
        public string AreaCode { get; set; }
        
        public bool ApplyBankNameFilter { get; set; }
        public string BankName { get; set; }

    }
}
