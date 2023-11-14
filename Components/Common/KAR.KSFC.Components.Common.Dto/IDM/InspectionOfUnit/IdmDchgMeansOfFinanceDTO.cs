using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit
{
    public class IdmDchgMeansOfFinanceDTO
    {
        public long DcmfRowId { get; set; }
        public long LoanAcc { get; set; }
        public int? Action { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public DateTime? DcmfRqdt { get; set; }
        public DateTime? DcmfApdt { get; set; }
        public int? DcmfApau { get; set; }
        public int? DcmfCd { get; set; }

        public string Category { get; set; }

        public string FinanceType { get; set; }

      //  [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? DcmfAmt { get; set; }
        public int? DcmfMfType { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
