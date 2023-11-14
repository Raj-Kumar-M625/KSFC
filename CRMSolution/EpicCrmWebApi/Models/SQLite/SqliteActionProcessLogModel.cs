using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteActionProcessLogModel
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        [Display(Name = "Able to Run?")]
        public bool LockAcquiredStatus { get; set; }
        [Display(Name = "Initated (IST)")]
        public System.DateTime At { get; set; }
        [Display(Name = "Last Update (IST)")]
        public System.DateTime Timestamp { get; set; }

        [Display(Name = "Completed?")]
        public bool HasCompleted { get; set; }
        [Display(Name = "Failed?")]
        public bool HasFailed { get; set; }

        public string ProcessName { get; set; }
    }
}