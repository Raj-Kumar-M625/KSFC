using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class PromoterRelationMasterDTO
    {
        [DisplayName("PromoterRelation Code")]
        [Required(ErrorMessage = "The Promoter Relation Code is required")]
        public int PromrelCd { get; set; }

        [DisplayName("Promoter Relation Details")]
        [Required(ErrorMessage = "The Promoter Relation Details is required")]
        public string PromrelDets { get; set; }
 
        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

    }
}
