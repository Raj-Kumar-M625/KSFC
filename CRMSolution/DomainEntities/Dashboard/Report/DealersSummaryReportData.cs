using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DealersSummaryReportData : OfficeHierarchy
    {
        public string StaffCode { get; set; }
        public string EmployeeName { get; set; }
        public string BranchName { get; set; }
        public string DivisionName { get; set; }
        public int TotalDealersCount { get; set; }
        public int GeoTagCompleted { get; set; }
        public int GeoTagsPending { get; set; }
        public bool EmployeeStatus { get; set; }
        public bool EmployeeStatusInsp { get; set; }

    }

    public class DealersGeoTagCount
    {
        public string StaffCode { get; set; }
        public int GeoTagCount { get; set; }

    }

}
