using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteSTRData : SqliteBase
    {
        public string Id { get; set; }
        public string STRNumber { get; set; }
        public string VehicleNumber { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public int DWSCount { get; set; }
        public int BagCount { get; set; }
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

        public List<SqliteDWSData> DWS { get; set; }
    }
}