using System;
using DomainEntities.Questionnaire;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DownloadProjects
    {
        public long Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectCategory { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public string ProjectStatus { get; set; }
        public DateTime AssignedStartDate { get; set; }
        public DateTime AssignedEndDate { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedBy { get; set; }
    }

    public class DownloadTasks
    {
        public long TaskId { get; set; }
        public long XRefProjectId { get; set; }
        public long TaskAssignmentId { get; set; }
        public string ProjectName { get; set; }
        public string TaskDescription { get; set; }
        public string ActivityType { get; set; }
        public string ClientType { get; set; }
        public string ClientName { get; set; }
        public string ClientCode { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime ActualStartDate { get; set; }
        public DateTime ActualEndDate { get; set; }
        public string Comments { get; set; }
        public string TaskStatus { get; set; }
        public DateTime AssignedStartDate { get; set; }
        public DateTime AssignedEndDate { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedBy { get; set; }
        public bool IsSelfAssigned { get; set; }
        public bool IsCreatedOnPhone { get; set; }

    }

}
