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
    
    public partial class StockInputTag
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public StockInputTag()
        {
            this.StockInputs = new HashSet<StockInput>();
        }
    
        public long Id { get; set; }
        public string GRNNumber { get; set; }
        public System.DateTime ReceiveDate { get; set; }
        public string VendorName { get; set; }
        public string VendorBillNo { get; set; }
        public System.DateTime VendorBillDate { get; set; }
        public int TotalItemCount { get; set; }
        public decimal TotalAmount { get; set; }
        public string Notes { get; set; }
        public string Status { get; set; }
        public string ZoneCode { get; set; }
        public string AreaCode { get; set; }
        public string TerritoryCode { get; set; }
        public string HQCode { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
        public bool IsEditAllowed { get; set; }
        public long CyclicCount { get; set; }
        public string ReviewNotes { get; set; }
        public string ReviewedBy { get; set; }
        public System.DateTime ReviewDate { get; set; }
        public string StaffCode { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StockInput> StockInputs { get; set; }
    }
}