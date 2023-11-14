using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.Vendor
{
    public class VendorDefaultsModel
    {
        public VendorDefaultsModel()
        {

        }
        public int? Id { get; set; }

        [ForeignKey("Vendor")]
        public int? VendorId { get; set; }

        [Required]
        [Display(Name = "Category")]
        public string Category { get; set; }

        //[Required]
        //[Range(0,28 , ErrorMessage = "GST Percentage Should vary  between 0 to 28 percentage.")]
        //[Display(Name = "GST (0-28%)")]
        //public decimal GSTPercentage { get; set; }

        [Required]
        [Display(Name = "Payment Terms")]
        public string PaymentTerms { get; set; }

        [Required]
        [Display(Name = "TDS Section")]
        public string TDSSection { get; set; }

        [Required]
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; }

        [Required]
        [Range(0,30,ErrorMessage = "TDS Percentage Should vary  between 0 to 30 percentage.")]
        [Display(Name = "TDS (0-10%)")]
        public decimal TDSPercentage { get; set; }

        [Required]
        [Range(0,2,ErrorMessage = "GST-TDS Percentage Should vary  between 0 to 2 percentage.")]
        [Display(Name = "GST TDS ")]
        public decimal GST_TDSPercentage { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime ModifiedOn { get; set; }
        public virtual VendorViewModel VendorModel { get; set; }
    }
}
