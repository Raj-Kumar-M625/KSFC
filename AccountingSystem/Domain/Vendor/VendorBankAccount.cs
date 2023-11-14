using Domain.Master;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Vendor
{
    public class VendorBankAccount
    {
        [Key]
        public int Id { get; set; }
       
        [ForeignKey("BranchMaster")]
        public int? BankMasterId { get; set; }

        [ForeignKey("Vendor")]
        public int? VendorId { get; set; }
        public string? BeneficiaryName { get; set; }
        public string? AccountNumber { get; set; }
        public string? CreatedBy { get; set; }     
        public DateTime? ModifiedOn { get; set; }  
       public string? ModifedBy { get; set; }
   
        public virtual BranchMaster BranchMaster { get; set; }

        public virtual Vendors Vendor { get; set; }
    }
}
