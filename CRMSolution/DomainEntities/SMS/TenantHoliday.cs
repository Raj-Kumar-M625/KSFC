using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class TenantHoliday
    {
        public int Id { get; set; }
        public long TenantId { get; set; }
        public DateTime HolidayDate { get; set; }
        public string Description { get; set; }
        public string TenantName { get; set; }
    }
}
