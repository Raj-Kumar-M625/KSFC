using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class TblCondStgMast
    {
        public byte CondStgId { get; set; }
        public int? CondStgCode { get; set; }
        public string CondStgDesc { get; set; }
        public virtual TblAddlCondDet TblAddlCondDet { get; set; }   
    }
}
