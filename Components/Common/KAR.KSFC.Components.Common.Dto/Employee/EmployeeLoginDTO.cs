using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.Employee
{
    public class EmployeeLoginDTO
    {
        [DisplayName("Employee userId")]
        [Required(ErrorMessage = "The Employee userId is required")]
        public int EmpuserId { get; set; }

        [DisplayName("Employee Id")]
        [Required(ErrorMessage = "The Employee Id is required")]
        public string EmpId { get; set; }

        [DisplayName("Employee Password")]
        [Required(ErrorMessage = "The Employee Password is required")]
        public string EmpPswd { get; set; }

        [DisplayName("DSC SlNo")]
        [Required(ErrorMessage = "The Designation SlNo is required")]
        public int? DscSlno { get; set; }

        [DisplayName("DSC Public Key")]
        [Required(ErrorMessage = "The DSC Public Key is required")]
        public string DscPubkey { get; set; }


        [DisplayName("DSC ExpairyDate")]
        [Required(ErrorMessage = "The DSC ExpairyDate is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DscExpdate { get; set; }

        [DisplayName("DSC Name")]
        [Required(ErrorMessage = "The DSC Name is required")]
        public string DscName { get; set; }

    }
}
