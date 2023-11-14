using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class AttendanceReportModel
    {
        public IEnumerable<AttendanceReportDataModel> AttendanceReportData { get; set; }
        public IEnumerable<OfficeHierarchyModel> OfficeHierarchy { get; set; }
        public ICollection<EmployeeRecord> AppUsers { get; set; }
        public ICollection<DayRecord> ReportDays { get; set; }
    }
}