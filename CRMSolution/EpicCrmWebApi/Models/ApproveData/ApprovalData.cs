using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ApprovalData
    {
        public long Id { get; set; }
        public string RefNo { get; set; }
        public decimal ApproveAmount { get; set; }
        public string Comment { get; set; }
        public DateTime ApproveDate { get; set; }
        public string ReportType { get; set; }
    }
}