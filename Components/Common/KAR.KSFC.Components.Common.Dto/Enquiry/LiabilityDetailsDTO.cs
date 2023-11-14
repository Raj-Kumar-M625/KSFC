using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class LiabilityDetailsDTO
    {
        [Display(Name = "S. No.")]
        public int? Id { get; set; }
        [Display(Name = "Name")]
        [MaxLength(30)]

        public string Name { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        public int? NameId { get; set; }
        [Display(Name = "Liability Description")]
        [MaxLength(40)]
        public string Description { get; set; }
        [Display(Name = "Value (Lakhs)")]
        [MaxLength(20)]
        [Required(ErrorMessage = "Value is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid Value.")]
        public string Value { get; set; }
    }
}
