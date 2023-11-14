using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class Activity : EntityRequestBase
    {
        public long EmployeeDayId { get; set; }
        public string ClientName { get; set; }
        public string ClientPhone { get; set; }
        public string ClientType { get; set; }
        public string ActivityType { get; set; }
        public string Comments { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int ImageCount { get; set; }
        public string ClientCode { get; set; }
        public bool AtBusiness { get; set; }
        public decimal ActivityAmount { get; set; }
        public int ContactCount { get; set; }
        public int ActivityTrackingType { get; set; }
        //public IEnumerable<byte[]> Images { get; set; }
        public IEnumerable<string> Images { get; set; }
        public IEnumerable<ActivityContact> Contacts { get; set; }
    }
}
