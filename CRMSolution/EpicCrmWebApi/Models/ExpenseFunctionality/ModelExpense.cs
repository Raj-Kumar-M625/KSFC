using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ModelExpense
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        public long DayId { get; set; }

        [Display(Name = "Date")]
        public DateTime ExpenseDate { get; set; }

        [Display(Name = "Amount (Rs.)")]
        public decimal TotalAmount { get; set; }

        [Display(Name = "Zone Approved")]
        public bool IsZoneApproved { get; set; }

        [Display(Name = "Area Approved")]
        public bool IsAreaApproved { get; set; }

        [Display(Name = "Territory Approved")]
        public bool IsTerritoryApproved { get; set; }

        public IEnumerable<ModelExpenseItem> Items { get; set; }
        public IEnumerable<ModelExpenseApproval> Approvals { get; set; }

        public long EmployeeDayId { get; set; }
        public int ActivityCount { get; set; }

        [Display(Name = "Employee Phone")]
        public string Phone { get; set; }
    }
}