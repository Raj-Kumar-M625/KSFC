using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.AdminModule
{
    public class AssignRoleDto
    {
        [Required(ErrorMessage = "Employee Number is required")]
        public string EmployeeNumber { get; set; }

        [Required(ErrorMessage ="Employee Name is required")]
        public string EmployeeName { get; set; }



        [Required(ErrorMessage = "Please select module")]
        public string ModuleId { get; set; }
        public string ModuleName { get; set; }


        public string Email { get; set; }
        public string MobileNumber { get; set; }



        [Required(ErrorMessage ="Please select Role")]
        public string RoleId { get; set; }
        public string RoleName { get; set; }

    }



}
