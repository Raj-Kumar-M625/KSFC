using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.InputSearchCriteria
{
    public class GenericInputSearchCriteria
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string TransactionType { get; set; }
        public decimal MinAmount { get; set; }
        public decimal MaxAmount { get; set; }
        public long AccountNo { get; set; }
        public string BankName { get; set; }
        public string FileName { get; set; }
        public int Count { get; set; }

    }
}
