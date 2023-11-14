using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class SqliteDomainSTRData
    {
        public string PhoneDbId { get; set; }
        public string STRNumber { get; set; }
        public string VehicleNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public long DWSCount { get; set; }
        public long BagCount { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal NetWeight { get; set; }
        public long StartOdometer { get; set; }
        public long EndOdometer { get; set; }
        public DateTime STRDate { get; set; }
        public bool IsNew { get; set; } = true;
        public bool IsTransferred { get; set; } = false;
        public string TransfereeName { get; set; }
        public string TransfereePhone { get; set; }

        public DateTime TimeStamp { get; set; }
        public string ActivityId { get; set; }
        public DateTime TimeStamp2 { get; set; }
        public string ActivityId2 { get; set; }

        public List<SqliteDomainDWSData> DomainDWSData { get; set; }
        public IEnumerable<String> Images { get; set; }
    }

    public class SqliteSTRData : SqliteDomainSTRData
    {
        public long Id { get; set; }
        public long BatchId { get; set; }
        public long EmployeeId { get; set; }
        public bool IsProcessed { get; set; }
        public long STRId { get; set; }
        public int ImageCount { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime DateUpdated { get; set; }
    }
}
