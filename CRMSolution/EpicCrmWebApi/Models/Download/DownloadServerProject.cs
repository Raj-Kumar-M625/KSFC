using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DownloadServerProjects
    {
        public long Id { get; set; }
        public string ProjectName { get; set; }
        public string ProjectDescription { get; set; }
        public string ProjectCategory { get; set; }
        public string PlannedStartDate { get; set; }
        public string PlannedEndDate { get; set; }
        public string ActualStartDate { get; set; }
        public string ActualEndDate { get; set; }
        public string ProjectStatus { get; set; }
        public string AssignedStartDate { get; set; }
        public string AssignedEndDate { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedBy { get; set; }
    }

    public class DownloadServerTasks
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
        public string PlannedStartDate { get; set; }
        public string PlannedEndDate { get; set; }
        public string ActualStartDate { get; set; }
        public string ActualEndDate { get; set; }
        public string Comments { get; set; }
        public string TaskStatus { get; set; }
        public string AssignedStartDate { get; set; }
        public string AssignedEndDate { get; set; }
        public string CreatedBy { get; set; }
        public string AssignedBy { get; set; }

    }
}