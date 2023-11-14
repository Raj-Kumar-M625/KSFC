using Domain.Vendor;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Master
{
    public class BranchMaster
    {
        [Key]
        public int branch_id { get; set; }

        [ForeignKey("BankDetails")]
        //[ForeignKey(nameof(BankDetails))]

        public int? branch_bank_id { get; set; }
        public string? branch_name { get; set; }
        public string? branch_ifsc { get; set; }
        public string? branch_micr_no { get; set; }
        public bool? branch_is_active { get; set; }
        public int? branch_created_by { get; set; }
        public DateTime? branch_created_on { get; set; }
        public int? branch_updated_by { get; set; }
        public DateTime? branch_updated_on { get; set; }

        public virtual BankDetails BankDetails { get; set; }
        //public virtual VendorBankAccount VendorBankAccount { get; set; }




    }
}
