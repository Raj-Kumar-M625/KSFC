using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class LDCheckListDTO
    {
        public int? PrimarySecurityCount { get; set; }
        public int? PrimarySecurityEdit { get; set; }
        public int? GuarantorDeedCount { get; set; }
        public int? GuarantorDeedEdit { get; set; }
        public int? HypothecationCount { get; set; }
        public int? HypothecationEdit { get; set; }
        public int? SecurityChargeCount { get; set; }
        public int? SecurityChargeEdit { get; set; }
        public int? CersaiCount { get; set; }
        public int? CersaiEdit { get; set; }
        public int? ConditionCount { get; set; }
    }
}
