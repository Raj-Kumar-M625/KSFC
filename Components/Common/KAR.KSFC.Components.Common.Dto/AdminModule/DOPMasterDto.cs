using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.AdminModule
{
    /// <summary>
    /// Delegation Of Power Matsers
    /// </summary>
    public class DOPMasterDto
    {
        /// <summary>
        /// DelegationId 
        /// </summary>
        [DisplayName("Id")]
        [Required(ErrorMessage = "The Id is required")]
        public int Id { get; set; }

        [DisplayName("Code")]
        [Required(ErrorMessage = "The Code is required")]
        public string Code { get; set; }

        [DisplayName("RoleId")]
        [Required(ErrorMessage = "The RoleId is required")]
        public int RoleId { get; set; }

        [DisplayName("AttributeId")]
        [Required(ErrorMessage = "The AttributeId is required")]
        public int AttributeId { get; set; }

        [DisplayName("SubAttributeId")]
        [Required(ErrorMessage = "The SubAttributeId is required")]
        public int SubAttributeId { get; set; }

        [DisplayName("Value")]
        [Required(ErrorMessage = "The Value is required")]
        public int Value { get; set; }

    }
}
