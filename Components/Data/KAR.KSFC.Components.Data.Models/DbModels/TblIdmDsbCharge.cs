using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblIdmDsbCharge
    {
  
        public int IdmDsbChargeId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? SecurityCd { get; set; }
        public int? ChargeTypeCd { get; set; }
        public decimal? SecurityValue { get; set; }
        public string NocIssueBy { get; set; }
        public string NocIssueTo { get; set; }
        public DateTime? NocDate { get; set; }
        public string AuthLetterBy { get; set; }
        public DateTime? AuthLetterDate { get; set; }
        public DateTime? BoardResolutionDate { get; set; }
        public DateTime? MoeInsuredDate { get; set; }
        public string RequestLtrNo { get; set; }
        public DateTime? RequestLtrDate { get; set; }
        public string? BankIfscCd { get; set; }
        public string? BankRequestLtrNo { get; set; }
        public DateTime? BankRequestLtrDate { get; set; }
        public string ChargePurpose { get; set; }
        public string ChargeDetails { get; set; }
        public string ChargeConditions { get; set; }
        public string UploadDocument { get; set; }
        public string ApprovedEmpId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string SecurityDets { get; set; }    
        public int? BankIfscId { get; set; }
        public virtual TblSecurityRefnoMast TblSecurityRefnoMast { get; set; }
        public virtual TblChargeType TblChargeType { get; set; }
        public virtual TbIIfscMaster TbIIfscMaster { get; set; }
    }
}
