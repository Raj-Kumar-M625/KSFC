using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicUpload
{
    public class SiteAlert
    {
        public long SiteId { get; set; }
        public string SiteName { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDesc { get; set; }
        public DateTime UTCAT { get; set; }
    }
}
