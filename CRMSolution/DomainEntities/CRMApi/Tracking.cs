using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Tracking : EntityRequestBase
    {
        public long EmployeeDayId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public long ActivityId { get; set; } = 0;
        public bool IsMileStone { get; set; } = false;
        public bool IsStartOfDay { get; set; } = false;
        public bool IsEndOfDay { get; set; } = false;
    }
}
