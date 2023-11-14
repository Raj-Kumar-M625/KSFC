using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.CreationOfDisbursmentProposal
{
    public class TblIdmReleDetlsDTO
    {
        public long ReleId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public long? PropNumber { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? ReleDueAmount { get; set; }
        public int? ReleAtParAmount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? ReleAtParCharges { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? ReleBnkChg { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? ReleUpFrtAmount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? ReleDocChg { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? ReleComChg { get; set; }
        public int? ReleFdAmount { get; set; }
        public int? ReleFdGlcd { get; set; }
        public int? ReleOthAmount { get; set; }
        public int? ReleOthGlcd { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? ReleAdjAmount { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? ReleAdjRecSeq { get; set; }
        public int? ReleAddUpFrtAmount { get; set; }
        public int? ReleAddlafdAmount { get; set; }
        public int? ReleSertaxAmount { get; set; }
        public int? ReleCersai { get; set; }
        public int? ReleSwachcess { get; set; }
        public int? Relekrishikalyancess { get; set; }
        
        public int? ReleCollGuaranteeFee { get; set; }
        public int? ReleNumber { get; set; }
        public int? AddlAmt1 { get; set; }
        public int? AddlAmt2 { get; set; }
        public int? AddlAmt3 { get; set; }
        public int? AddlAmt4 { get; set; }
        public int? AddlAmt5 { get; set; }
        public DateTime? ReleDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? ReleAmount { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public int? Action { get; set; }
        public decimal? ReleChargeAmount { get; set; }

        public int DeptCode { get; set; }
        public string DeptName { get; set; }
        public int DsbOthdebitId { get; set; }
        public string DsbOthdebitDesc { get; set; }
        [DisplayName("Bank Charge")]
        public string BnkChrg { get; set; }

        public string DocCharge { get; set; }
        public string Insurencecharge { get; set; }

        public string Penalty { get; set; }
        public string UpCharge { get; set; }
        public string LegalCharge { get; set; }

        public string CommCharge { get; set; }
        public string FDAmt { get; set; }
        public string OthAmt { get; set; }
        public string AdjustAmt { get; set; }
        public string AddUpAmt { get; set; }
        public string AddFDAmt { get; set; }
        public string SerTaxAmt { get; set; }
        public string CersaiAmt { get; set; }
        public string SwachCess { get; set; }
        public string KrishiCess { get; set; }
        public string CollGuarFee { get; set; }
        public string add_amt1 { get; set; }
        public string add_amt2 { get; set; }
        public string AtParAmt { get; set; }
       
        public string Newloan { get; set; }
        public string NewDsbOthdebitId { get; set; }
        public string NewLoanSub { get; set; }
        public string NewOffcCd { get; set; }
        public string NewChargeAmount { get; set; }
        public decimal? BenfAmt { get; set; }

        public virtual TblIdmDisbPropDTO TblIdmDisbProp { get; set; }
    }
}
