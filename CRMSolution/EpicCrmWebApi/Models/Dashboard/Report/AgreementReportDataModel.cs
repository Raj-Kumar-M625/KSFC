using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace EpicCrmWebApi
{
    public class AgreementReportDataModel
    {
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Contact Count")]
        public int ContactCount { get; set; }
        [Display(Name = "Image Count")]
        public int ImageCount { get; set; }
        [Display(Name = "Season Name")]
        public string WorkflowSeasonName { get; set; }
        [Display(Name = "UniqueId Type")]
        public string UniqueIdType { get; set; }
        [Display(Name = "UniqueId")]
        public string UniqueId { get; set; }
        [Display(Name = "Father /Husband Name")]
        public string FatherHusbandName { get; set; }
        [Display(Name = "Active")]
        public string IsActive { get; set; }
        [Display(Name = "Agreement #")]
        public string AgreementNumber { get; set; }
        [Display(Name = "Agreement Status")]
        public string Status { get; set; }
        [Display(Name = "Agreement Acers")]
        public decimal LandSize { get; set; }
        [Display(Name = "Latitude")]
        public decimal Latitude { get; set; }
        [Display(Name = "Longitude")]
        public decimal Longitude { get; set; }
        [Display(Name = "At Business")]
        public string AtBusiness { get; set; }
        [Display(Name = "Bank Details Count")]
        public int BankDetailCount { get; set; }
        [Display(Name = "Agreement Date")]
        public System.DateTime EntityDate { get; set; }
        [Display(Name = "Zone Name")]
        public string ZoneName { get; set; }
        [Display(Name = "Zone Code")]
        public string ZoneCode { get; set; }
        [Display(Name = "Crop Name")]
        public string CropName { get; set; }
        [Display(Name = "Area Name")]
        public string AreaName { get; set; }
        [Display(Name = "Area Code")]
        public string AreaCode { get; set; }
        [Display(Name = "Territory Name")]
        public string TerritoryName { get; set; }

        [Display(Name = "Territory Code")]
        public string TerritoryCode { get; set; }

        [Display(Name = "Cluster Name")]
        public string ClusterName { get; set; }

        [Display(Name = "Cluster Code")]
        public string ClusterCode { get; set; }

        [Display(Name = "Village Name")]
        public string VillageName { get; set; }
        [Display(Name = "Client Name")]
        public string EntityName { get; set; }
        [Display(Name = "HQ Name")]
        public string HQName { get; set; }
        [Display(Name = "HQ Code")]
        public string HQCode { get; set; }
        [Display(Name = "Rate/Kg(Rs)")]
        public decimal RatePerKg { get; set; }
    }
}