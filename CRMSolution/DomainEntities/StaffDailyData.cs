using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class StaffDailyData
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public long TenantId { get; set; }
        public string StaffCode { get; set; }
        public string DivisionCode { get; set; }
        public string SegmentCode { get; set; }
        public string AreaCode { get; set; }
        public decimal TargetOutstandingYTD { get; set; }
        public decimal TotalCostYTD { get; set; }
        public decimal CGAYTD { get; set; }
        public decimal GT180YTD { get; set; }
        public decimal CollectionTargetYTD { get; set; }
        public decimal CollectionActualYTD { get; set; }
        public decimal SalesTargetYTD { get; set; }
        public decimal SalesActualYTD { get; set; }
    }
}
