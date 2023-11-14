using DocumentFormat.OpenXml.Drawing.Charts;
using System;

namespace Presentation.GridFilters.Reconcile
{
    public class ReconcileFilter
    {
        public string Date { get; set; }
        public string Status { get; set; }
        public string Bankname { get; set; }
        public decimal PayableAmount { get; set; }
        public decimal PayableMaxAmount { get; set; }
        public string TransactionType { get; set; }
        public string AccountNumber { get; set; }
        public string Description { get; set; }
        public string Reconcile { get; set; }
        public string[] forder { get; set; }
    }
}
