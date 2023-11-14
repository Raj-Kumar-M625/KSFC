using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class SmsData
    {
        public string AgreementNumber { get; set; }
        public string TypeName { get; set;}  // crop name
        public string EntityName { get; set; }
        public string FieldPersonName { get; set; }
        public string PhoneNumber { get; set; }
        public string CompanyName { get; set; }
        public decimal AmountRequested { get; set; }
        public decimal AmountApproved { get; set; }
        // used in issue / return sms
        public long Quantity { get; set; }
        public string ItemCode { get; set; }
        public string ItemUnit { get; set; }
        public string ItemDetails { get; set; }
    }

    // used to parse the data coming as XML in TenantSmsData table
    public class OuterSmsData
    {
        public SmsData Row { get; set; }
    }
}
