using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi.Models
{
    public class LeaveModel
    {
        public string Id { get; set; }

        [Display(Name = "Leave Types")]
        public string LeaveType { get; set; }

        [Display(Name = "Leave Duration")]
        public string LeaveDuration { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode {get;set;}

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

    }
}
