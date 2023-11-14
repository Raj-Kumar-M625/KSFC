using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EntityCreateModel
    {
        public long Id { get; set; }

        public long EmployeeId { get; set; }

        public long DayId { get; set; }
        [Required(ErrorMessage = "Village is required")]

        public string HQCode { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        [Required(ErrorMessage = "Select Employee")]

        public string EmployeeName { get; set; }

        [Display(Name = "At Business")]
        public bool AtBusiness { get; set; } = true;

        [Display(Name = "Client Type")]
        public string EntityType { get; set; }

        [Display(Name = "Client Name")]
        [Required(ErrorMessage = "Farmer Name is Required")]
        [RegularExpression(@"^[a-zA-Z ]{2,50}$", ErrorMessage = "Invalid Name")]
        [MinLength(3, ErrorMessage = "Enter atleast 3 characters.")]
        public string EntityName { get; set; }

        [Display(Name = "Date")]
        public System.DateTime EntityDate { get; set; }

       
        [Display(Name = "Land Size")]
        [Required(ErrorMessage = "Land Size is Required")]
        [Range(0.01, 9999.99, ErrorMessage = "Invalid Land Size")]
        public string LandSize { get; set; }

        [RegularExpression(@"^-?[0-9]+(\.[0-9]{1,9})$", ErrorMessage = "Valid Latitude with maximum 9 decimal places.")]
        [Required(ErrorMessage = "Latitude Required")]
        public decimal Latitude { get; set; }

        [RegularExpression(@"^-?[0-9]+(\.[0-9]{1,9})$", ErrorMessage = "Valid Longitude with maximum 9 decimal places.")]
        [Required(ErrorMessage = "Longitude Required")]
        public decimal Longitude { get; set; }

        public long SqliteEntityId { get; set; }

        [Display(Name = "Contact Count")]
        public int ContactCount { get; set; }

        [Display(Name = "Crop Count")]
        public int CropCount { get; set; }

        [Display(Name = "Unique Id Type")]
        public string UniqueIdType { get; set; }

        [RegularExpression(@"^[1-9][0-9]{11}$", ErrorMessage = "Enter 12 Digits")]
        [Required(ErrorMessage = "Aadhar is Required")]
        [Display(Name = "Unique Id")]
        public string UniqueId { get; set; }

        [Display(Name = "Tax Id")]
        [RegularExpression(@"^[0-9a-zA-Z ]{0,50}$", ErrorMessage = "Invalid TaxId")]
        public string TaxId { get; set; }

        [Display(Name = "Father/Husband Name")]
        [MaxLength(50, ErrorMessage = "Father/Husband name can be maximum 50 characters.")]
        [Required(ErrorMessage = "Father/Husband Name is Required")]
        [MinLength(3, ErrorMessage = "Enter atleast 3 characters.")]
        public string FatherHusbandName { get; set; }

        [Display(Name = "Village Name")]
        public string HQName { get; set; }

        [Display(Name = "Cluster Code")]
        [Required(ErrorMessage = "Cluster is required")]

        public string TerritoryCode { get; set; }

        [Display(Name = "Cluster Name")]
        public string TerritoryName { get; set; }


        public string ZoneName { get; set; }
        [Required(ErrorMessage = "Zone is required")]
        public string ZoneCode { get; set; }

        public string AreaName { get; set; }
        [Required(ErrorMessage = "Area is required")]

        public string AreaCode { get; set; }

        [Required(ErrorMessage = "Select Atleast one crop")]
        public IEnumerable<string> Crops { get; set; }

        public string EntityNumber { get; set; }

        [MinLength(10, ErrorMessage = "Enter 10 digits number")]
        [Required(ErrorMessage = "Phone Number is Required")]
        public string Number { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public bool IsEditAllowed { get; set; }
    }
}