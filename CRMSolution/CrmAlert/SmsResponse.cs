using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrmAlert
{
    public class Message2
    {
        public int num_parts { get; set; }
        public string sender { get; set; }
        public string content { get; set; }
    }

    public class Message3
    {
        public int id { get; set; }
        public string recipient { get; set; }
    }

    public class Message
    {
        public int balance { get; set; }
        public int batch_id { get; set; }
        public int cost { get; set; }
        public int num_messages { get; set; }
        public Message2 message { get; set; }
        public string receipt_url { get; set; }
        public string custom { get; set; }
        public List<Message3> messages { get; set; }
    }

    public class Error
    {
        public int code { get; set; }
        public string message { get; set; }
    }

    public class MessagesNotSent
    {
        public string unique_id { get; set; }
        public string number { get; set; }
        public string message { get; set; }
        public Error error { get; set; }
    }

    public class SmsResponseFormatBase
    {
        public bool test { get; set; }
        public int balance_pre_send { get; set; }
        public int total_cost { get; set; }
        public int balance_post_send { get; set; }
        public string status { get; set; }
    }

    public class SmsResponseFormat : SmsResponseFormatBase
    {
        public List<Message> messages { get; set; }
        public List<MessagesNotSent> messages_not_sent { get; set; }
    }

    public class SmsResponseLog : SmsResponseFormatBase
    {
        public int MessagesSent { get; set; }
        public int MessagesNotSent { get; set; }
        public string CurrentDateTime { get; set; }
        public string LogFileName { get; set; }
    }
}
