using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class LeaveFilter
    {
        public string Zone { get; set; }
        public string Area { get; set; }
        public string Territory { get; set; }
        public string HQ { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public int LeaveDuration { get; set; }
        public string LeaveType { get; set; }
        public string LeaveStatus { get; set; }

    }
}