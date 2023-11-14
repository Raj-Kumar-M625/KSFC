using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters
{
    public class ProjectCostGroupMasterDTO
    {

        [DisplayName("Project Cost Group Code")]
        public int PjcostgroupCd { get; set; }

        [DisplayName("Project Cost Group Details")]
        public string PjcostgroupDets { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }
    }
}
