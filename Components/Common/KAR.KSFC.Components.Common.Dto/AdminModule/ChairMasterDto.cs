using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
 using System.ComponentModel.DataAnnotations;
 

namespace KAR.KSFC.Components.Common.Dto.AdminModule
{
    /// <summary>
    /// Chair Master for Office Work Flow
    /// </summary>
    public class ChairMasterDto
    {
      
         [DisplayName("Id")]
         [Required(ErrorMessage = "The Id is required")]
        public int Id { get; set; }
 
         [DisplayName("Code")]
         [Required(ErrorMessage = "The Code is required")]
        public int Code { get; set; }
 
         [DisplayName("Description")]
         [Required(ErrorMessage = "The Description is required")]
        public string Description { get; set; }
 
         [DisplayName("OfficeCode")]
         [Required(ErrorMessage = "The OfficeCode is required")]
        public int OfficeCode { get; set; }
    }

}
