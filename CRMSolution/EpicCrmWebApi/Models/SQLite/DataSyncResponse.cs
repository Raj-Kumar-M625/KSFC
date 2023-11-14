using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class DataSyncResponse : MinimumResponse
    {
        public long BatchId { get; set; }
    }
}