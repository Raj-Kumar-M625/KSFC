using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteWorkFlowDisplayData
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public long EntityId { get; set; }
        public string EntityType { get; set; }
        public string EntityName { get; set; }
        public long AgreementId { get; set; }
        public string Agreement { get; set; }
        public long EntityWorkFlowDetailId { get; set; }

        [Display(Name ="Crop Name")]
        public string TypeName { get; set; }
        public string Phase { get; set; }
        public DateTime FieldVisitDate { get; set; }
        public bool IsStarted { get; set; }
        public System.DateTime Date { get; set; }
        public string MaterialType { get; set; }
        public int MaterialQuantity { get; set; }
        public bool GapFillingRequired { get; set; }
        public int GapFillingSeedQuantity { get; set; }
        public int LaborCount { get; set; }
        public int PercentCompleted { get; set; }
        public bool IsProcessed { get; set; }
        public long EntityWorkFlowId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime Timestamp { get; set; }

        public long FollowUpCount { get; set; }

        public string BatchNumber { get; set; }
        public string LandSize { get; set; }
        public long ItemCount { get; set; }
        public string DWSEntry { get; set; }
        public long ItemsUsedCount { get; set; }
        public long YieldExpected { get; set; }
        public long BagsIssued { get; set; }
        public DateTime HarvestDate { get; set; }
    }
}