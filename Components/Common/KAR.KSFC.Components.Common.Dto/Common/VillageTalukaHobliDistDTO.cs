using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.Common
{
    public class VillageTalukaHobliDistDTO
    {

        [DisplayName("VillageCode")]
        [Required(ErrorMessage = "The VillageCode is required")]
        public int VillageCode { get; set; }

        [DisplayName("VillageName")]
        [Required(ErrorMessage = "The VillageName is required")]
        public string VillageName { get; set; }

        [DisplayName("HobliCode")]
        [Required(ErrorMessage = "The HobliCode is required")]
        public int HobliCode  { get; set; }

        [DisplayName("HobliName")]
        [Required(ErrorMessage = "The HobliName is required")]
        public string HobliName { get; set; }

        [DisplayName("TalukaCode")]
        [Required(ErrorMessage = "The TalukaCode is required")]
        public int TalukaCode { get; set; }

        [DisplayName("TalukaName")]
        [Required(ErrorMessage = "The Taluka Name is required")]
        public string TalukaName { get; set; }

        [DisplayName("DistrictCode")]
        [Required(ErrorMessage = "The District Code is required")]
        public int DistrictCode { get; set; }

        [DisplayName("DistrictName")]
        [Required(ErrorMessage = "The District Name is required")]
        public string DistrictName { get; set; }
    }
}
