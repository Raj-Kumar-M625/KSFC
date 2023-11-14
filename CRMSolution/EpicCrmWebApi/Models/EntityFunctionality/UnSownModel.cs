using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class UnSownModel
    {
        public long Id { get; set; }

        public long EmployeeId { get; set; }

        public long DayId { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        public string HQCode { get; set; }

        [Display(Name = "Client Type")]
        public string EntityType { get; set; }

        [Display(Name = "Client Name")]
        public string EntityName { get; set; }

        [Display(Name = "Unique Id Type")]
        public string UniqueIdType { get; set; }

        [Display(Name = "Unique Id")]
        public string UniqueId { get; set; }

        [Display(Name = "Land Size")]
        public string LandSize { get; set; }
    }
}