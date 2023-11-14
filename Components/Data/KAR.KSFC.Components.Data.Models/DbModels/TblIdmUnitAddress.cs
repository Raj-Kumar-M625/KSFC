using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    //Added by Gagana
    public partial class TblIdmUnitAddress
    {
        public int IdmUtAddressRowid { get; set; }
        public long? LoanAcc { get; set; }
        public int? LoanSub { get; set; }
        public byte? OffcCd { get; set; }
        public int UtCd { get; set; }
        public int AddtypeCd { get; set; }
        public string UtAddress { get; set; }
        public string UtArea { get; set; }
        public string UtCity { get; set; }
        public int? UtPincode { get; set; }
        public int? UtTelephone { get; set; }
        public int? UtMobile { get; set; }
        public int? UtAltMobile { get; set; }
        public string UtEmail { get; set; }
        public string UtAltEmail { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public byte? UtDistCd { get; set; }
        public int? UtTlqCd { get; set; }
        public int? UtHobCd { get; set; }
        public int? UtVilCd { get; set; }
        public virtual TblAddressCdtab TblAddressCdtab { get; set; }
    }
}
