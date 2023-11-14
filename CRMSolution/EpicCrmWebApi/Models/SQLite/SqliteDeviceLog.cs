using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class SqliteDeviceLog
    {
        public DateTime TimeStamp { get; set; }
        public string LogText { get; set; }
    }
}