using Presentation.Models.Vendor;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.Master
{
    public class BankMasterModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Branch")]
        public string BranchName { get; set; }

        [Display(Name = "IFSC Code")]
        public string IfscCode { get; set; }
        public string AccountNumber { get;set; }
        public bool IsBulkTDS { get; set; }
        public bool IsBulkGSTTDS { get; set; }
        public bool IsBulkPayment { get; set; }

    }
}
