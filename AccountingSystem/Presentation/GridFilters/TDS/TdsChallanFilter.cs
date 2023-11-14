using System;

namespace Presentation.GridFilters.TDS
{
    public class TdsChallanFilter
    {
        public string[] forder { get; set; }
        public string tdsSection { get; set; }
        public int noOfVendors { get; set; }
        public int noOfTransactions { get; set; }
        public decimal payableMinAmount { get; set; }
        public decimal payableMaxAmount { get; set; }
        public string bankName { get; set; }

        public string accountNo { get; set; }

        public string challanDate { get; set; }
    }
}
