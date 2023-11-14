using Application.DTOs.Master;
using Application.DTOs.Vendor;
using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.Master
{
    public class BankDetailsModel
    {
        [Key]
        public int Id { get; set; }

        
        public string? BankName { get; set; }
        public bool? bank_is_active { get; set; }
        public int? bank_created_by { get; set; }
        public DateTime? bank_created_on { get; set; }
        public int? bank_updated_by { get; set; }
        public DateTime? bank_updated_on { get; set; }
        public virtual BranchMasterDto BranchMaster { get; set; }
        public virtual VendorBankAccountDto VendorBankAccounts { get; set; }
    }
}
