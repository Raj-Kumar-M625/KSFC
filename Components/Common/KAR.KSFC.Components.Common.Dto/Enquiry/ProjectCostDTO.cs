using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class ProjectCostDTO
    {
        [Display(Name = "S. No.")]
        public int? Id { get; set; }
        [MaxLength(50)]
        [Display(Name = "Cost Component")]
        [Required(ErrorMessage = "Project Cost Component is required.")]
        public string ProjectCostComponent { get; set; }
        [Display(Name = "Cost(in Lakhs)")]
        [MaxLength(50)]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Invalid Cost.")]
        [Required(ErrorMessage = "Cost is required.")]
        public string Cost { get; set; }
    }
}
