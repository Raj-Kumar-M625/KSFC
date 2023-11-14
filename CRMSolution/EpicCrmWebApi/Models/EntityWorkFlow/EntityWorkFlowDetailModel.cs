using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class EntityWorkFlowDetailModel
    {
        public long Id { get; set; }

        //[Display(Name = "Profile Id")]
        public long EntityId { get; set; }

        [Display(Name = "Profile Name")]
        public string EntityName { get; set; }

        [Display(Name = "Profile Id")]
        public string EntityNumber { get; set; }
        
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; }

        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        [Display(Name = "Crop")]
        public string TypeName { get; set; }

        [Display(Name = "Season Name")]
        public string SeasonName { get; set; }

        [Display(Name = "Phase")]
        public string Phase { get; set; }

        [Display(Name = "Planned Start Date")]
        public DateTime PlannedFromDate { get; set; }

        [Display(Name = "Planned End Date")]
        public DateTime PlannedEndDate { get; set; }


        [Display(Name = "Planned Start Date")]
        public string PlannedFromDateAsText { get; set; }

        [Display(Name = "Planned End Date")]
        public string PlannedEndDateAsText { get; set; }


        [Display(Name = "Completed On")]
        public DateTime? CompletedOn { get; set; }

        public string Status { get; set; }

        public string HQCode { get; set; }

        [Display(Name = "Started")]
        public bool IsStarted { get; set; }

        [Display(Name = "Start Date")]
        public Nullable<System.DateTime> WorkFlowDate { get; set; }

        [Display(Name = "Seed Type")]
        public string MaterialType { get; set; }

        [Display(Name = "Seed Qty.")]
        public int MaterialQuantity { get; set; }

        [Display(Name = "Gap Filling")]
        public bool GapFillingRequired { get; set; }

        [Display(Name = "Gap Fill Qty.")]
        public int GapFillingSeedQuantity { get; set; }

        [Display(Name = "# Labor")]
        public int LaborCount { get; set; }

        [Display(Name = "% complete")]
        public int PercentCompleted { get; set; }

        [Display(Name = "Agreement Number")]
        public string Agreement { get; set; }

        [Display(Name = "Agreement Status")]
        public string AgreementStatus { get; set; }

        [Display(Name = "Aadhar")]
        public string UniqueId { get; set; }

        public long ActivityId { get; set; }

        // April 11 2020 - PJM
        [Display(Name = "Batch Number")]
        public string BatchNumber { get; set; }

        [Display(Name = "Acres")]
        public string LandSize { get; set; }

        [Display(Name = "Plant/Nipping Count")]
        public long ItemCount { get; set; }

        [Display(Name = "DWS Entry")]
        public string DWSEntry { get; set; }

        [Display(Name = "Spary/Fertilizer Used")]
        public long ItemsUsedCount { get; set; }

        [Display(Name = "Expected Yield Qty (Kgs)")]
        public long YieldExpected { get; set; }

        [Display(Name = "# Bags Issued")]
        public long BagsIssued { get; set; }

        [Display(Name = "Harvest Date")]
        public DateTime HarvestDate { get; set; }

        public int Sequence { get; set; }

        public bool IsFollowUpRow { get; set; }

        [Display(Name = "Operation Status")]
        public int IsActiveAsNumber { get; set; }

        [MaxLength(100, ErrorMessage ="Notes can be maximum 100 characters.")]
        public string Notes { get; set; }
    }
}