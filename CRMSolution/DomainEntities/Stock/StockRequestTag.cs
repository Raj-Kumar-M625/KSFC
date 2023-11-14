using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Stock
    {
        public string ZoneCode { get; set; }
        public string AreaCode { get; set; }
        public string TerritoryCode { get; set; }
        public string HQCode { get; set; }
        public string StaffCode { get; set; }
        public string EmployeeName { get; set; }

        public string CreatedBy { get; set; }
        public DateTime DateCreated { get; set; }

        public string UpdatedBy { get; set; }
        public DateTime DateUpdated { get; set; }
    }

    public class StockRequestTag : Stock
    {
        public long Id { get; set; }

        public string RequestNumber { get; set; }
        public System.DateTime RequestDate { get; set; }

        public string Notes { get; set; }
        public string Status { get; set; }

        public int? ItemCountFromLines { get; set; }

        public long CyclicCount { get; set; }

        public string RequestType { get; set; }

        public bool IsEditAllowed => Status.Equals(StockStatus.Requested.ToString(), StringComparison.OrdinalIgnoreCase) ||
            Status.Equals(StockStatus.ClearRequest.ToString(), StringComparison.OrdinalIgnoreCase) ||
            Status.Equals(StockStatus.AddRequest.ToString(), StringComparison.OrdinalIgnoreCase)
            ;

        public string CurrentUser { get; set; }
    }
}
