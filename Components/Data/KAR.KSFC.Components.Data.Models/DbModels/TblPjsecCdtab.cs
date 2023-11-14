using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblPjsecCdtab
    {
        public TblPjsecCdtab()
        {
            TblEnqSecDets = new HashSet<TblEnqSecDet>();
        }

        public int SecCode { get; set; }
        public string SecDets { get; set; }
        public byte? SecFlg { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string UniqueId { get; set; }

        public virtual ICollection<TblEnqSecDet> TblEnqSecDets { get; set; }
        public virtual TblSecurityRefnoMast TblSecurityRefnoMast { get; set; }
    }
}
