using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EpicUpload
{
    public class FileUploadRequest
    {
        public string FileName { get; set; }
        public string TableName { get; set; }
        public bool IsCompleteRefresh { get; set; }
        public byte[] FileContent { get; set; }
    }
}
