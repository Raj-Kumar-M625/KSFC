using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class ProjectCostMasterDTO
    {

        [DisplayName("Project Cost Code")]
        public int PjcostCd { get; set; }

        [DisplayName("Project Cost Details")]
        public string PjcostDets { get; set; }

        [DisplayName("Project Cost Flag")]
        public int? PjcostFlg { get; set; }

        [DisplayName("Sequence Code")]
        public decimal? SeqNo { get; set; }

        [DisplayName("Project Cost Group Code")]
        public int? PjcostgroupCd { get; set; }

        [DisplayName("Project Cost SubGroup Code")]
        public int? PjcsgroupCd { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

    }
}
