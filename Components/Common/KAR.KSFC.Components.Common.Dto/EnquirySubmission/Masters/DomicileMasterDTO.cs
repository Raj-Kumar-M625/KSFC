using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class DomicileMasterDTO
    {
        [DisplayName("Domicile Code")]
        
        public int DomCd { get; set; }
        [DisplayName("Domicile Details")]
        public string DomDets { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
