using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblCondStgCdtab
    {
        public byte CondStgCd { get; set; }
        public string CondStgDets { get; set; }
        public int CondStgDisSeq { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string CreateBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        //public virtual TblIdmCondDet TblIdmCondDet { get; set; }
        public  virtual TblAddlCondDet TblAddlCondDet { get; set; }
    }
}

