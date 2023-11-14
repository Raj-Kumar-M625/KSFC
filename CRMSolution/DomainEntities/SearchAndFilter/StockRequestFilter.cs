using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class StockRequestFilter : BaseSearchCriteria
    {
        public bool ApplyStockRequestTagIdFilter { get; set; }
        public long StockRequestTagId { get; set; }

        public bool ApplyStockRequestIdFilter { get; set; }
        public long StockRequestId { get; set; }

        public bool ApplyRequestNumberFilter { get; set; }
        public bool IsExactRequestNumberMatch { get; set; }
        public string RequestNumber { get; set; }

        public bool ApplyDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public bool ApplyStatusFilter { get; set; }
        public string Status { get; set; }

        public bool ApplyEmployeeCodeFilter { get; set; }
        public string EmployeeCode { get; set; }

        public bool ApplyEmployeeNameFilter { get; set; }
        public string EmployeeName { get; set; }

        public bool ApplyItemTypeFilter { get; set; }
        public string ItemType { get; set; }

        public bool ApplyItemIdFilter { get; set; }
        public long ItemId { get; set; }

        public string StockRequestType { get; set; }
        public string TagRecordStatus { get; set; }
    }
}
