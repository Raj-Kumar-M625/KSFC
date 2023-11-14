using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace EpicCrmWebApi
{
    public class MinimumResponse
    {
        public DateTime DateTimeUtc { get; set; }

        public HttpStatusCode StatusCode { get; set; }

        public string Content { get; set; }

        public bool EraseData { get; set; }
        public bool EnableLogging { get; set; }

        public MinimumResponse()
        {
            DateTimeUtc = DateTime.UtcNow;
            Content = "";
            StatusCode = HttpStatusCode.BadRequest;
            EraseData = false;
            EnableLogging = false;
        }
    }
}