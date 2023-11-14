using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class Tblfm8fm13CDTab
    {
        public int FormTypeCD { get; set; }
        public string? FormType { get; set; }
      
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public string? CreatedBy { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public virtual TblIdmDsbFm813 TblIdmDsbFm813 { get; set; }
    }
}
