using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class StockLedgerFilter : BaseSearchCriteria
    {
        public bool ApplyRecordIdFilter { get; set; }
        public long RecordId { get; set; }

        public bool ApplyReferenceNumberFilter { get; set; }
        public string ReferenceNumber { get; set; }

        public bool ApplyDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public bool ApplyItemTypeFilter { get; set; }
        public string ItemType { get; set; }

        public bool ApplyItemIdFilter { get; set; }
        public long ItemId { get; set; }

        public bool ApplyEmployeeCodeFilter { get; set; }
        public string EmployeeCode { get; set; }

        public bool ApplyEmployeeNameFilter { get; set; }
        public string EmployeeName { get; set; }
    }
}
