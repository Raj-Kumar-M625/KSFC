using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblEnqTemptab
    {
        public TblEnqTemptab()
        {
            TblEnqAddressDets = new HashSet<TblEnqAddressDet>();
            TblEnqBankDets = new HashSet<TblEnqBankDet>();
            TblEnqBasicDets = new HashSet<TblEnqBasicDet>();
            TblEnqDocDets = new HashSet<TblEnqDocDet>();
            TblEnqGassetDets = new HashSet<TblEnqGassetDet>();
            TblEnqGbankDets = new HashSet<TblEnqGbankDet>();
            TblEnqGliabDets = new HashSet<TblEnqGliabDet>();
            TblEnqGnwDets = new HashSet<TblEnqGnwDet>();
            TblEnqGuarDets = new HashSet<TblEnqGuarDet>();
            TblEnqMftotalDets = new HashSet<TblEnqMftotalDet>();
            TblEnqPassetDets = new HashSet<TblEnqPassetDet>();
            TblEnqPbankDets = new HashSet<TblEnqPbankDet>();
            TblEnqPjcostDets = new HashSet<TblEnqPjcostDet>();
            TblEnqPjfinDets = new HashSet<TblEnqPjfinDet>();
            TblEnqPjmfDets = new HashSet<TblEnqPjmfDet>();
            TblEnqPliabDets = new HashSet<TblEnqPliabDet>();
            TblEnqPnwDets = new HashSet<TblEnqPnwDet>();
            TblEnqPromDets = new HashSet<TblEnqPromDet>();
            TblEnqRegnoDets = new HashSet<TblEnqRegnoDet>();
            TblEnqSecDets = new HashSet<TblEnqSecDet>();
            TblEnqSisDets = new HashSet<TblEnqSisDet>();
            TblEnqTrcostDets = new HashSet<TblEnqTrcostDet>();
            TblEnqWcDets = new HashSet<TblEnqWcDet>();
        }

        public int EnqtempId { get; set; }
        public string PromPan { get; set; }
        public DateTime? EnqInitDate { get; set; }
        public int? EnqRefNo { get; set; }
        public string EnqNote { get; set; }
        public DateTime? EnqSubmitDate { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }
        public int? EnqStatus { get; set; }
        public bool? HasAssociateSisterConcern { get; set; }

        public virtual ICollection<TblEnqAddressDet> TblEnqAddressDets { get; set; }
        public virtual ICollection<TblEnqBankDet> TblEnqBankDets { get; set; }
        public virtual ICollection<TblEnqBasicDet> TblEnqBasicDets { get; set; }
        public virtual ICollection<TblEnqDocDet> TblEnqDocDets { get; set; }
        public virtual ICollection<TblEnqGassetDet> TblEnqGassetDets { get; set; }
        public virtual ICollection<TblEnqGbankDet> TblEnqGbankDets { get; set; }
        public virtual ICollection<TblEnqGliabDet> TblEnqGliabDets { get; set; }
        public virtual ICollection<TblEnqGnwDet> TblEnqGnwDets { get; set; }
        public virtual ICollection<TblEnqGuarDet> TblEnqGuarDets { get; set; }
        public virtual ICollection<TblEnqMftotalDet> TblEnqMftotalDets { get; set; }
        public virtual ICollection<TblEnqPassetDet> TblEnqPassetDets { get; set; }
        public virtual ICollection<TblEnqPbankDet> TblEnqPbankDets { get; set; }
        public virtual ICollection<TblEnqPjcostDet> TblEnqPjcostDets { get; set; }
        public virtual ICollection<TblEnqPjfinDet> TblEnqPjfinDets { get; set; }
        public virtual ICollection<TblEnqPjmfDet> TblEnqPjmfDets { get; set; }
        public virtual ICollection<TblEnqPliabDet> TblEnqPliabDets { get; set; }
        public virtual ICollection<TblEnqPnwDet> TblEnqPnwDets { get; set; }
        public virtual ICollection<TblEnqPromDet> TblEnqPromDets { get; set; }
        public virtual ICollection<TblEnqRegnoDet> TblEnqRegnoDets { get; set; }
        public virtual ICollection<TblEnqSecDet> TblEnqSecDets { get; set; }
        public virtual ICollection<TblEnqSisDet> TblEnqSisDets { get; set; }
        public virtual ICollection<TblEnqTrcostDet> TblEnqTrcostDets { get; set; }
        public virtual ICollection<TblEnqWcDet> TblEnqWcDets { get; set; }
    }
}
