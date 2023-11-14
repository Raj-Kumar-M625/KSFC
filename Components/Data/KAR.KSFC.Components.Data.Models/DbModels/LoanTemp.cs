using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class LoanTemp
    {
        public int RowID { get; set; }
        public string LoanAccNo { get; set; }
        public int LoanID { get; set; }
        public int? SubLoan { get; set; }
        public string UnitName { get; set; }
        public bool IsActive { get; set; }
    }
}
