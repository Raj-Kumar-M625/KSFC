using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    /// <summary>
    ///  Author: Gagana K; Module: SecurityCharge; Date:21/07/2022
    /// </summary>
    public class IdmSecurityChargeDTO
    {
        public int? Action { get; set; }
        public int IdmDsbChargeId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? SecurityCd { get; set; }
        public int? ChargeTypeCd { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:n2}")]
        public decimal? SecurityValue { get; set; }
        public string? NocIssueBy { get; set; }
        public string? NocIssueTo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? NocDate { get; set; }
        public string? AuthLetterBy { get; set; }
        public DateTime? AuthLetterDate { get; set; }
        public DateTime? BoardResolutionDate { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? MoeInsuredDate { get; set; }
        public string? RequestLtrNo { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime? RequestLtrDate { get; set; }
        public string? BankIfscCd { get; set; }
        public string? BankRequestLtrNo { get; set; }
        public DateTime? BankRequestLtrDate { get; set; }
        public string? ChargePurpose { get; set; }
        public string? ChargeDetails { get; set; }
        public string? ChargeConditions { get; set; }
        public string? UploadDocument { get; set; }
        public string? ApprovedEmpId { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? BankIfscId { get; set; }
        public string SecurityDets { get; set; }
        public virtual SecurityMasterDTO TblSecurityRefnoMast { get; set; }
        public virtual ChargeTypeDTO TblChargeType { get; set; }
        public virtual IfscMasterDTO TbIIfscMaster { get; set; }
        public virtual IdmFileUploadDto FileUploadDto { get; set; }
    }
}