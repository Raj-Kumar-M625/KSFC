using System;
using System.ComponentModel.DataAnnotations;


namespace EpicCrmWebApi
{
    public class DealersSummaryReportDataModel
    {
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Employee Branch")]
        public string BranchName { get; set; }

        [Display(Name = "Employee Division")]
        public string DivisionName { get; set; }

        [Display(Name = "Total Dealers Count")]
        public int TotalDealersCount { get; set; }

        [Display(Name = "Geo Tags Completed")]
        public int GeoTagCompleted { get; set; }

        [Display(Name = "Geo Tags Pending")]
        public int GeoTagsPending { get; set; }

        [Display(Name = "Employee Status")]
        public bool  EmployeeStatus { get; set; }
    }
}
