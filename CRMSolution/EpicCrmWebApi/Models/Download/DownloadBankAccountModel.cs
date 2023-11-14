using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpicCrmWebApi
{
    // model used to send bank accounts where sales person can deposit cheques
    public class DownloadBankAccountModel
    {
        public string BankName { get; set; }
        public string BankPhone { get; set; }
        public string AccountNumber { get; set; }
        public string IFSC { get; set; }
        public string Branch { get; set; }
    }
}