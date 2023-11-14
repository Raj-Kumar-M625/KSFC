using System;
using System.ComponentModel.DataAnnotations;


namespace EpicCrmWebApi
{
    public class GeoTagReportDataModel
    {
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Employee Division")]
        public string DivisionName { get; set; }
        [Display(Name = "Employee Zone")]
        public string ZoneName { get; set; }

        [Display(Name = "Employee Branch")]
        public string BranchName { get; set; }

        [Display(Name = "Client Name")]
        public string CustomerName { get; set; }

        [Display(Name = "Client Code")]
        public string CustomerCode { get; set; }

        [Display(Name = "Client Type")]
        public string CustomerType { get; set; }

        [Display(Name = "Geo Tagging Status")]
        public bool GeoTagStatus { get; set; }
    }
}