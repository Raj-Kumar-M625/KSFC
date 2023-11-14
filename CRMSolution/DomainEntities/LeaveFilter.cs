using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public  class LeaveFilter :BaseSearchCriteria
    {
        public bool ApplyEmployeeCodeFilter { get; set; }
        public string EmployeeCode { get; set; }

        public bool ApplyEmployeeNameFilter { get; set; }
        public string EmployeeName { get; set; }

        public bool ApplyDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public bool ApplyLeaveDurationFilter { get; set; }
        public int LeaveDuration { get; set; }

        public bool ApplyLeaveTypeFilter { get; set; }
        public string LeaveType { get; set; }

        public bool ApplyLeaveStatusFilter { get; set; }
        public string LeaveStatus { get; set; }

    }
}
