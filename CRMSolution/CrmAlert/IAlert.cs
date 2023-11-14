using System.Collections.Generic;

namespace CrmAlert
{
    public interface IAlert
    {
        //string Send(string message);
        string Send(string message, IEnumerable<string> phoneNumbers);
        SmsResponseFormat SendBulk(SMSMessages smsMessages, string smsRequestLogFile, string smsResponseLogFile);

        bool ParseSendResponse(string responseReceived);
        string PutMessageInTemplate(string inputMessage, string template);
    }
}
