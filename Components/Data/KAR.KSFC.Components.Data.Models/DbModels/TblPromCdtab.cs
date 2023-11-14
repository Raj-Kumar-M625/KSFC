using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblPromCdtab
    {
        public TblPromCdtab()
        {
            TblEnqGassetDets = new HashSet<TblEnqGassetDet>();
            TblEnqGbankDets = new HashSet<TblEnqGbankDet>();
            TblEnqGliabDets = new HashSet<TblEnqGliabDet>();
            TblEnqGnwDets = new HashSet<TblEnqGnwDet>();
            TblEnqGuarDets = new HashSet<TblEnqGuarDet>();
            TblEnqPassetDets = new HashSet<TblEnqPassetDet>();
            TblEnqPbankDets = new HashSet<TblEnqPbankDet>();
            TblEnqPliabDets = new HashSet<TblEnqPliabDet>();
            TblEnqPnwDets = new HashSet<TblEnqPnwDet>();
            TblEnqPromDets = new HashSet<TblEnqPromDet>();
        }

        public long PromoterCode { get; set; }
        public string PromoterPan { get; set; }
        public string PromoterName { get; set; }
        public DateTime? PromoterDob { get; set; }
        public string PromoterGender { get; set; }
        public string PromoterPassport { get; set; }
        public string PromoterPhoto { get; set; }

        public DateTime? PanValidationDate { get; set; }
        public string PromoterEmailid { get; set; }
        public long? PromoterMobno { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblEnqGassetDet> TblEnqGassetDets { get; set; }
        public virtual ICollection<TblEnqGbankDet> TblEnqGbankDets { get; set; }
        public virtual ICollection<TblEnqGliabDet> TblEnqGliabDets { get; set; }
        public virtual ICollection<TblEnqGnwDet> TblEnqGnwDets { get; set; }
        public virtual ICollection<TblEnqGuarDet> TblEnqGuarDets { get; set; }
        public virtual ICollection<TblEnqPassetDet> TblEnqPassetDets { get; set; }
        public virtual ICollection<TblEnqPbankDet> TblEnqPbankDets { get; set; }
        public virtual ICollection<TblEnqPliabDet> TblEnqPliabDets { get; set; }
        public virtual ICollection<TblEnqPnwDet> TblEnqPnwDets { get; set; }
        public virtual ICollection<TblEnqPromDet> TblEnqPromDets { get; set; }
        public virtual IdmPromAssetDet IdmPromAssetDet { get; set; }

      //  public virtual IdmPromoter IdmPromoter { get; set; }    
    }
}
