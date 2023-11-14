using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainEntities
{
    public class ExcelUploadStatus
    {
        public long Id { get; set; }
        public long TenantId { get; set; }
        public string UploadType { get; set; }
        public string UploadTable { get; set; }
        public string UploadFileName { get; set; }
        public long RecordCount { get; set; }
        public string RequestedBy { get; set; }
        public bool IsCompleteRefresh { get; set; }
        public System.DateTime RequestTimestamp { get; set; }
        public System.DateTime PostingTimestamp { get; set; }
        public bool IsPosted { get; set; }

        public string LocalFileName { get; set; }
        public bool IsParsed { get; set; }
        public int ErrorCount { get; set; }
        public bool IsLocked { get; set; }
        public System.DateTime LockTimestamp { get; set; }

        public bool IsExcel => LocalFileName.ToLower().EndsWith("xlsx");

        public bool IsCSV
        {
            get
            {
                string fileNameAsLowerCase = LocalFileName.ToLower();
                return fileNameAsLowerCase.EndsWith("csv") || fileNameAsLowerCase.EndsWith("txt");
            }
        }
    }
}
