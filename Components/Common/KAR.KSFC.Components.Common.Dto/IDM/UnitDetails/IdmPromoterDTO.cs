using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class IdmPromoterDTO
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

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? PromDob { get; set; }
        public int? PromAge { get; set; }
        public string NameFatherSpouse { get; set; }
        public int PclasCd { get; set; }
        public int? PsubclasCd { get; set; }
        public int? PromExpYrs { get; set; }
        public string PromExpDet { get; set; }
        public int? PqualCd { get; set; }
        public string PromAddlQual { get; set; }
        public int DomCd { get; set; }
        public string PromPassport { get; set; }
        public string PromPan { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? PromJnDt { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? PromExDt { get; set; }
        public long? PromMobileNo { get; set; }
        public string PromEmail { get; set; }
        public long? PromTelNo { get; set; }
        public bool? PromChief { get; set; }
        public bool? PromMajor { get; set; }
        public bool? PromPhyHandicap { get; set; }
        public string PromPhoto { get; set; }
        public string UniqueId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Action { get; set; }
        public string PdesigDet { get; set; }
        public string PclassDet { get; set; }
        public string PdomDet { get; set; }

        public virtual IdmPromAddressDTO TblIdmPromAddress { get; set; }
       public virtual TblPromcdtabDTO TblPromcdtab { get; set; }    
    }
}
