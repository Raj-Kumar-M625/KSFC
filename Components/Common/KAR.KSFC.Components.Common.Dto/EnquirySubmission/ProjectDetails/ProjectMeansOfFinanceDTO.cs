using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.EnquirySubmission.ProjectDetails
{
    public class ProjectMeansOfFinanceDTO
    {
        public int EnqPjmfId { get; set; }

        [DisplayName("Enquiry Id")]
        public int? EnqtempId { get; set; }

        [DisplayName("Means Of Finance Type")]
        [Required(ErrorMessage = "Finance type is required")]
        public int PjmfCd { get; set; }

        [DisplayName("Means Of Finance Value")]
        [Required(ErrorMessage = "Finance value is required")]
        public decimal? EnqPjmfValue { get; set; }

        [DisplayName("Means Of Finance CatCode")]
        [Required(ErrorMessage ="Finance Category is required")]
        public int? MfcatCd { get; set; }

        [DisplayName("Unique Id")]
        public string UniqueId { get; set; }

        public virtual PjmfcatCdtabMasterDTO MfcatCdNavigation { get; set; }
        public virtual PjmfCdtabMasterDTO PjmfCdNavigation { get; set; }


    }
}
