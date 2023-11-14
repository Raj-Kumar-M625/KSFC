using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class FollowUpTaskAction
    {
        public int Id { get; set; }
        public long XRefTaskId { get; set; }
        public long XRefActivityId { get; set; }
        public long XRefTaskAssignmentId { get; set; }
        public long EmployeeId { get; set; }
        public System.DateTime TimeStamp { get; set; }
        public long SqliteTaskActionId { get; set; }
        public string Status { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

    }
}
