//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DBModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class EntityWorkFlowDetail
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public EntityWorkFlowDetail()
        {
            this.EntityWorkFlowDetailItemUseds = new HashSet<EntityWorkFlowDetailItemUsed>();
        }
    
        public long Id { get; set; }
        public long EntityWorkFlowId { get; set; }
        public int Sequence { get; set; }
        public string Phase { get; set; }
        public System.DateTime PlannedStartDate { get; set; }
        public System.DateTime PlannedEndDate { get; set; }
        public string PrevPhase { get; set; }
        public long ActivityId { get; set; }
        public bool IsComplete { get; set; }
        public Nullable<System.DateTime> ActualDate { get; set; }
        public Nullable<System.DateTime> PrevPhaseActualDate { get; set; }
        public string PhaseCompleteStatus { get; set; }
        public Nullable<long> EmployeeId { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime Timestamp { get; set; }
        public bool IsStarted { get; set; }
        public string MaterialType { get; set; }
        public int MaterialQuantity { get; set; }
        public bool GapFillingRequired { get; set; }
        public int GapFillingSeedQuantity { get; set; }
        public int LaborCount { get; set; }
        public int PercentCompleted { get; set; }
        public long BatchId { get; set; }
        public Nullable<System.DateTime> WorkFlowDate { get; set; }
        public string TagName { get; set; }
        public bool IsFollowUpRow { get; set; }
        public string Notes { get; set; }
        public long ParentRowId { get; set; }
        public string BatchNumber { get; set; }
        public string LandSize { get; set; }
        public string DWSEntry { get; set; }
        public long ItemCount { get; set; }
        public long ItemsUsedCount { get; set; }
        public long YieldExpected { get; set; }
        public long BagsIssued { get; set; }
        public System.DateTime HarvestDate { get; set; }
        public bool IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<EntityWorkFlowDetailItemUsed> EntityWorkFlowDetailItemUseds { get; set; }
        public virtual EntityWorkFlow EntityWorkFlow { get; set; }
    }
}