using System;
using System.Collections.Generic;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class TblCondCdtab
    {
        public short CondCd { get; set; }
        public string CondDets { get; set; }
        public byte? CondStg { get; set; }
        public short CondSub { get; set; }
        public short? CondFlg { get; set; }
        public short? CondStatusFlag { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
      
    }
}
