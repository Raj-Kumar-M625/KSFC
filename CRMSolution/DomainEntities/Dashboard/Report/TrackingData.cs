using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TrackingData
    {
        public string StartPosition { get; set; }
        public string EndPosition { get; set; }
        public decimal TrackingDistanceInMeters { get; set; }

        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
