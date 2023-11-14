using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class StockFilter : BaseSearchCriteria
    {
        public bool ApplyStockInputTagIdFilter { get; set; }
        public long StockInputTagId { get; set; }

        public bool ApplyVendorNameFilter { get; set; }
        public string VendorName { get; set; }

        public bool ApplyGRNNumberFilter { get; set; }
        public bool IsExactGrnNumberMatch { get; set; }
        public string GRNNumber { get; set; }

        public bool ApplyInvoiceNumberFilter { get; set; }
        public string InvoiceNumber { get; set; }

        public bool ApplyDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public bool ApplyStatusFilter { get; set; }
        public string Status { get; set; }

    }
}
