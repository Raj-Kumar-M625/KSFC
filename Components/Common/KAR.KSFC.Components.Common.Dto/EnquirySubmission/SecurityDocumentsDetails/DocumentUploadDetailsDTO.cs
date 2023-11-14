using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.SecurityDocumentsDetails
{
    public class DocumentUploadDetailsDTO
    {


        [DisplayName("Document Details Code")]
        [Required(ErrorMessage = "The Document Details Code is required")]
        public int DocdetCd { get; set; }


        [DisplayName("Document Details")]
        [Required(ErrorMessage = "The Document Details are required")]
        public string DocdetDets { get; set; }

        [DisplayName("Document Category Code")]
        [Required(ErrorMessage = "The Document Category Code is required")]
        public int DoccatCd { get; set; }

        [DisplayName("Sequence Number")]
        public decimal? SeqNo { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
