using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class MessageOnDemandResponse
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public MessageOnDemandResponse()
        {
            Status = false;
            Message = "";
        }
    }
}