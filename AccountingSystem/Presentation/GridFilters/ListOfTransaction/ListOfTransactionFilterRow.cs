using Omu.AwesomeMvc;

namespace Presentation.GridFilters.ListOfTransaction
{
    public class ListOfTransactionFilterRow
    {
        public KeyContent[] VendorName { get; set; }
        public KeyContent[] transactionType { get; set; }
        public KeyContent[] Status { get; set; }
    }
}
