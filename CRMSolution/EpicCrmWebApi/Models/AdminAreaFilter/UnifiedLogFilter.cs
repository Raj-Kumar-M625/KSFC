using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class UnifiedLogFilter
    {
        public string LogType { get; set; }

        public int StartItem { get; set; }
        public int ItemCount { get; set; }
        public string ProcessFilter { get; set; }
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
    }
}