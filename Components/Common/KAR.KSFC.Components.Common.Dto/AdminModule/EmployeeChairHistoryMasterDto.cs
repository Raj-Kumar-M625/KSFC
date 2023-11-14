using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.AdminModule
{
    /// <summary>
    /// Employee Chair History Details Master 
    /// </summary>
    public class EmployeeChairHistoryMasterDto
    {
        /// <summary>
        /// EmpChairHistoryId 
        /// </summary>
        ///

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

        [DisplayName("From Date")]
        [Required(ErrorMessage = "The From Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? FromDate { get; set; }

        [DisplayName("To Date")]
        [Required(ErrorMessage = "The To Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MMM-yyyy}")]
        public DateTime? ToDate { get; set; }
    }
}
