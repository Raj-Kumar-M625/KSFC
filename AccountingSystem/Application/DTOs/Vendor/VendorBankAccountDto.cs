using Application.DTOs.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace Application.DTOs.Vendor
{
    public class VendorBankAccountDto
    {
        [Key]
        public int Id { get; set; }

        //[ForeignKey("BranchMaster")]
        [ForeignKey(nameof(BranchMaster))]
        public int? BankMasterId { get; set; }

        [ForeignKey("Vendor")]
        public int? VendorId { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? AccountNumber { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
       
        public virtual BranchMasterDto BranchMaster { get; set; }
        public virtual VendorDetailsDto Vendor { get; set; }
    }
}
#nullable disable