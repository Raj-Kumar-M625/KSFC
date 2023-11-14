using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Enquiry
{
    public class RegistrationDetailsDTO
    {
        [Display(Name = "S. No.")]
        public int? Id { get; set; }
        [Display(Name = "Registration Type")]
        [Required(ErrorMessage = "Registration Type is rquired.")]
        public string RegType { get; set; }
        [Display(Name = "Registration Number")]
        [Required(ErrorMessage = "Registration number is rquired.")]
        [MaxLength(20)]
        public string RegNumber { get; set; }
    }
}
