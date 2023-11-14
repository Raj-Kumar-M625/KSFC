using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class ProjectPrevFYDetailsDTO
    {
        [Display(Name = "S. No.")]
        public int? Id { get; set; }
        [Display(Name = "Financial Year")]
        [Required(ErrorMessage = "Financial Year is required.")]
        public string FinancialYear { get; set; }
        [Display(Name = "Financial Component")]
        [Required(ErrorMessage = "Financial Component is required.")]
        public string FinancialComponent { get; set; }
        [Display(Name = "Amount (Lakhs)")]
        [Required(ErrorMessage = "Amount is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid Amount.")]
        public string Amount { get; set; }

        public bool ProvisionalBalanceSheetDetails { get; set; }
    }
}
