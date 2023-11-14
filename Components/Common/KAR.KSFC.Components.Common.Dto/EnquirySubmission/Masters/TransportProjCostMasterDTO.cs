using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class TransportProjCostMasterDTO
    {

        [DisplayName("TransportProjectCost Code")]
        [Required(ErrorMessage = "The Promoter Code is required")]
        public int TrPjcostCd { get; set; }

        [DisplayName("TransportProjectCost Details")]
        [Required(ErrorMessage = "The Promoter Code is required")]
        public string TrPjcostDet { get; set; }

        [DisplayName("TransportProjectCost Flag")]
        [Required(ErrorMessage = "The Promoter Code is required")]
        public byte? TrPjcostFlg { get; set; }

        [DisplayName("Sequence Number")]
        public decimal? SeqNo { get; set; }

        

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
