using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.AdminModule
{
    public class OfficeMasterDto
    {
        [DisplayName("OffcCd")]
        [Required(ErrorMessage = "The Office code is required")]
        public byte OffcCd { get; set; }

        [DisplayName("OffcNam")]
        [Required(ErrorMessage = "The Office Name is required")]
        public string OffcNam { get; set; }

    }
}
