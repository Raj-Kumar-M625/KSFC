using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.AdminModule
{
    public class SubAttributeMasterDto
    {
        [DisplayName("Id")]
        [Required(ErrorMessage = "The Id is required")]
        public int Id { get; set; }

        [DisplayName("Description")]
        [Required(ErrorMessage = "The Description is required")]
        public string Description { get; set; }


        [DisplayName("Attribute Id")]
        [Required(ErrorMessage = "The AttributeId is required")]
        public int AttributeId { get; set; }

        [DisplayName("Attribute UnitId")]
        [Required(ErrorMessage = "The Attribute UnitId is required")]
        public int AttributeUnitId { get; set; }

        [DisplayName("AttributeUnit OperatorId")]
        [Required(ErrorMessage = "The AttributeUnit OperatorId is required")]
        public int AttributeUnitOperatorId { get; set; }
    }
}
