using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class FollowUpTask
    {
        public long Id { get; set; }
        public long XRefProjectId { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public string ActivityType { get; set; }
        public string ClientType { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public string Comments { get; set; }
        public string Status { get; set; }
        public int CyclicCount { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public string CurrentUser { get; set; }
        public bool IsCreatedOnPhone { get; set; }
        public long ActivityCount { get; set; }
    }
}
