using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class FileUploadResponse : MinimumResponse
    {
        public long RequestId { get; set; }
    }
}