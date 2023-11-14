using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.UnitDetails
{
    public class IdmChangeBankDetailsDTO
    {
        public int IdmUtBankRowId { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int? UtCd { get; set; }
        public string UtIfsc { get; set; }
        public int? BankIfscId { get; set; }
        public int? UtBankPincode { get; set; }
        public string UtBank { get; set; }
        public string UtBankBranch { get; set; }
        public string UtBankAddress { get; set; }
        public string UtBankArea { get; set; }
        public string UtBankCity { get; set; }
        public string UtBankPhone { get; set; }
        public bool UtBankPrimary { get; set; }
        public string UtBankState { get; set; }
        public string UtBankDistrict { get; set; }
        public string UtBankTaluka { get; set; }
        public string UtBankAccno { get; set; }
        public string UtBankHolderName { get; set; }
        public string AccType { get; set; }

        public int? UtAccType { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string UniqueId { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? Action { get; set; }
    }
}
