using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConApp
{
    public class SingleSms
    {
        public string number { get; set; }
        public string text { get; set; }
    }

    public class SMSMessages
    {
        public bool test { get; set; }
        public string sender { get; set; }
        public List<SingleSms> messages { get; set; }
    }
}


