using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    public class UnSownAgreementModel
    {
        public string TypeName { get; set; }
        public string AgreementNumber { get; set; }
        public string Status { get; set; }

        public int ActivityCount { get; set; }
        public Decimal TotalAdvanceRequested { get; set; }
        public Decimal TotalAdvanceApproved { get; set; }
    }
}