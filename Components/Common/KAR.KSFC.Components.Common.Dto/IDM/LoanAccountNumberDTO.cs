using KAR.KSFC.Components.Common.Dto.AdminModule;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class LoanAccountNumberDTO
    {
        public string LoanOffice { get; set; }
        public string EncryptedLoanUnit { get; set; }
        public string LoanUnit { get; set; }
        public long? LoanAcc { get; set; }
        [NotMapped]
        public string? EncryptedLoanAcc { get; set; }

        public int? LoanSub { get; set; }

        public string? EncryptedLoanSub { get; set; }
        public string? EncryptedInUnit { get; set; }
        
        public long InMastId { get; set; }

        public string? EncryptedInOffc { get; set; }
        public byte? InOffc { get; set; }
        public int? InUnit { get; set; }
        public int? InSno { get; set; }
        public long? InNo { get; set; }
        public int? InTy { get; set; }  
        public decimal? InSanAmt { get; set; }
        public DateTime? InSanDt { get; set; }
        public int? InSchm { get; set; }
        public int? InStat { get; set; }
        public decimal? InIntrLow { get; set; }
        public decimal? InIntrHigh { get; set; }
        public decimal? InIntReb { get; set; }
        public int? InPmode { get; set; }
        public int? InImode { get; set; }
        public int? InMortPrd { get; set; }
        public int? UnitId { get; set; }
        public int? InPurTy { get; set; }
        public int? InSub { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        // public byte OffcCd { get; set; }

        [NotMapped]
        public bool? ShowView { get; set; }
        public virtual EmployeeChairDetailsDTO ChairCode { get; set; }
       public virtual EmployeeChairDetailsDTO OffcCd { get; set; }

        public virtual OfficeDto OffcCdtab { get; set; }
        public virtual UnitMasterDto TblUnitMast { get; set; }
        //  public virtual ChairDTO OffcCd { get; set; }

        public virtual LegalWorkFlowDto TblIdmLegalWorkflow { get; set; } //by gowtham s 

        // public virtual TblEmpchairDetsDto TblEmpchairDets { get; set; } //by gowtham s 

        public virtual IdmSidbiApprovalDTO TblIdmSidbiApproval { get; set; }
    }
}
