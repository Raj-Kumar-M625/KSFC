using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class DocumentDetailMasterDTO
    {
        [DisplayName("Document Detail Code")]
        [Required(ErrorMessage = "The Document Detail Code is required")]
        public int DocdetCd { get; set; }

        [DisplayName("Document Details")]
        [Required(ErrorMessage = "The Document Detail is required")]
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
