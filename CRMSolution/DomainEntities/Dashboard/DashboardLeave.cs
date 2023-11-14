using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DashboardLeave
    {
        public long Id { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string LeaveType { get; set; }
        public string LeaveReason { get; set; }
        public string Comment { get; set; }
        public int DaysCountExcludingHolidays { get; set; }
        public int DaysCount { get; set; }
        public string LeaveStatus { get; set; }
        public string ApproveNotes { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string CurrentUser {get;set;}

        public string HQCode { get; set; }
        public bool IsEditAllowed => String.IsNullOrEmpty(LeaveStatus) || LeaveStatus.Equals("Pending");

    }
}
