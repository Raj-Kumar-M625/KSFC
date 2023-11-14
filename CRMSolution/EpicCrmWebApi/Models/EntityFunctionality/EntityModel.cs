using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EntityModel
    {
        public long Id { get; set; }

        public long EmployeeId { get; set; }

        public long DayId { get; set; }

        public string HQCode { get; set; }

        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "At Business")]
        public bool AtBusiness { get; set; }

        [Display(Name = "Consent")]
        public bool Consent { get; set; }  //Swetha Made change on 24-11-2021

        [Display(Name = "Client Type")]
        public string EntityType { get; set; }

        [Display(Name = "Client Name")]
        [Required(ErrorMessage = "Client Name Required")]
        [RegularExpression(@"^[a-zA-Z ]{2,50}$", ErrorMessage = "Invalid Name")]
        public string EntityName { get; set; }

        [Display(Name = "Date")]
        public System.DateTime EntityDate { get; set; }

        public string Address { get; set; }

        [RegularExpression(@"^[a-zA-Z ]{0,50}$", ErrorMessage = "Invalid City Name")]
        public string City { get; set; }

        [RegularExpression(@"^[a-zA-Z ]{0,50}$", ErrorMessage = "Invalid State Name")]
        public string State { get; set; }

        [RegularExpression(@"^[1-9][0-9]{5}$", ErrorMessage = "Invalid Pincode")]
        public string Pincode { get; set; }
       
        [Display(Name = "Land Size")]
        [Required(ErrorMessage = "Land Size Required")]
        [Range(0.01, 9999.99, ErrorMessage = "Invalid Land Size")]
        public string LandSize { get; set; }

        // [RegularExpression(@"^-?([1-9]?[0-9])\.{1}\d{1,12}", ErrorMessage = "Invalid Latitude")]
        [DisplayFormat(DataFormatString = "{0:0.000000000}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^-?[0-9]+(\.[0-9]{1,9})$", ErrorMessage = "Valid Latitude with maximum 9 decimal places.")]
        [Required(ErrorMessage = "Latitude Required")]
        public decimal Latitude { get; set; }

        // [RegularExpression(@"^-{1,3}\d*\.{0,1}\d+$", ErrorMessage = "Invalid Longitude")]
        [DisplayFormat(DataFormatString = "{0:0.000000000}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^-?[0-9]+(\.[0-9]{1,9})$", ErrorMessage = "Valid Longitude with maximum 9 decimal places.")]
        [Required(ErrorMessage = "Longitude Required")]
        public decimal Longitude { get; set; }

        public long SqliteEntityId { get; set; }

        [Display(Name = "Contact Count")]
        public int ContactCount { get; set; }

        [Display(Name = "Crop Count")]
        public int CropCount { get; set; }

        [Display(Name = "Image Count")]
        public int ImageCount { get; set; }

        [Display(Name = "Agreement Count")]
        public int AgreementCount { get; set; }

        [Display(Name = "Bank Detail Count")]
        public int BankDetailCount { get; set; }

        [Display(Name = "Survey  Count")]
        public int SurveyDetailCount { get; set; }  // Added by swetha


        [Display(Name = "Unique Id Type")]
        public string UniqueIdType { get; set; }

        [RegularExpression(@"^[1-9][0-9]{11}$", ErrorMessage = "Enter 12 Digits")]
        [Required(ErrorMessage = "Unique Id Required")]
        [Display(Name = "Unique Id")]
        public string UniqueId { get; set; }

        [Display(Name = "Tax Id")]
        [RegularExpression(@"^[0-9a-zA-Z ]{0,50}$", ErrorMessage = "Invalid TaxId")]
        public string TaxId { get; set; }

        [Display(Name = "Father/Husband Name")]
        [MaxLength(50, ErrorMessage = "Father/Husband name can be maximum 50 characters.")]
        public string FatherHusbandName { get; set; }

        [Display(Name = "Village Name")]
        public string HQName { get; set; }

        [Display(Name = "Cluster Code")]
        public string TerritoryCode { get; set; }

        [Display(Name = "Cluster Name")]
        public string TerritoryName { get; set; }

        //[Display(Name = "Major Crop")]
        //public string MajorCrop { get; set; }

        //[Display(Name = "Last Crop")]
        //public string LastCrop { get; set; }

        //[Display(Name = "Water Source")]
        //public string WaterSource { get; set; }

        //[Display(Name = "Soil Type")]
        //public string SoilType { get; set; }

        //[Display(Name = "Sowing Type")]
        //[MaxLength(50, ErrorMessage = "Sowing Type name can be maximum 50 characters.")]
        //public string SowingType { get; set; }

        //[Display(Name = "Sowing Date")]
        //public DateTime SowingDate { get; set; }

        [Display(Name = "Entity Number")]
        public string EntityNumber { get; set; }

        [Display(Name = "Active?")]
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public int DWSCount { get; set; }
        public int IssueReturnCount { get; set; }
        public int AdvanceRequestCount { get; set; }

        public bool IsEditAllowed { get; set; }
    }
}