using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class CodeTable
    {

        public int Id { get; set; }
        public string ModuleName { get; set; }
        public string CodeType { get; set; }
        public string CodeName { get; set; }
        public string CodeValue { get; set; }
        public string DisplaySequence { get; set; }
        public bool IsActive { get; set; }
        public virtual TblLaReceiptDet TblLaReceiptDet { get; set; }
        
    }
}
