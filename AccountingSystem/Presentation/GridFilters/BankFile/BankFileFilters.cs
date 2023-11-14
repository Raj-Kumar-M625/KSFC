using System;

namespace Presentation.Extensions.Payment
{
    public class BankFileFilters
    {
        public string[] forder { get; set; }
        public int noOfVendors { get; set; }
        public int noOfTransactions { get; set; }
        public string fileGenDate { get; set; }
        public string bankName { get; set; }
        public string accountNo { get; set; }
        public decimal paidAmount { get; set; }
        public string paymentStatus { get; set; }
     
    }
}
