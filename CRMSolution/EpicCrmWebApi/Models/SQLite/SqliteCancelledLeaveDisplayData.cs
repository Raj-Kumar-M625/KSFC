using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteCancelledLeaveDisplayData
    {
        public long Id { get; set; }

        [Display(Name= "Batch Id")]
        public long BatchId { get; set; }
        [Display(Name= "Employee Id")]
        public long EmployeeId { get; set; }
        [Display(Name= "Leave Id")]
        public long LeaveId { get; set; }
        [Display(Name= "Is Processed")]
        public bool IsProcessed { get; set; }
    }
}