using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class DayPlanData
    {
        public long ActivityId { get;  set;}
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public long DayId { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }

        [DefaultValue(0.0)]
        public decimal ActualSalesAmount { get; set; }
        public decimal TargetSalesAmount { get; set; }

        [DefaultValue(0.0)]
        public decimal ActualCollectionAmount { get; set; }
        public decimal TargetCollectionAmount { get; set; }

        [DefaultValue(0)]
        public int ActualDealerAppt { get; set; }
        public int TargetDealerAppt { get; set; }

        [DefaultValue(0)]
        public int ActualDemoActivity { get; set; }
        public int TargetDemoActivity { get; set; }

        [DefaultValue(0.0)]
        public decimal ActualVigoreSales { get; set; }
        public decimal TargetVigoreSales { get; set; }
    }

    public class OrderPaymentGroupData
    {
        public long EmployeeId { get; set; }
        public long DayId { get; set; }
        public DateTime Date { get; set; }

        [DefaultValue(0.0)]
        public decimal ActualSalesAmount { get; set; }

        [DefaultValue(0.0)]
        public decimal ActualCollectionAmount { get; set; }
    }

    public class ActivityTypeData
    {
        public string  ActivityType { get; set; }
        public long TenantEmployeeId { get; set; }
        public long EmployeeDayId { get; set; }
        public int ActivityCount { get; set; }

        public long DayId { get; set; }
    }

    public class DayPlanTargetData {
        public long TenantId { get; set; }
        public long EmployeeId { get; set; }
        public long DayId { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime Date { get; set; }

        [DefaultValue(0.0)]
        public decimal TargetSalesAmount { get; set; }

        [DefaultValue(0.0)]
        public decimal TargetCollectionAmount { get; set; }

        [DefaultValue(0)]
        public int TargetDealerAppt { get; set; }

        [DefaultValue(0.0)]
        public decimal TargetVigoreSales { get; set; }
        public long SqliteDayPlanTargetId { get; set; }
    }

    public class DayPlanReportData
    {
        public long EmployeeId { get; set; }
        public string EmployeeCode { get; set; }
        public long DayId { get; set; }
        public string Name { get; set; }
        public DateTime Date { get; set; }

        [DefaultValue(0.0)]
        public decimal ActualSalesAmount { get; set; }
        public decimal TargetSalesAmount { get; set; }

        [DefaultValue(0.0)]
        public decimal ActualCollectionAmount { get; set; }
        public decimal TargetCollectionAmount { get; set; }

        [DefaultValue(0)]
        public int ActualDealerAppt { get; set; }
        public int TargetDealerAppt { get; set; }

        [DefaultValue(0)]
        public int ActualDemoActivity { get; set; }
        public int TargetDemoActivity { get; set; }

        [DefaultValue(0.0)]
        public decimal ActualVigoreSales { get; set; }
        public decimal TargetVigoreSales { get; set; }
    }
}
