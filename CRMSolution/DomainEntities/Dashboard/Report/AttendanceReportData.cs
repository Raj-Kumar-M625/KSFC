using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class AttendanceData
    {
        public long TrackingId { get; set; }
        public long TenantEmployeeId { get; set; }
        public string Name { get; set; }
        public long DayId { get; set; }
        public DateTime Date { get; set; }
        public DateTime At { get; set; }
        public long EmployeeDayId { get; set; }
        public bool IsStartOfDay { get; set; }
        public bool IsEndOfDay { get; set; }
        public string StaffCode { get; set; }
        public string ExpenseHQCode { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }

        public decimal GoogleMapsDistanceInMeters { get; set; }

        public long ActivityCount { get; set; }
    }

    public class AbsenteeData
    {
        public long TenantEmployeeId { get; set; }
        public string Name { get; set; }
        public long DayId { get; set; }
        public DateTime Date { get; set; }
        public string StaffCode { get; set; }
        public string ExpenseHQCode { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public bool IsActiveInSap { get; set; }
        public DateTime SignupDate { get; set; }
    }

    public class AppSignUpData
    {
        public long TenantEmployeeId { get; set; }
        public string Name { get; set; }
        public DateTime SignupDate { get; set; }
        public string StaffCode { get; set; }
        public string ExpenseHQCode { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }

        public DateTime LastAccessDate { get; set; }
        public string AppVersion { get; set; }
        public string PhoneModel { get; set; }
        public string PhoneOS { get; set; }
    }

    public class DeviceInfo
    {
        public long TenantEmployeeId { get; set; }
        public DateTime LastAccessDate { get; set; }
        public string AppVersion { get; set; }
        public string PhoneModel { get; set; }
        public string PhoneOS { get; set; }
    }

    public class AppSignInData
    {
        public long TenantEmployeeId { get; set; }
        public string Name { get; set; }
        public string StaffCode { get; set; }
        public string ExpenseHQCode { get; set; }
        public long DaysActive { get; set; }
        public string Phone { get; set; }
        public bool IsActive { get; set; }
        public DateTime SignupDate { get; set; }
        public string Department { get; set; }
        public string Designation { get; set; }
        public bool IsActiveInSap { get; set; }
    }

    public class AttendanceReportData
    {
        public bool IsEmpty { get; set; }
        public long RefStartTrackingId { get; set; }
        public long RefEndTrackingId { get; set; }

        public long TenantEmployeeId { get; set; }
        public string Name { get; set; }
        public long DayId { get; set; }
        public DateTime Date { get; set; }
        public long EmployeeDayId { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string StaffCode { get; set; }
        public string ExpenseHQCode { get; set; }
        public string StartLocation { get; set; }
        public string EndLocation { get; set; }

        public double Hours { get; set; }

        public long ActivityCount { get; set; }
        public decimal DistanceTravelled { get; set; }
    }
}
