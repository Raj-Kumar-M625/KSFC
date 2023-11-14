using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteFollowUpTask : SqliteBase
    {
        public string Id { get; set; }
        public bool IsNewEntity { get; set; }
        public string ParentReferenceId { get; set; }  // fk to ServerEntity.Id or 
        public string ProjectName { get; set; }
        public long ProjectId { get; set; } // Default Project Id
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

    }
}