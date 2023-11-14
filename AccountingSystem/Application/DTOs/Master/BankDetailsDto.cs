using Application.DTOs.Vendor;
using Domain.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#nullable enable
namespace Application.DTOs.Master
{
    public class BankDetailsDto
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
#nullable disable