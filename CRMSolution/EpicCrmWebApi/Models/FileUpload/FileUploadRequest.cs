using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class FileUploadRequest
    {
        public string FileName { get; set; }
        public string TableName { get; set; }
        public bool IsCompleteRefresh { get; set; }
        public byte[] FileContent { get; set; }
    }
}