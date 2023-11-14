using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblAppUnitDetail
    {
        public TblAppUnitDetail()
        {
            TblAppTeamDets = new HashSet<TblAppTeamDet>();
            TblAppUnitAddresses = new HashSet<TblAppUnitAddress>();
            TblAppUnitBanks = new HashSet<TblAppUnitBank>();
            TblAppUnitLoanDets = new HashSet<TblAppUnitLoanDet>();
            TblAppUnitProducts = new HashSet<TblAppUnitProduct>();
        }

        public int UtRowid { get; set; }
        public int UtCd { get; set; }
        public int? EgNo { get; set; }
        public int? UtName { get; set; }
        public byte? CnstCd { get; set; }
        public DateTime? IncorporationDt { get; set; }
        public byte? KznCd { get; set; }
        public string UtZone { get; set; }
        public int? SizeCd { get; set; }
        public string UnitPan { get; set; }
        public string UnitGstin { get; set; }
        public int IndCd { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual ICollection<TblAppTeamDet> TblAppTeamDets { get; set; }
        public virtual ICollection<TblAppUnitAddress> TblAppUnitAddresses { get; set; }
        public virtual ICollection<TblAppUnitBank> TblAppUnitBanks { get; set; }
        public virtual ICollection<TblAppUnitLoanDet> TblAppUnitLoanDets { get; set; }
        public virtual ICollection<TblAppUnitProduct> TblAppUnitProducts { get; set; }

       

        public virtual TblIndCdtab ProdIndNavigation { get; set; }
    }
}
