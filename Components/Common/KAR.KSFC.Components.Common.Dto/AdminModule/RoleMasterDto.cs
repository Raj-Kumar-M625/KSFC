using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.AdminModule
{
    public class RoleMasterDto
    {
        [DisplayName("Id")]
        [Required(ErrorMessage = "The Id is required")]
        public int Id { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "The Description is required")]

        public string Description { get; set; }

        [DisplayName("ModuleId")]
        [Required(ErrorMessage = "The ModuleId is required")]
        public int ModuleId { get; set; }

    }
}
