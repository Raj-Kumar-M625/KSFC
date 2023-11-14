using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class StockFulfillmentData
    {
        public long StockRequestId { get; set; }
        public long StockRequestTagId { get; set; }
        public long CyclicCount { get; set; }
        public string Status { get; set; }

        public string CurrentUser { get; set; }

        // used in case of fulfillment of stock
        public IEnumerable<StockBalance> StockBalances { get; set; }

        // used in case of remove stock
        public StockBalance StockBalanceRec { get; set; }

        public DateTime CurrentIstTime { get; set; }

        public string Notes { get; set; }
        public bool IsConfirmClicked { get; set; }
        public bool IsDenyClicked { get; set; }
    }
}
