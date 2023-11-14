using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TbIIfscMaster
    {
        public int IFSCRowID { get; set; }
        public string IFSCCode { get; set; }
        public string BankName { get; set; }
        public string BranchName { get; set; }
        public string BankAddress { get; set; }
        public string BankState { get; set; }  
        public string BankDistrict { get; set; }
        public string BankTaluk { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? CreatedDate { get; set; }
         public virtual TblIdmDsbCharge TblIdmDsbCharge { get; set; }
         public virtual IdmPromoterBankDetails IdmPromoterBankDetails { get; set; }
    }
}
