using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DealersNotMetReportDataModel
    {
        [Display(Name = "Customer Code")]
        public string CustomerCode { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }
        [Display(Name = "Last Activity")]
        public string LastActivity { get; set; }
        [Display(Name = "HQ Code")]
        public   string HQCode { get; set; }
        [Display(Name = "HQ Name")]
        public   string HQName { get; set; }
        [Display(Name = "Contact Number")]
        public string ContactNumber { get; set; }
        [Display(Name = "Active")]
        public bool IsActive { get; set; }
    }
}