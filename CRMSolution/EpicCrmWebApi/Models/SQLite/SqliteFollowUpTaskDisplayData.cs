using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteFollowUpTaskDisplayData
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsNewEntity { get; set; }
        public string ParentReferenceId { get; set; }  // fk to ServerEntity.Id or 
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string ActivityType { get; set; }
        public string ClientType { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public DateTime FollowUpStartDate { get; set; }
        public DateTime FollowUpEndDate { get; set; }
        public string Comments { get; set; }
        public string FollowUpStatus { get; set; }
        public DateTime TimeStamp { get; set; }
        public DateTime NotificationDate { get; set; }
        public string PhoneDbId { get; set; }
        public bool IsProcessed { get; set; }
        public long TaskId { get; set; }
        public long TaskAssignmentId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }

    }
}