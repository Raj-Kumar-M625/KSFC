using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class AttendanceReportDataModel
    {
        public long RefStartTrackingId { get; set; }
        public long RefEndTrackingId { get; set; }

        public long TenantEmployeeId { get; set; }
        [Display(Name = "Employee Name")]
        public string Name { get; set; }
        public long DayId { get; set; }
        public DateTime Date { get; set; }
        public long EmployeeDayId { get; set; }

        [Display(Name="Start Time")]
        public DateTime StartTime { get; set; }
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }
        [Display(Name = "Employee Code")]
        public string StaffCode { get; set; }

        public string ExpenseHQCode { get; set; }

        [Display(Name = "Start Address")]
        public string StartLocation { get; set; }

        [Display(Name = "End Address")]
        public string EndLocation { get; set; }

        public double Hours { get; set; }

        [Display(Name = "Activity Count")]
        public long ActivityCount { get; set; }

        [Display(Name = "Distance Travelled (in km)")]
        public decimal DistanceTravelled { get; set; }
    }
}