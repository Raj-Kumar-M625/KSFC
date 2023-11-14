using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class UnitInfo1
    {
        public long UtCd { get; set; }
        public byte? UtOffc { get; set; }
        public string UtName { get; set; }
        public string UtRadr1 { get; set; }
        public string UtRadr2 { get; set; }
        public string UtRadr3 { get; set; }
        public int? UtPin { get; set; }
        public string UtRgrms { get; set; }
        public long? UtRtel1 { get; set; }
        public long? UtRtel2 { get; set; }
        public string UtRetelx { get; set; }
        public string UtRfax { get; set; }
        public string UtOadr1 { get; set; }
        public string UtOadr2 { get; set; }
        public string UtOadr3 { get; set; }
        public int? UtOpin { get; set; }
        public string UtOgrms { get; set; }
        public long? UtOtel1 { get; set; }
        public long? UtOtel2 { get; set; }
        public string UtOtlx { get; set; }
        public string UtOfax { get; set; }
        public string UtFadr1 { get; set; }
        public string UtFadr2 { get; set; }
        public string UtFadr3 { get; set; }
        public long? UtFtel { get; set; }
        public byte? UtCnst { get; set; }
        public int? UtInd { get; set; }
        public int? UtSize { get; set; }
        public int? UtProd1 { get; set; }
        public int? UtProd2 { get; set; }
        public int? UtProd3 { get; set; }
        public int? UtProd4 { get; set; }
        public int? UtNriInvst { get; set; }
        public string UtFcollab { get; set; }
        public int? UtChfProm { get; set; }
        public byte? UtDist { get; set; }
        public byte? UtTlq { get; set; }
        public byte? UtKzone { get; set; }
        public int? UtHob { get; set; }
        public int? UtVil { get; set; }
        public byte? UtSzone { get; set; }
        public DateTime? UtIncDate { get; set; }
        public string UtBanker { get; set; }
        public string UtCadrs { get; set; }
        public byte? UnitId { get; set; }
        public string UtRstdCd { get; set; }
        public string UtOstdCd1 { get; set; }
        public string UtOstdCd2 { get; set; }
        public string UtBankAcno { get; set; }
        public string UtFstdCd { get; set; }
        public byte? UtPromOffc { get; set; }
        public int? UtCorpCd { get; set; }
        public string UtWeb { get; set; }
        public string UtEmail { get; set; }
        public string UtBankAdr1 { get; set; }
        public string UtBankAdr2 { get; set; }
        public string UtBankAdr3 { get; set; }
        public string UtBankAdr4 { get; set; }
        public long? UtBankTel1 { get; set; }
        public long? UtBankTel2 { get; set; }
        public string UtRebType { get; set; }
        public string UtGstNo { get; set; }
        public string UtPan { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual HobCdtab UtHobNavigation { get; set; }
        public virtual TblIndCdtab UtIndNavigation { get; set; }
        public virtual OffcCdtab UtOffcNavigation { get; set; }
        public virtual TblSizeCdtab UtSizeNavigation { get; set; }
        public virtual VilCdtab UtVilNavigation { get; set; }
    }
}
