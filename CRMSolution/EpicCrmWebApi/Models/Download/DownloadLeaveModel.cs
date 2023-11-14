using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadLeaveModel
    {
        public long Id { get; set; }
        public int DaysCountExcludingHolidays{ get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string LeaveType { get; set; }
        public string LeaveReason { get; set; }
        public string Comment { get; set; }
        public string ApproverNotes { get; set; }
        public string LeaveStatus { get; set; }
    }
}