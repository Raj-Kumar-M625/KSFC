using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using CRMUtilities;
using System.Web.Http.ModelBinding;
//using System.Web.Mvc;

namespace EpicCrmWebApi
{
    public class EntitySurveyModel
    {
        public bool isValidDate = false;

        public long Id { get; set; }
        public long EntityId { get; set; }

        //[RegularExpression(@"^[1-9]$", ErrorMessage = "Select Season")]
        public long WorkflowSeasonId { get; set; }
        
        [Display(Name ="Season Name")]
        public string WorkflowSeasonName { get; set; }

        [Display(Name = "Crop Name")]
        public string TypeName { get; set; }

        [Display(Name = "Land Size (Acres)")]
        public decimal LandSizeInAcres { get; set; }

        [Display(Name = "Survey Number")]
        //[RegularExpression(@"^([a-zA-Z0-9/]{2,50})$", ErrorMessage = "Invalid Agreement Number")]
        public string SurveyNumber { get; set; }

        [Display(Name = "Status")]
        [Required(ErrorMessage = "Select Survey Status")]
        public string Status { get; set; }

        [Display(Name = "Major Crop")]
        public string MajorCrop { get; set; }

        [Display(Name = "Last Crop")]
        public string LastCrop { get; set; }

        [Display(Name = "Water Source")]
        public string WaterSource { get; set; }

        [Display(Name = "Soil Type")]
        public string SoilType { get; set; }

        [Display(Name = "Sowing Date")]
        public DateTime? SowingDate { get; set; }

        public long ActivityId { get; set; }

        [Display(Name = "Client Type")]
        public string EntityType { get; set; }

        [Display(Name = "Client Name")]
        public string EntityName { get; set; }
    }
}