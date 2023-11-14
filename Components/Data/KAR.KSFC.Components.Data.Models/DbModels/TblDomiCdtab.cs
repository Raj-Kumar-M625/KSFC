using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblDomiCdtab
    {
        public TblDomiCdtab()
        {
            TblEnqGuarDets = new HashSet<TblEnqGuarDet>();
            TblEnqPromDets = new HashSet<TblEnqPromDet>();
        }

        public int DomCd { get; set; }
        public string DomDets { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? UniqueId { get; set; }

        public virtual ICollection<TblEnqGuarDet> TblEnqGuarDets { get; set; }
        public virtual ICollection<TblEnqPromDet> TblEnqPromDets { get; set; }
    }
}
