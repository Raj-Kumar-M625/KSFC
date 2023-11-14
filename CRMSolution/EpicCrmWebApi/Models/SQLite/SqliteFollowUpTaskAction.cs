using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteFollowUpTaskAction : SqliteBase
    {
        public string Id { get; set; }
        public bool IsNewTask { get; set; }
        public long TaskId { get; set; }
        public string ParentReferenceTaskId { get; set; }
        public long TaskAssignmentId { get; set; }
        public string SqliteActionPhoneDbId { get; set; }
        public string Status { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}