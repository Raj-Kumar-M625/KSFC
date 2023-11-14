using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EmployeeStatsMinResponse : MinimumResponse
    {
        public long TenantId { get; set; }
        public string ReportDate { get; set; }
        public string Imei { get; set; }
    }

    public class EmployeeStatsSummaryWithDetailResponse : EmployeeStatsMinResponse
    {
        public bool IsGlobal { get; set; }
        public string DerivedRollUp { get; set; }
        public long ItemCount { get; set; }
        public int CurrentLevel { get; set; }
        public int NextLevel { get; set; }
        public bool IsDrillDownSupported { get; set; }
        public IEnumerable<SmsSummaryDataModel> SummaryWithDetail { get; set; }
    }

    public class ExecutiveStatsResponse : EmployeeStatsMinResponse
    {
        public IEnumerable<OfficeHierarchy> OfficeHierarchy { get; set; }
        public IEnumerable<CodeTable> Divisions { get; set; }
        public IEnumerable<CodeTable> Segments { get; set; }
        public IEnumerable<DivisionSegment> DivisionSegments { get; set; }
        public IEnumerable<SalesPersonMiniModel> SalesPersons { get; set; }

        public IEnumerable<DownloadStaffDailyData> StaffDailyData { get; set; }

        // list of HQs for each staff code, based on assignment
        public IEnumerable<AssignedHQ> AssignedHQs { get; set; }

        public IEnumerable<DownloadCustomer> Customers { get; set; }
        public IEnumerable<CustomerDivisionBalance> CustomerDivisionBalances { get; set; }
        public IEnumerable<StaffDivision> StaffDivisions { get; set; }

        public IEnumerable<PPA> PPAData { get; set; }
    }
}