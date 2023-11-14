using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteActionLocationDisplayData
    {
        public long Id { get; set; }
        public long SqliteActionId { get; set; }
        public string Source { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public DateTime At { get; set; }
        public string LocationTaskStatus { get; set; }
        public string LocationException { get; set; }
        public bool IsGood { get; set; }
    }
}