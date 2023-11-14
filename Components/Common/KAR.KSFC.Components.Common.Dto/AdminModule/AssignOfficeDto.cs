using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace KAR.KSFC.Components.Common.Dto.AdminModule
{
    public class AssignOfficeDto
    {
        [DisplayName("Employee Number")]
        [Required(ErrorMessage = "Employee Number is Required")]
        public string TeyTicketNum { get; set; }

        [DisplayName("Employee Name")]
        [Required(ErrorMessage = "Employee Name is Required")]
        public string EmpoyeeName { get; set; }


        public string MobileNumber { get; set; }

        public string Email { get; set; }

        [DisplayName("Office")]
        [Required(ErrorMessage = "Employee Office is Required")]
        public string OfficeId { get; set; }

        [DisplayName("Chair")]
        [Required(ErrorMessage = "Employee Chair is Required")]
        public string ChairId { get; set; }

        [DisplayName("Operating Designation")]
        [Required(ErrorMessage = "Employee operating designation is Required")]
        public string OpsDesignationId { get; set; }

        public bool IsCheckedIn { get; set; }

        [DisplayName("From Date")]
        [Required(ErrorMessage = "From date is required")]
        public string CommencementDate { get; set; }

        public List<AssignOfficeMasterHistoryDto> AssignOfficeDataDto { get; set; }
    }


    
}
