using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public class SecurityTypeCdtab
    {
        public int RowID { get; set; }
        public int CodeValue { get; set; }
        public string CodeText { get; set; }
        public bool IsActive { get; set; }
    }
}
