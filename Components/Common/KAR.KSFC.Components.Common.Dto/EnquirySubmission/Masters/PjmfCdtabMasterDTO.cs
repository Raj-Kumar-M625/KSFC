using System;

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class PjmfCdtabMasterDTO
    {
        [DisplayName("Means Of Finance Type")]
        public int PjmfCd { get; set; }

        [DisplayName("Means Of Finance Details")]
        public string PjmfDets { get; set; }

        [DisplayName("Means Of Finance Flag")]
        public byte? PjmfFlg { get; set; }

        [DisplayName("MeansofFinance Category Code")]
        public int? MfcatCd { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

    }
}
