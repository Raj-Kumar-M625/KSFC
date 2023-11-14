using Omu.AwesomeMvc;

namespace Presentation.GridFilters.Adjustment
{
    public class AdjustmentFilterRow
    {
        public KeyContent[] CreatedBy { get; set; }
        public KeyContent[] ApprovedBy { get; set; }
        public KeyContent[] PaymentStatus { get; set; }
    }
}
