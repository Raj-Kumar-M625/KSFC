using Application.DTOs.Master;
using Application.DTOs.Vendor;
using Domain.Master;
using System;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.Master
{
    public class BranchMasterModel
    {
        [Key]
        public int branch_id { get; set; }
        public int branch_bank_id { get; set; }
        public string branch_name { get; set; }
        public string branch_ifsc { get; set; }
        public string branch_micr_no { get; set; }
        public bool branch_is_active { get; set; }
        public int branch_created_by { get; set; }
        public DateTime branch_created_on { get; set; }
        public int branch_updated_by { get; set; }
        public DateTime branch_updated_on { get; set; }

        public virtual BankDetailsDto BankDetails { get; set; }
        public virtual VendorBankAccountDto VendorBankAccounts { get; set; }

    }
}
