using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using DatabaseLayer;
using System.Globalization;
using CRMUtilities;

namespace BusinessLayer
{
    // Process/Send Order confirmation to Customer
    public class OrderCustomerSms : ISMSType
    {
        public void Process(long tenantId, TenantSmsType smsType, DateTime currentIstDateTime,
            string smsRequestLogFile, string smsResponseLogFile)
        {
            if (smsType == null)
            {
                return;
            }

            string guid = Guid.NewGuid().ToString();
            Business.LogError("OrderCustomerSms", $"TenantId: {tenantId} | SmsType: {smsType.TypeName} | Guid: {guid}");

            string messageTextFormat = smsType.MessageText;

            if (String.IsNullOrEmpty(messageTextFormat))
            {
                Business.LogError("OrderCustomerSms", $"{guid} : Invalid / Empty Message Text Format");
                return;
            }

            do
            {
                // get list of orders that are pending for SMS
                IEnumerable<long> orders = DBLayer.GetOrderForCustomerSMS(1, tenantId, -1);
                if (orders == null || orders.Count() == 0)
                {
                    break;
                }

                long orderId = orders.First();

                Business.LogError($"{nameof(OrderCustomerSms)}", $"{guid} : Preparing to send SMS for Order Id {orderId}", ">");

                // retrieve order Record
                Order orderRec = DBLayer.GetOrder(orderId);

                if (String.IsNullOrEmpty(orderRec.CustomerPhone))
                {
                    Business.LogError("OrderCustomerSms", $"{guid} : Invalid Customer Phone for Order Id {orderId}", ">");
                    DBLayer.MarkCustomerSMSSentInOrder(orderId);
                    return;
                }

                string actualSMSText = messageTextFormat.Replace("{OrderNumber}", orderRec.Id.ToString());
                actualSMSText = actualSMSText.Replace("{OrderDate}", orderRec.OrderDate.Format());
                actualSMSText = actualSMSText.Replace("{OrderTotal}", orderRec.TotalAmount.Format());
                List<string> phones = new List<string> { orderRec.CustomerPhone };
                long smsLogId = 0;
                bool status = Business.SendSMS(tenantId, phones, actualSMSText, currentIstDateTime, smsType.TypeName, "Auto",
                    Utils.SiteConfigData.OrderSMSToCustomerTemplate,
                    out smsLogId);
                Business.LogError("OrderCustomerSms", $"{guid} : {status}; SMSLogId {smsLogId}", ">");

                // at the end unlock Order + mark it as processed for SMs as well.
                DBLayer.MarkCustomerSMSSentInOrder(orderId);
            } while (true);
        }
    }
}
