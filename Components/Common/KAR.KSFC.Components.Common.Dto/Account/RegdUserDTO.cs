using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto
{
    public class RegdUserDTO
    {


        [DisplayName("mobile")]
        [Required(ErrorMessage = "The Mobile Number is required")]
        public string mobile { get; set; }

        [DisplayName("Pan")]
        [Required(ErrorMessage = "The Pan is required")]
        [RegularExpression(@"^([A-Z]{5}\d{4}[A-Z]{ 1})$")]
        public string Pan { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "The Name is required")]
        public string Name { get; set; }

        [DisplayName("Branch")]
        [Required(ErrorMessage = "The Branch is required")]
        public string Branch { get; set; }
    }
}
