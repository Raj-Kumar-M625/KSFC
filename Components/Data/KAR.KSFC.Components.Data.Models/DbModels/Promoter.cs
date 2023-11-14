using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class Promoter
    {
        public long? PromCode { get; set; }
        public byte? PromOffc { get; set; }
        public long? PromUnit { get; set; }
        public string PromName { get; set; }
        public string PromDesg { get; set; }
        public string PromSex { get; set; }
        public byte? PromAge { get; set; }
        public decimal? PromShare { get; set; }
        public byte? PromDom1 { get; set; }
        public byte? PromClas1 { get; set; }
        public byte? PromClas2 { get; set; }
        public string PromQual1 { get; set; }
        public string PromQual2 { get; set; }
        public byte? PromExpYrs { get; set; }
        public string PromExpDet { get; set; }
        public DateTime? PromJnDt { get; set; }
        public DateTime? PromExDt { get; set; }
        public byte? PromExAppBy { get; set; }
        public string PromNriCountry { get; set; }
        public string PromPadr1 { get; set; }
        public string PromPadr2 { get; set; }
        public string PromPadr3 { get; set; }
        public string PromPadr4 { get; set; }
        public string PromTadr1 { get; set; }
        public string PromTadr2 { get; set; }
        public string PromTadr3 { get; set; }
        public string PromTadr4 { get; set; }
        public byte? PromMajor { get; set; }
        public string PromNwDets { get; set; }
        public string PanNo { get; set; }
        public string PassNo { get; set; }
        public string PromGuardian { get; set; }
        public long? PromResTel { get; set; }
        public string PromMobile { get; set; }
        public string PromEmail { get; set; }
        public string PromPhyHandicap { get; set; }
        public string PromAadhaar { get; set; }
        public DateTime? PromDob { get; set; }
        public int? PromExApprEmp { get; set; }
        public long? PromoterCode { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual OffcCdtab PromOffcNavigation { get; set; }
        public virtual UnitInfo1 PromUnitNavigation { get; set; }
        public virtual TblPromCdtab PromoterCodeNavigation { get; set; }
    }
}
