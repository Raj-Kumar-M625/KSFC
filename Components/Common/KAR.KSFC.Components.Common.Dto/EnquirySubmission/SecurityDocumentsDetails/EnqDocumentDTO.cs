using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails
{
    public class EnqDocumentDTO
    {
        [DisplayName("Document Id")]
         [Required(ErrorMessage = "The Document Id is required")]
        public int Id { get; set; }
 
         [DisplayName("Enquiry Id")]
         [Required(ErrorMessage = "The Enquiry Id is required")]
        public int EnquiryId { get; set; }
 
         [DisplayName("Description")]
         [Required(ErrorMessage = "The Description is required")]
        public string Description { get; set; }
 
 
         [DisplayName("Process")]
         [Required(ErrorMessage = "The Process is required")]
        public string Process { get; set; }
 
         [DisplayName("Document Section")]
         [Required(ErrorMessage = "The Document Section is required")]
        public string DocSection { get; set; }
 
 
         [DisplayName("Document FileType")]
         [Required(ErrorMessage = "The Document Filetype is required")]
        public string FileType { get; set; }
 
         [DisplayName("Unique Id")]
         [Required(ErrorMessage = "The Unique Id is required")]
        public string UniqueId { get; set; }

    }
}
