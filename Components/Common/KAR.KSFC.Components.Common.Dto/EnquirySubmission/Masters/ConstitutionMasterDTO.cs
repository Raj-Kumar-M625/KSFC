using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class ConstitutionMasterDTO
    {

        [DisplayName("Constitution Code")]
        public int CnstCd { get; set; }

        [DisplayName("Constitution Details")]
        public string CnstDets { get; set; }

        [DisplayName("Constitution Tax Rate")]
        public decimal CnstTaxr { get; set; }

        public DateTime? ModifiedDate { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

        [DisplayName("Constitution Pan character")]
        public string CnstPanchar { get; set; }
    }
}
