using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ApproveDataResponse
    {
        public int Status { get; set; }
        public string Message { get; set; }
        public string ApprovedBy { get; set; }
    }
}