using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EmployeeDailyConsolidationModel
    {
        public long EmployeeId { get; set; }
        public long DayId { get; set; }

        [Display(Name = "Employee Code")]
        public string StaffCode { get; set; }
        [Display(Name = "Order")]
        public decimal TotalOrderAmount { get; set; }
        [Display(Name = "Return")]
        public decimal TotalReturnOrderAmount { get; set; }
        [Display(Name = "Total Exp.")]
        public decimal TotalExpenseAmount { get; set; }
        [Display(Name = "Payment")]
        public decimal TotalPaymentAmount { get; set; }
        [Display(Name = "Act.Count")]
        public long ActivityCount { get; set; }

        [Display(Name = "Start Place")]
        public string StartPosition { get; set; }
        [Display(Name = "End Place")]
        public string EndPosition { get; set; }

        [Display(Name = "Start Time")]
        public DateTime StartTime { get; set; }
        [Display(Name = "End Time")]
        public DateTime EndTime { get; set; }


        [Display(Name = "Google KM")]
        public decimal TrackingDistanceInMeters { get; set; }
    }
}