using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class OffcCdtab
    {
        public OffcCdtab()
        {
            TblChairCdtabs = new HashSet<TblChairCdtab>();
            TblEmpchairDets = new HashSet<TblEmpchairDet>();
            TblEmpchairhistDets = new HashSet<TblEmpchairhistDet>();
            TblEmploginTabs = new HashSet<TblEmploginTab>();
            TblEnqBasicDets = new HashSet<TblEnqBasicDet>();
            UnitInfo1s = new HashSet<UnitInfo1>();
        }

        public byte OffcCd { get; set; }
        public string OffcNam { get; set; }
        public string OffcAdr1 { get; set; }
        public string OffcAdr2 { get; set; }
        public string OffcAdr3 { get; set; }
        public int? OffcPin { get; set; }
        public int? OffcTel1 { get; set; }
        public int? OffcTel2 { get; set; }
        public int? OffcTel3 { get; set; }
        public int? OffcTlx2 { get; set; }
        public int? OffcFax { get; set; }
        public byte? OffcDist { get; set; }
        public byte? OffcZone { get; set; }
        public int? OffcBmcd { get; set; }
        public string OffcIfsCd { get; set; }
        public string OffcInopbnkacNo { get; set; }
        public string OffcMailId { get; set; }
        public string OffcBmMailId { get; set; }
        public string OffcStNo { get; set; }
        public string OffcTaxNo { get; set; }
        public string OffcBsrCd { get; set; }
        public string OffcStdCd { get; set; }
        public string OffcNamKannada { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual DistCdtab OffcDistNavigation { get; set; }
        public virtual KznCdtab OffcZoneNavigation { get; set; }
        public virtual TblAppLoanMast TblAppLoanMast { get; set; }
        public virtual ICollection<TblChairCdtab> TblChairCdtabs { get; set; }
        public virtual ICollection<TblEmpchairDet> TblEmpchairDets { get; set; }
        public virtual ICollection<TblEmpchairhistDet> TblEmpchairhistDets { get; set; }
        public virtual ICollection<TblEmploginTab> TblEmploginTabs { get; set; }
        public virtual ICollection<TblEnqBasicDet> TblEnqBasicDets { get; set; }
        public virtual ICollection<UnitInfo1> UnitInfo1s { get; set; }

        public virtual TblIdmLegalWorkflow TblIdmLegalWorkflow { get; set; } //by gowtham s on 2/8/22

        //public virtual TblIdmIrLand TblIdmIrLand { get; set; }

        //public virtual TblIdmSidbiApproval TblIdmSidbiApproval { get; set; } // By Dev on 19/08/2022
        //public virtual TblIdmFirstInvestmentClause TblIdmFirstInvestmentClause { get; set; } // By Akhila on 19/08/2022
    }
}
