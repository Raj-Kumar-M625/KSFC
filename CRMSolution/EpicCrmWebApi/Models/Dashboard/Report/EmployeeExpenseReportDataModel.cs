using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EmployeeExpenseReportDataModel
    {
        [Display(Name = "Employee Code")]
        public string StaffCode { get; set; }

        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        [Display(Name = "Date")]
        public DateTime ExpenseDate { get; set; }

        [Display(Name = "Mode & Class of Fare")]
        public decimal ModeAndClassOfFare { get; set; }

        [Display(Name = "Lodge Expenses")]
        public decimal LodgeRent { get; set; }

        [Display(Name = "Local Conveyance")]
        public decimal LocalConveyance { get; set; }

        [Display(Name = "Outstation Conveyance")]
        public decimal OutstationConveyance { get; set; }

        [Display(Name = "Incdl Charges")]
        public decimal IncdlCharges { get; set; }

        [Display(Name = "Communication Expenses")]
        public decimal CommunicationExpenses { get; set; }

        [Display(Name = "Start Place")]
        public string StartPosition { get; set; }

        [Display(Name = "End Place")]
        public string EndPosition { get; set; }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }

        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Total Exp.")]
        public decimal TotalExpenseAmount =>
            ModeAndClassOfFare + LodgeRent + LocalConveyance + IncdlCharges + CommunicationExpenses + OutstationConveyance;

        public string Department { get; set; }
        public string Designation { get; set; }

        [Display(Name = "Zone Code")]
        public string ZoneCode { get; set; }
        [Display(Name = "Zone Name")]
        public string ZoneName { get; set; }
        [Display(Name = "Area Code")]
        public string AreaCode { get; set; }
        [Display(Name = "Area Name")]
        public string AreaName { get; set; }
        [Display(Name = "Territory Code")]
        public string TerritoryCode { get; set; }
        [Display(Name = "Territory Name")]
        public string TerritoryName { get; set; }
        [Display(Name = "HQ Code")]
        public string HQCode { get; set; }
        [Display(Name = "HQ Name")]
        public string HQName { get; set; }

        public decimal TrackingDistanceInMeters { get; set; }
    }
}