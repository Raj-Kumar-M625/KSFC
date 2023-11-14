using System;
using System.ComponentModel;
 using System.ComponentModel.DataAnnotations;
namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class DocumentCategoryMasterDTO
    {
        [DisplayName("Document Category Code")]
         [Required(ErrorMessage = "Document Category Code is required")]
        public int DoccatCd { get; set; }
 
         [DisplayName("Document Category Details")]
         [Required(ErrorMessage = "Document Category Details is required")]
        public string DoccatDets { get; set; }
 
         [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
