using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class FinancialComponentMasterDTO
    {

        [DisplayName("Financial Component")]
        public int FincompCd { get; set; }


        [DisplayName("Financial Component Details")]
        public string FincompDets { get; set; }

        [DisplayName("Sequence Number")]
        public decimal? SeqNo { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
