using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblIdmBenfDet
    {
        public long BenfId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? BenfNumber { get; set; }
        public DateTime? BenfDate { get; set; }
        public int? BenfDept { get; set; }
        public string BenfType { get; set; }
        public string BenfName { get; set; }
        public int? BenfCode { get; set; }
        public decimal? BenfAmt { get; set; }
        public int? BenfInstType { get; set; }
        public bool? BenfInstFlag { get; set; }
        public int? BenfRecSeq { get; set; }
        public string DDAtparLoc { get; set; }
        public decimal? BenfRelAdjAmt { get; set; }
        public string BenfRtgsAcNo { get; set; }
        public string BenfRtgsIfsc { get; set; }
        public string BenfRtgsBank { get; set; }
        public string BenfRtgsBnkBranch { get; set; }
        public string BenfRtgsBnkCity { get; set; }
        public int? BenfRtgsChqNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? BenfRtgsChqDt { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
    }
}
