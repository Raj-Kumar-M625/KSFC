using System;
using System.ComponentModel.DataAnnotations;

namespace EpicCrmWebApi
{
    public class DayPlanReportDataModel
    {
        public long EmployeeId { get; set; }
        public long DayId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string Name { get; set; }

        [Display(Name = "Date")]
        public DateTime Date { get; set; }

        [Display(Name = "Actual Sales (Rs)")]
        public decimal ActualSalesAmount { get; set; }

        [Display(Name = "Target Sales (Rs)")]
        public decimal TargetSalesAmount { get; set; }

        [Display(Name = "Actual Collection (Rs)")]
        public decimal ActualCollectionAmount { get; set; }

        [Display(Name = "Target Collection (Rs)")]
        public decimal TargetCollectionAmount { get; set; }

        [Display(Name = "Actual Dealer Appointments")]
        public int ActualDealerAppt { get; set; }

        [Display(Name = "Target Dealer Appointments")]
        public int TargetDealerAppt { get; set; }

        [Display(Name = "Actual Demonstrations")]
        public int ActualDemoActivity { get; set; }

        [Display(Name = "Target Demonstrations")]
        public int TargetDemoActivity { get; set; }

        [Display(Name = "Actual Vigore Sales (kgs)")]
        public decimal ActualVigoreSales { get; set; }

        [Display(Name = "Target Vigore Sales(kgs)")]
        public decimal TargetVigoreSales { get; set; }
    }
}