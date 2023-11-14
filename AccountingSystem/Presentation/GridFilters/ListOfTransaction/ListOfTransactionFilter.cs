namespace Presentation.GridFilters.ListOfTransaction
{
    public class ListOfTransactionFilter
    {
        public string vendorName { get; set; }
        public decimal maximumAmount { get; set; }
        public decimal minimumAmount { get; set; }
        public string assessmentYear { get; set; }
        public string transactionType { get; set; }
        public string transactionDate { get; set; }
        public string referenceNumber { get; set; }
        public string status { get; set; } 
        public string approvedBy { get; set; } 
        public string billNo { get; set; } 
        public string accountNumber { get; set; } 
        public string[] forder { get; set; }
        public string UTRNumber { get; set; }
        public string name { get; set; }
    }
}
