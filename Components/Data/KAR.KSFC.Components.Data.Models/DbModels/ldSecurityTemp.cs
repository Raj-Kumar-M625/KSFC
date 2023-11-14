using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class ldSecurityTemp
    {
        public ldSecurityTemp()
        {
            SecurityCategoryCdtabs = new HashSet<SecurityCategoryCdtab>();
        }   
        public int RowID { get; set; }
        public string SecurityHolder { get; set; }
        public string SecurityCategory { get; set; }
        public string SecurityType { get; set; }
        public string SecurityDescription { get; set; }
        public string SecurityDeedNo { get; set; }
        public string DeedDescription { get; set; }
        public string SubRegistrarOffice { get; set; }
        public DateTime DateOfExecution { get; set; }
        public int ValueInLakhs { get; set; }
        public string AccountNumber { get; set; }
        public bool IsActive { get; set; }
        public virtual ICollection<SecurityCategoryCdtab> SecurityCategoryCdtabs { get; set; }
    }
}
