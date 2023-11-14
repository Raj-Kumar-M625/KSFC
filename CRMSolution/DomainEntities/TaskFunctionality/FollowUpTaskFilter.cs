using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class FollowUpTaskFilter : BaseSearchCriteria
    {
        public bool ApplyTaskFilter { get; set; }
        public string TaskDescription { get; set; }

        public bool ApplyProjectFilter { get; set; }
        public string ProjectName { get; set; }

        public bool ApplyClientTypeFilter { get; set; }
        public string ClientType { get; set; }

        public bool ApplyClientNameFilter { get; set; }
        public string ClientName { get; set; }
        public bool ApplyActivityTypeFilter { get; set; }
        public string ActivityType { get; set; }

        public bool ApplyDateFilter { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }

        public bool ApplyCreatedByFilter { get; set; }
        public string CreatedBy { get; set; }

        public bool ApplyUpdatedByFilter { get; set; }
        public string UpdatedBy { get; set; }

        //For Employee Status
        public bool ApplyEmployeeStatusFilter { get; set; }
        public bool EmployeeStatus { get; set; }

        public bool ApplyTaskStatusFilter { get; set; }
        public string TaskStatus { get; set; }

        public bool ApplyActiveFilter { get; set; }
        public bool IsActive { get; set; }

    }
}
