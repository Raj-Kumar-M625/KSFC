using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM.Disbursement
{
    public class RelaxationDTO
    {
        public long? LoanAcc { get; set; }
        public long RelaxCondId { get; set; }
        public string ConditionDetails { get; set; }
        public byte ConditionType { get; set; }
        public string CondTypeDet { get; set; }
        public bool? WhRelAllowed { get; set; }
        public bool? IsActive { get; set; }
        public bool? WhRelaxation { get; set; }
        public string ModelName { get; set; }
    }
}
