using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EpicUpload
{
    public class FileUploadResponse
    {
        public DateTime DateTimeUtc { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public string Content { get; set; }
        public long RequestId { get; set; }
    }
}
