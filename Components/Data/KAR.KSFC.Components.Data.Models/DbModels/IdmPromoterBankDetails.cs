using Org.BouncyCastle.Asn1.Crmf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class IdmPromoterBankDetails
    {
        public int IdmPromBankId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte?  OffcCd { get; set; }
        public int? UtCd { get; set; }
        public long PromoterCode { get; set; }
        public int? PrmAcType { get; set; }
        public string PrmBankName { get; set; }
        public string PrmBankBranch { get; set; }
        public string PrmBankLoc { get; set; }
        public long? PrmAcNo { get; set; }
        public int? PrmIfscId { get; set; }
        public string PrmBankAddress { get; set; }
        public string PrmBankState { get; set; }
        public string PrmBankDistrict { get; set; }
        public string PrmBankTaluk { get; set; }
        public int? PrmBankPincode { get; set; }
        public string PrmBankAcName { get; set; }
        public bool PrmPrimaryBank { get; set; }
        public int? PrmCibilScore { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }   
        public virtual TbIIfscMaster TbIIfscMaster { get; set; }       
                
    }
}
