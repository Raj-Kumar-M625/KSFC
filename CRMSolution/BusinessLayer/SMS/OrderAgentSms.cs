using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using DatabaseLayer;
using CRMUtilities;

namespace BusinessLayer
{
    // Process/Send Order confirmation to Agent
    public class OrderAgentSms : ISMSType
    {
        public void Process(long tenantId, TenantSmsType smsType, DateTime currentIstDateTime,
            string smsRequestLogFile, string smsResponseLogFile)
        {
            if (smsType == null)
            {
                return;
            }

            string guid = Guid.NewGuid().ToString();
            Business.LogError("OrderAgentSms", $"TenantId: {tenantId} | SmsType: {smsType.TypeName} | Guid: {guid}");

            string messageTextFormat = smsType.MessageText;

            if (String.IsNullOrEmpty(messageTextFormat))
            {
                Business.LogError("OrderAgentSms", $"{guid} : Invalid / Empty Message Text Format");
                return;
            }

            do
            {
                // get list of orders that are pending for SMS
                IEnumerable<long> orders = DBLayer.GetOrderForAgentSMS(1, tenantId, -1);
                if (orders == null || orders.Count() == 0)
                {
                    break;
                }

                long orderId = orders.First();

                Business.LogError("OrderAgentSms", $"{guid} : Preparing to send SMS for Order Id {orderId}", ">");

                // retrieve order Record
                Order orderRec = DBLayer.GetOrder(orderId);

                if (String.IsNullOrEmpty(orderRec.Phone))
                {
                    Business.LogError("OrderAgentSms", $"{guid} : Invalid Agent Phone for Order Id {orderId}", ">");
                    DBLayer.MarkAgentSMSSentInOrder(orderId);
                    return;
                }

                string actualSMSText = messageTextFormat.Replace("{OrderNumber}", orderRec.Id.ToString());
                actualSMSText = actualSMSText.Replace("{OrderDate}", orderRec.OrderDate.Format());
                actualSMSText = actualSMSText.Replace("{OrderTotal}", orderRec.TotalAmount.Format());
                actualSMSText = actualSMSText.Replace("{CustomerName}", orderRec.CustomerName);
                actualSMSText = actualSMSText.Replace("{CustomerCode}", orderRec.CustomerCode);
                List<string> phones = new List<string> { orderRec.Phone };
                long smsLogId = 0;
                bool status = Business.SendSMS(tenantId, phones, actualSMSText, currentIstDateTime, smsType.TypeName, 
                    "Auto",
                    Utils.SiteConfigData.SMSTemplate,
                    out smsLogId);
                Business.LogError("OrderAgentSms", $"{guid} : {status}; SMSLogId {smsLogId}", ">");

                // at the end unlock Order + mark it as processed for SMs as well.
                DBLayer.MarkAgentSMSSentInOrder(orderId);
            } while (true);
        }
    }
}
