using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteFollowUpTaskActionDisplayData
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsNewTask { get; set; }
        public long TaskId { get; set; }
        public string ParentReferenceTaskId { get; set; }
        public long TaskAssignmentId { get; set; }
        public string SqliteActionPhoneDbId { get; set; }
        public string Status { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime NotificationDate { get; set; }
        public string PhoneDbId { get; set; }
        public bool IsProcessed { get; set; }
        public long TaskActionId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
    }
}