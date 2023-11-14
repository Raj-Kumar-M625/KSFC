using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class LDSecurityDetailsDTO
    {
        public int RowID { get; set; }

        [DisplayName("Name of Security Holder")]
        [Required(ErrorMessage = "The Security Holder name is required")]
        public string SecurityHolder { get; set; }

        [DisplayName("Category of Security")]
        [Required(ErrorMessage = "The Security Category is required")]
        public string SecurityCategory { get; set; }

        [DisplayName("Type of Security")]
        [Required(ErrorMessage = "The Security Type is required")]
        public string SecurityType { get; set; }

        [DisplayName("Description of Security")]
        public string SecurityDescription { get; set; }

        [DisplayName("Security Deed No.")]
        [Required(ErrorMessage = "The Security Deed Number name is required")]
        public string SecurityDeedNo { get; set; }

        [DisplayName("Deed Description")]
        public string DeedDescription { get; set; }

        [DisplayName("Sub Registrar Office")]
        [Required(ErrorMessage = "The Sub Registrar Office is required")]
        public string SubRegistrarOffice { get; set; }

        [DisplayName("Date of Execution")]
        [Required(ErrorMessage = "The Date of Execution is required")]
        public DateTime DateOfExecution { get; set; }

        [DisplayName("Value (in Lakhs)")]
        [Required(ErrorMessage = "The Value is required")]
        public int ValueInLakhs { get; set; }
        public string AccountNumber { get; set; }
        public bool IsActive { get; set; }
        public string Action { get; set; }
        public IFormFile file { get; set; }
    }
}
