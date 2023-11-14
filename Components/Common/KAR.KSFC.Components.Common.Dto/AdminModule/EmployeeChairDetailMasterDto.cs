using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.AdminModule
{
    /// <summary>
    /// Employee Chair Details Master 
    /// </summary>
    public class EmployeeChairDetailMasterDto
    {
        /// <summary>
        /// EmpChairId 
        /// </summary>
        [DisplayName("Id")]
        [Required(ErrorMessage = "The Id is required")]
        public int Id { get; set; }

        [DisplayName("EmpId")]
        [Required(ErrorMessage = "The EmpId is required")]
        public string EmpId { get; set; }

        [DisplayName("OfficeCode")]
        [Required(ErrorMessage = "The OfficeCode is required")]
        public int OfficeCode { get; set; }

        [DisplayName("DesignationCode")]
        [Required(ErrorMessage = "The DesignationCode is required")]
        public string DesignationCode { get; set; }

        [DisplayName("ChairCode")]
        [Required(ErrorMessage = "The ChairCode is required")]
        public int ChairCode { get; set; }
    }
}
