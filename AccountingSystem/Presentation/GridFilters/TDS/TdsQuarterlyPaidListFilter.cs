using System;

namespace Presentation.GridFilters.TDS
{
    public class TdsQuarterlyPaidListFilter
    {
        public string[] forder { get; set; }
        public string tdsSection { get; set; }
        public decimal noOfVendors { get; set; }
        public decimal noOfTransactions { get; set; }
        public decimal payableMinAmount { get; set; }
        public decimal payableMaxAmount { get; set; }
        public string bankName { get; set; }

        public string accountNo { get; set; }
        public string quarter { get; set; }

        public string challanDate { get; set; }

        public int assessmentYear { get; set; }
    }
}
