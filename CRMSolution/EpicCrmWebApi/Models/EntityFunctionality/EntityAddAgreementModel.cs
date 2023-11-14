using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CRMUtilities;
using System.Web.Http.ModelBinding;
//using System.Web.Mvc;

namespace EpicCrmWebApi
{
    public class EntityAddAgreementModel 
    {
        public long EntityId { get; set; }

        //[RegularExpression(@"^[1-9]$", ErrorMessage = "Select Season")]

        [Required(ErrorMessage = "Select Crop Name")]
        [Display(Name = "Crop Name")]
        public long WorkflowSeasonId { get; set; }

        [Required(ErrorMessage = "Enter Land Size")]
        [Display(Name = "Land Size (Acres)")]
        public decimal LandSizeInAcres { get; set; }

        [Display(Name = "Employee Name")]
        [Required]
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        [Display(Name = "Zone")]
        [Required]
        public string ZoneCode { get; set; }
        public string ZoneName { get; set; }
        [Required]
        [Display(Name = "Area")]
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        [Required]
        [Display(Name = "Cluster")]
        public string TerritoryCode { get; set; }
        public string TerritoryName { get; set; }
        [Required]
        [Display(Name = "Village")]
        public string HQCode { get; set; }
        public string HQName { get; set; }

        public DateTime PassBookReceivedDate { get; set; }

    }
}