using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class SecurityDetailsDTO
    {
        [Display(Name = "S. No.")]
        public int? Id { get; set; }
        [Display(Name = "Type of Security")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Security type is rquired.")]
        public string SecurityType { get; set; }
        [MaxLength(50)]
        public string SecurityDtl { get; set; }
        [MaxLength(100)]
        public string Description { get; set; }
        [Display(Name = "Name of Holder")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Holder name is rquired.")]
        public string HolderName { get; set; }
        [MaxLength(50)]
        public string Relation { get; set; }
        [MaxLength(20)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid value.")]
        [Display(Name = "Value")]
        [Required(ErrorMessage = "Value is rquired.")]
        public string Value { get; set; }
    }
}
