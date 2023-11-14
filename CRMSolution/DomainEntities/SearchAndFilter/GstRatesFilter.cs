using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
   public class GstRateFilter
    {
        public bool ApplyGstCodeFilter { get; set; }
        public string GstCode { get; set; }

        public bool ApplyDateFilter { get; set; }
        public DateTime SearchDate { get; set; }
    }
}
