using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace KAR.KSFC.Components.Common.Dto.AdminModule
{
    public class AttributeUnitOperatorMasterDto
    {
        [DisplayName("Id")]
        [Required(ErrorMessage = "The Id is required")]
        public int Id { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "The Description is required")]
        public string Description { get; set; }
    }
}
