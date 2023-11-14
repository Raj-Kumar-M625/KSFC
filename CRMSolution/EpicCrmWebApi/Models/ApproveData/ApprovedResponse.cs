using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class ApprovedResponse
    {
        public long Id { get; set; }
        public string RefNo { get; set; }
        public decimal ApproveAmount { get; set; }
        public string Comment { get; set; }
        public string ApproveDate { get; set; }
        public bool IsApproved { get; set; }
        public string ApprovedBy { get; set; }
    }
}