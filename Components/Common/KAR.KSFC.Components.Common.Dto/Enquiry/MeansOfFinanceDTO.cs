using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class MeansOfFinanceDTO
    {
        [Display(Name = "S. No.")]
        public int? Id { get; set; }
        [Display(Name = "Category of Finance")]
        [Required(ErrorMessage = "Category of finance is required.")]
        public string CatMeansOfFinance { get; set; }
        [Display(Name = "Finance Type")]
        public string FinanceType { get; set; }
        [Display(Name = "Value (in Lakhs)")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid Value.")]
        [Required(ErrorMessage = "Value is required.")]
        public string Value { get; set; }
    }
}
