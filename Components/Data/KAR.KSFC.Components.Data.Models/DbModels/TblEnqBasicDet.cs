using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqBasicDet
    {
        public int EnqBdetId { get; set; }
        public int? EnqtempId { get; set; }
        public string EnqApplName { get; set; }
        public string EnqAddress { get; set; }
        public string EnqPlace { get; set; }
        public int? EnqPincode { get; set; }
        public string EnqEmail { get; set; }
        public int? AddlLoan { get; set; }
        public string UnitName { get; set; }
        public int? EnqRepayPeriod { get; set; }
        public int? EnqLoanamt { get; set; }
        public int? ConstCd { get; set; }
        public int? PurpCd { get; set; }
        public int SizeCd { get; set; }
        public int ProdCd { get; set; }
        public int VilCd { get; set; }
        public int PremCd { get; set; }
        public int IndCd { get; set; }
        public byte OffcCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public int? PromotorClass { get; set; }
        public virtual TblCnstCdtab ConstCdNavigation { get; set; }
        public virtual TblEnqTemptab Enqtemp { get; set; }
        public virtual OffcCdtab OffcCdNavigation { get; set; }
        public virtual TblPremCdtab PremCdNavigation { get; set; }
        public virtual TblPurpCdtab PurpCdNavigation { get; set; }
        public virtual TblSizeCdtab SizeCdNavigation { get; set; }
        public virtual VilCdtab VilCdNavigation { get; set; }
    }
}
