using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class StartDay : EntityRequestBase
    {
        public long EmployeeId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }

        public string PhoneModel { get; set; }
        public string PhoneOS { get; set; }
        public string AppVersion { get; set; }
    }
}
