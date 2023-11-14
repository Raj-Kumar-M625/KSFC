using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    // Entity Bank Details
    public class DownloadEntityBankDetailModel
    {
        public string AccountNumber { get; set; }
        public string AccountHolderName { get; set; }
        public bool IsSelf { get; set; }
        public string BankName { get; set; }
        public string BankBranch { get; set; }

        public string Status { get; set; }
        public bool IsApproved { get; set; }
        public string Comments { get; set; }
    }
}