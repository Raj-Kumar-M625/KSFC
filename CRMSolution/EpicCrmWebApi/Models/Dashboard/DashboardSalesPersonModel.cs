using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DashboardSalesPersonModel
    {
        public int Id { get; set; }

        [Display(Name="Employee Code")]
        public string StaffCode { get; set; }

        public string Name { get; set; }
        public string Phone { get; set; }
        [Display(Name = "Active (SAP)")]
        public bool IsActive { get; set; }

        [Display(Name = "CRM Id")]
        public long EmployeeId { get; set; }

        [Display(Name = "Portal")]
        public bool OnWeb { get; set; }

        [Display(Name = "HQ Code")]
        public string HQCode { get; set; }
        [Display(Name = "HQ Name")]
        public string HQName { get; set; }

        public string Grade { get; set; }

        public string Department { get; set; }
        public string Designation { get; set; }
        public string Ownership { get; set; }

        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; }

        [Display(Name = "Fuel Type")]
        public string FuelType { get; set; }

        [Display(Name = "Vehicle Number")]
        public string VehicleNumber { get; set; }
    }
}