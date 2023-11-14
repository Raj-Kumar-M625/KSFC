using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblAppUnitBank
    {
        public int UtBankRowid { get; set; }
        public int UtCd { get; set; }
        public int? EnqRefNo { get; set; }
        public string UtIfsc { get; set; }
        public string UtBank { get; set; }
        public string UtBankBranch { get; set; }
        public string UtBankAddress { get; set; }
        public string UtBankArea { get; set; }
        public string UtBankCity { get; set; }
        public int? UtBankPhone { get; set; }
        public bool? UtBankPrimary { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblAppUnitDetail UtCdNavigation { get; set; }
    }
}
