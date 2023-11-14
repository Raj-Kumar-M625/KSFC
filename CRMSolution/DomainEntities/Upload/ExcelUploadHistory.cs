using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ExcelUploadHistory
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public string UploadType { get; set; }
        public string UploadFileName { get; set; }
        public long RecordCount { get; set; }
        public string RequestedBy { get; set; }
        public bool IsCompleteRefresh { get; set; }
        public System.DateTime RequestTimestamp { get; set; }
        public System.DateTime PostingTimestamp { get; set; }
    }
}
