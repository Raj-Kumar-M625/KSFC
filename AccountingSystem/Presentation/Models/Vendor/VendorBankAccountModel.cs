using Domain.Master;
using Presentation.Models.Master;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.Vendor
{
    public class VendorBankAccountModel
    {
        [Key]
        public int? Id { get; set; }

        [Required]
        [ForeignKey("BranchMaster")]
        public int? BankMasterId { get; set; }

        [ForeignKey("Vendor")]
        public int? VendorId { get; set; }

        [Required]
        [MaxLength(100)]
        [Display(Name = "Beneficiary Name")]
        public string? BeneficiaryName { get; set; }

        [Required]
        [MaxLength(20)]
        [Display(Name = "Account Number")]
        public string? AccountNumber { get; set; }
        public string? CreatedBy { get; set; }
      
        public virtual VendorViewModel Vendor { get; set; }
        //public virtual BankMasterModel BankMaster { get; set; }
        public virtual BranchMasterModel BranchMaster { get; set; }
    }
}
