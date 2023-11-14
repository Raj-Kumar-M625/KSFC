using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DashboardReportModel
    {
        public ICollection<EmployeeRecord> AppUsers { get; set; }
        public ICollection<DayRecord> ReportDays { get; set; }
        public ICollection<DashboardDataSet> DashboardDataSet { get; set; }
        public DateTime ReportStartDate { get; set; }
        public DateTime ReportEndDate { get; set; }

        // added on 20.06.19 for Distance Report
        public IEnumerable<OfficeHierarchyModel> OfficeHierarchy { get; set; }
    }
}