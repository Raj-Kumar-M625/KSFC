using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Expense
    {
        public long Id { get; set; }
        public string ReportType { get; set; }

        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public long DayId { get; set; }
        public DateTime ExpenseDate { get; set; }
        public decimal TotalAmount { get; set; }
        public string StaffCode { get; set; }

        public bool IsZoneApproved { get; set; }
        public bool IsAreaApproved { get; set; }
        public bool IsTerritoryApproved { get; set; }

        public ICollection<ExpenseApproval> Approvals { get; set; }
        public decimal ZoneApprovedAmount { get; set; }
        public decimal AreaApprovedAmount { get; set; }
        public decimal TerritoryApprovedAmount { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public bool IsActiveInSap { get; set; }
    }

    public class BulkExpense
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public decimal TotalAmount { get; set; }

        public ICollection<ExpenseApproval> Approvals { get; set; }

    }
}
