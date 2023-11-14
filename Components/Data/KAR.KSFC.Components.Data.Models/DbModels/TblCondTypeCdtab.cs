using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblCondTypeCdtab
    {
        public byte CondTypeCd { get; set; }
        public string CondTypeDets { get; set; }
        public int CondTypeDisSeq { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual TblIdmCondDet TblIdmCondDet { get; set; }
    }
}
