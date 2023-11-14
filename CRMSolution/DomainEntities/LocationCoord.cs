using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class LocationCoord
    {
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public string LocSource { get; set; }
    }
}
