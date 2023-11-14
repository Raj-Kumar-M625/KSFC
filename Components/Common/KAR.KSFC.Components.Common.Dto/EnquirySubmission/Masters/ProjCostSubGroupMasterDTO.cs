using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class ProjCostSubGroupMasterDTO
    {
        [DisplayName("Project Cost SubGroupCode")]
        public int PjcsgroupCd { get; set; }

        [DisplayName("Project Cost Details")]
        public string PjcsgroupDets { get; set; }

        [DisplayName("Project Cost group Id")]
        public int PjcostgroupCd { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
