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
    public class LDHypothecationDetailsDTO
    {
        public int RowID { get; set; }

        [DisplayName("Type Of Asset")]
        [Required(ErrorMessage = "The Type Of Asset is required")]
        public string TypeOfAsset { get; set; }

        [DisplayName("Type Of Asset")]
        [Required(ErrorMessage = "The Type Of Asset is required")]
        public string NameOfAsset { get; set; }

        [DisplayName("Asset Description")]
        [Required(ErrorMessage = "The Asset Description is required")]
        public string AssetDescription { get; set; }

        [DisplayName("Hypothication Description")]
        public string HypothicationDescription { get; set; }

        [DisplayName("Hypothication Deed No.")]
        [Required(ErrorMessage = "The Hypothication Deed Number is required")]
        public string SecurityDeedNo { get; set; }

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
