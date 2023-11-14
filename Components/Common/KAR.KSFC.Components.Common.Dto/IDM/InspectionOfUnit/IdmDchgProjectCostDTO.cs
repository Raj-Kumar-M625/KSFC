using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit
{
    public class IdmDchgProjectCostDTO
    {
        public long Id { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public DateTime? DcpcRequestDate { get; set; }
        public DateTime? DcpcApprovedate { get; set; }
        public int? DcpcstApproveAU { get; set; }
        public string ProjectCost { get; set; }

        public int? DcpcstCode { get; set; }

        //[Required(ErrorMessage = "Project Cost Amount is required")]
      //  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? DcpcAmount { get; set; }
        public DateTime? DcpcCommunicationDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public int? Action { get; set; }

    }
}
