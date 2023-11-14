using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Dto.IDM
{
    public class TblIdmHypothMapDto
    {
        public int HypothMapId { get; set; }
        public int HypothCode { get; set; }
        public string HypothNo { get; set; }
        public string HypothDeedNo { get; set; }
        public decimal HypothValue { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

    }
}
