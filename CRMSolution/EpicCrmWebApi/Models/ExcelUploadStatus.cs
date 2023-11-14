using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ExcelUploadStatus
    {
        public long Id { get; set; }
        public string UploadType { get; set; }
        public string UploadFileName { get; set; }
        public long RecordCount { get; set; }
        public string RequestedBy { get; set; }
        public bool IsCompleteRefresh { get; set; }
        public DateTime RequestTimestamp { get; set; }
        public DateTime PostingTimestamp { get; set; }
        public bool IsPosted { get; set; }

        public string LocalFileName { get; set; }
        public bool IsParsed { get; set; }
        public int ErrorCount { get; set; }
        public bool IsLocked { get; set; }
        public System.DateTime LockTimestamp { get; set; }

        public bool DoesFileExist =>
                String.IsNullOrEmpty(UploadFileName) == false && System.IO.File.Exists(UploadFileName);
    }
}