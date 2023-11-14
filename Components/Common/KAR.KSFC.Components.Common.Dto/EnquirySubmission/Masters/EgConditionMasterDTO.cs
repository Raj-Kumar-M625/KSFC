using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class EgConditionMasterDTO
    {

        [DisplayName("Condition Code")]
        [Required(ErrorMessage = "The Condition Code is required")]
        public short CondCd { get; set; }

        [DisplayName("Condition Details")]
        [Required(ErrorMessage = "The Condition Details is required")]
        public string CondDets { get; set; }

        [DisplayName("Condition Stage")]
        [Required(ErrorMessage = "The Condition Stage is required")]
        public byte? CondStg { get; set; }

        [DisplayName("Condition SubCode")]
        [Required(ErrorMessage = "The Condition SubCode is required")]
        public short CondSub { get; set; }

        [DisplayName("Condition Flag")]
        public short? CondFlg { get; set; }

        [DisplayName("Condition Status Code Flag")]
        public short? CondStatusFlag { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
