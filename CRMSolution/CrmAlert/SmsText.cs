using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmAlert
{
    public class SmsText
    {
        public string number { get; set; }
        public string text { get; set; }
    }

    public class SMSMessages
    {
        public bool test { get; set; }
        public string sender { get; set; }
        public List<SmsText> messages { get; set; }
    }
}
