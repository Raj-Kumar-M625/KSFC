using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblAppUnitLoanDet
    {
        public int UtLoanRowid { get; set; }
        public int UtCd { get; set; }
        public int? EnqRefNo { get; set; }
        public decimal UtLoanAmt { get; set; }
        public int? SchemeCd { get; set; }
        public int? LoanType { get; set; }
        public int? PurpCd { get; set; }
        public int? ActivityType { get; set; }
        public DateTime? DateInfoReceive { get; set; }
        public DateTime? DateInfoComplete { get; set; }
        public int? SubsidyType { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual TblAppUnitDetail UtCdNavigation { get; set; }
    }
}
