using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using System.ComponentModel;
 using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.AdminModule
{
    /// <summary>
    /// Delegation Of Power History Matsers
    /// </summary>
    public class DOPHistoryMasterDto
    {
        /// <summary>
        /// DelegationHistoryId 
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
 
         [DisplayName("From Date")]
         [Required(ErrorMessage = "The From Date is required")]
         [DataType(DataType.Date)]
         [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? FromDate { get; set; }
 
         [DisplayName("To Date")]
         [Required(ErrorMessage = "The To Date is required")]
         [DataType(DataType.Date)]
         [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ToDate { get; set; }
    }
}
