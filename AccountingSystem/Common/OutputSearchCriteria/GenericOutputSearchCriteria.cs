using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.OutputSearchCriteria
{
    public class GenericOutputSearchCriteria
    {
        public bool ApplyTransactionDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public bool ApplyTransactionTypeFilter { get; set; }
        public string TransactionType { get; set; }

        public bool ApplyAmountFilter { get; set; }
        public decimal MinAmount { get; set; } = 0;
        public decimal MaxAmount { get; set; } = decimal.MaxValue;
        public decimal Balance { get; set; }

        public bool ApplyAccountNumberFilter { get; set; }
        public long AccountNo { get; set; }

        public bool ApplyBankNameFilter { get; set; }
        public string BankName { get; set; }

        public bool ApplyFileNameFilter { get; set; }
        public string FileName { get; set; }
        public int Count { get; set; } = 1000;

        public bool ApplyVendorNameFile { get; set; }
        public string VendorName { get; set; }
    }
}
