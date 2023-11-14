
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.AdminModule
{
    public class AttributeMasterDto
    {

        [DisplayName("Id")]
        [Required(ErrorMessage = "The Id is required")]
        public int Id { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "The Description is required")]
        public string Description { get; set; }

        [DisplayName("RoleId")]
        [Required(ErrorMessage = "The Role Id is required")]
        public int RoleId { get; set; }

    }
}

