using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Application.DTOs.Vendor
{
    public class VendorDefaultsDto
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("VendorDetailsDto")]
        public int VendorId { get; set; }
        public string Category { get; set; }
      
        public string PaymentTerms { get; set; }
        public string TDSSection { get; set; }
        public string PaymentMethod { get; set; }
        public decimal TDSPercentage { get; set; }
        public decimal GST_TDSPercentage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }

        public DateTime ModifiedOn { get; set; }
        public virtual VendorDetailsDto VendorDetails { get; set; }
    }
}
