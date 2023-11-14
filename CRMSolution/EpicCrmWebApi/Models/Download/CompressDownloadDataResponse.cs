using DomainEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class CompressDownloadDataResponse
    {
        public bool Status { get; set; }
        public DateTime UtcTimestamp { get; set; }
        public string EncryptedData { get; set; }
        public string ErrorMessage { get; set; }
    }
}