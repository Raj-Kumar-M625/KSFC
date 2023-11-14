using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class AbsenteeReportDataModel
    {
        [Display(Name = "Employee Name")]
        public string Name { get; set; }
        public DateTime Date { get; set; }

        [Display(Name = "Employee Code")]
        public string StaffCode { get; set; }

        public string ExpenseHQCode { get; set; }

        public string Phone { get; set; }

        [Display(Name = "Employee")]
        public bool IsActive { get; set; }

        [Display(Name = "Signup Date")]
        public DateTime SignupDate { get; set; }
    }
}