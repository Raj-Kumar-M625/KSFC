using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.Employee
{
    public class EmployeeDesignationHistoryDTO
    {
        
        [DisplayName("EmployeeDesignationHistory Id")]
        [Required(ErrorMessage = "The EmployeeDesignationHistory Id is required")]
        public int EmpdesighistId { get; set; }

        [DisplayName("Employee Id")]
        [Required(ErrorMessage = "The Employee Id is required")]
        public string EmpId { get; set; }

        [DisplayName("Designation Code")]
        [Required(ErrorMessage = "The Designation Code is required")]
        public string DesigCode { get; set; }

        [DisplayName("DesignationType Code")]
        [Required(ErrorMessage = "The DesignationType Code is required")]
        public string DesigTypeCode { get; set; }

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
