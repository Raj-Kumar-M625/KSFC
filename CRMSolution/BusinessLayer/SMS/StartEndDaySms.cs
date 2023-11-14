using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using CRMUtilities;

namespace BusinessLayer
{
    // Process/Send SMS of type Start / End Day
    public class StartEndDaySms : ISMSType
    {
        public void Process(long tenantId, TenantSmsType smsType, DateTime currentIstDateTime,
            string smsRequestLogFile, string smsResponseLogFile)
        {
            if (smsType == null)
            {
                return;
            }

            string guid = Guid.NewGuid().ToString();
            Business.LogError("StartEndDaySms", $"TenantId: {tenantId} | SmsType: {smsType.TypeName} | Guid: {guid}");
            if (Business.IsTimeForSMS(tenantId, smsType, currentIstDateTime) == false)
            {
                Business.LogError("StartEndDaySms", $"{guid} : It is not time for SMS yet.", ">");
                return;
            }

            Business.LogError("StartEndDaySms", $"{guid} : Preparing to send SMS;", ">");

            // now get the staff codes for smsType
            IEnumerable<string> staffCodes = Business.GetStaffCodesForSms(tenantId, currentIstDateTime, smsType.SprocName);

            ICollection<string> phoneNumbers = Business.GetStaffPhoneNumbers(staffCodes);
            if (phoneNumbers.Count == 0)
            {
                Business.LogError("StartEndDaySms", $"{guid} : No Phone numbers.", ">");
                return;
            }

            long smsLogId = 0;
            bool status = Business.SendSMS(tenantId, phoneNumbers, smsType.MessageText, currentIstDateTime, smsType.TypeName, 
                "Auto", 
                Utils.SiteConfigData.SMSTemplate,
                out smsLogId);
            Business.LogError("StartEndDaySms", $"{guid} : {status}; SmsLogId: {smsLogId}");
            
        }
    }
}
