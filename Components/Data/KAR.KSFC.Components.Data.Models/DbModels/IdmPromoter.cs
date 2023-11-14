using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class IdmPromoter
    {
        public int IdmPromId { get; set; }       
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? UtCd { get; set; }
        public long PromoterCode { get; set; }
        public string PromName { get; set; }
        public int PdesigCd { get; set; }
        public string PromGender { get; set; }
        public DateTime? PromDob { get; set; }
        public int? PromAge { get; set; }
        public string NameFatherSpouse { get; set; }
        public int? PclasCd { get; set; }
        public int? PsubclasCd { get; set; }
        public int? PromExpYrs { get; set; }
        public string PromExpDet { get; set; }
        public int? PqualCd { get; set; }
        public string PromAddlQual { get; set; }
        public int? DomCd { get; set; }
        public string PromPassport { get; set; }
        public string PromPan { get; set; }
        public DateTime? PromJnDt { get; set; }
        public DateTime? PromExDt { get; set; }
        public long? PromMobileNo { get; set; }
        public string PromEmail { get; set; }
        public long? PromTelNo { get; set; }
        public bool? PromChief { get; set; }
        public bool? PromMajor { get; set; }
        public bool? PromPhyHandicap { get; set; }
        public string PromPhoto { get; set; }       
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
      //  public virtual TblPromCdtab TblPromCdtab { get; set; }
        public virtual TblPdesigCdtab TblPdesigCdtab { get; set; }             
    }
}
