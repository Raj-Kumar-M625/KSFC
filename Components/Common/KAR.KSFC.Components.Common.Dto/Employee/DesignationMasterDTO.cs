using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.Employee
{
    public class DesignationMasterDTO
    {

        [DisplayName("Designation Code")]
        [Required(ErrorMessage = "The Designation Code is required")]
        public string TgesCode { get; set; }

        [DisplayName("Designation Description")]
        [Required(ErrorMessage = "The Designation Description is required")]
        public string TgesDesc { get; set; }


        [DisplayName("Designation Hierarchy")]
        [Required(ErrorMessage = "The Designation Hierarchy is required")]
        public decimal TegsOrder { get; set; }

    }
}
