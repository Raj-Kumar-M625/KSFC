using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainTask
    {
        public bool IsNewEntity { get; set; }
        public long EntityId { get; set; }
        public string ParentReferenceId { get; set; }
        public string ProjectName { get; set; }
        public long ProjectId { get; set; }
        public string Description { get; set; }
        public string ActivityType { get; set; }
        public string ClientType { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public string PhoneDbId { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime NotificationDate { get; set; }
    }

    public class SqliteTaskData : SqliteDomainTask
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsProcessed { get; set; }
        public long TaskId { get; set; }
        public long TaskAssignmentId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
    }
}