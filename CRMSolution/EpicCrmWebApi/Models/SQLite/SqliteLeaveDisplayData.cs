using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteLeaveDisplayData
    {
        public long Id { get; set; }
        [Display(Name= "Batch Id")]
        public long BatchId { get; set; }
        [Display(Name= "Employee Id")]
        public long EmployeeId { get; set; }
        [Display(Name= "Phone DB Id")]
        public string PhoneDbId { get; set; }
        
        [Display(Name= "Start Date")]
        public System.DateTime StartDate { get; set; }
        [Display(Name= "End Date")]
        public System.DateTime EndDate { get; set; }
        [Display(Name= "Leave Type")]
        public string LeaveType { get; set; }
        [Display(Name= "Leave Reason")]
        public string LeaveReason { get; set; }
        [Display(Name= "Comment")]
        public string Comment { get; set; }
        [Display(Name= "DaysCountExcludingHolidays")]
        public int DaysCountExcludingHolidays { get; set; }

        [Display(Name = "DaysCount")]
        public int DaysCount{ get; set; }

        [Display(Name= "Is Processed")]
        public bool IsProcessed { get; set; }

        [Display(Name= "Leave Id")]
        public long LeaveId { get; set; }
    }
}