using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.Employee
{
    public class EmployeeDesignationDTO
    {

        public int EmpdesigId { get; set; }

        //[DisplayName("Employee Id")]
        //[Required(ErrorMessage = "The Employee Id is required")]
        public string EmpId { get; set; }

        [DisplayName("Substantive Designation Code")]
        [Required(ErrorMessage = "The Substantive Designation Code is required")]
        public string SubstDesigCode { get; set; }

        [DisplayName("Substantive start Date")]
        [Required(ErrorMessage = "Substantive designation start date is required")]
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime? SubstDate { get; set; }

        [DisplayName("PersonalPromotion Designation Code")]
        [Required(ErrorMessage = "The Personal Promotion Designation Code is required")]
        public string PpDesignCode { get; set; }

        [DisplayName("PP Date")]
       // [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "The PP Date is required")]
        public DateTime? PpDate { get; set; }

        [DisplayName("InCharge Designation Code")]
        [Required(ErrorMessage = "The InCharge Designation Code is required")]
        public string IcDesigCode { get; set; }

        [DisplayName("InCharge Date")]
      //  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "The InCharge Date is required")]
        public DateTime? IcDate { get; set; }

        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }






    }
}
