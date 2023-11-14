using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainEntities;
using DatabaseLayer;
using CRMUtilities;
using System.Configuration;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Collections.Specialized;
using CrmAlert;

namespace BusinessLayer
{
    // Process/Send End Day Sms to Agent as Bulk SMS
    public class EndDayAgentSmsAsBulk : ISMSType
    {
        public void Process(long tenantId, TenantSmsType smsType, DateTime currentIstDateTime,
            string smsRequestLogFile, string smsResponseLogFile)
        {
            if (smsType == null)
            {
                return;
            }

            string guid = Guid.NewGuid().ToString();
            Business.LogError("EndDayAgentSms", $"TenantId: {tenantId} | SmsType: {smsType.TypeName} | Date: {currentIstDateTime.Format()} | Guid: {guid}");

            if (Business.IsTimeForSMS(tenantId, smsType, currentIstDateTime) == false)
            {
                Business.LogError("EndDayAgentSms", $"{guid} : It is not time for SMS yet.", ">");
                return;
            }

            string messageTextFormat = smsType.MessageText;
            if (String.IsNullOrEmpty(messageTextFormat))
            {
                Business.LogError("EndDayAgentSms", $"{guid} : Invalid / Empty Message Text Format");
                return;
            }

            Business.LogError("EndDayAgentSms", $"{guid} : Preparing to send SMS;", ">");

            ICollection<SmsDataEx> smsData = Business.GetSmsDetailData(tenantId, currentIstDateTime)
                .Where(x => x.IsInFieldToday && String.IsNullOrEmpty(x.Phone) == false)
                .ToList();

            if (smsData.Count == 0)
            {
                Business.LogError("EndDayAgentSms", $"{guid} : No SMS to be sent", ">");
                return;
            }

            ICollection<DashboardDataSet> dashboardDataSet = Business.DashboardData(currentIstDateTime, currentIstDateTime);
            ICollection<EmployeeRecord> Users = Business.Users();

            // hours
            SearchCriteria sc = new SearchCriteria()
            {
                ApplyDateFilter = true,
                DateFrom = currentIstDateTime,
                DateTo = currentIstDateTime,
                IsSuperAdmin = true,
                CurrentUserStaffCode = ""
            };
            ICollection<AttendanceReportData> attendanceDataSet = Business.GetAttendanceReportDataSet(sc);
            var hoursData = attendanceDataSet
                .Where(x => x.StartTime != DateTime.MinValue && x.EndTime != DateTime.MinValue)
            .GroupBy(x => x.TenantEmployeeId)
            .Select(x => new
            {
                TenantEmployeeId = x.Key,
                TSpans = x.Select(y => y.EndTime.Subtract(y.StartTime)).ToArray()
            }).ToList();


            SMSMessages msgs = new SMSMessages()
            {
                messages = new List<SmsText>(),
                sender = Utils.SiteConfigData.SMSSender
                //ConfigurationManager.AppSettings["SMSSender"]
            };

            // Process for Each
            foreach (var sd in smsData)
            {
                long empId = Users.Where(x => x.EmployeeCode == sd.StaffCode).FirstOrDefault()?.EmployeeId ?? 0;
                DashboardDataSet dashboardData = dashboardDataSet.Where(x => x.TenantEmployeeId == empId).FirstOrDefault();

                var hd = hoursData.Where(x => x.TenantEmployeeId == empId).FirstOrDefault();
                TimeSpan totalTimeSpan = TimeSpan.Zero;
                if (hd != null)
                {
                    foreach (var t in hd.TSpans)
                    {
                        totalTimeSpan = totalTimeSpan.Add(t);
                    }
                }

                string actualSMSText = messageTextFormat.Replace("{OrderTotal}", sd.TotalOrderAmount.Format());
                actualSMSText = actualSMSText.Replace("{PaymentTotal}", sd.TotalPaymentAmount.Format());
                actualSMSText = actualSMSText.Replace("{ExpenseTotal}", sd.TotalExpenseAmount.Format());
                actualSMSText = actualSMSText.Replace("{WorkingHrs}", totalTimeSpan.Format());
                actualSMSText = actualSMSText.Replace("{ActivityCount}", (dashboardData?.ActivityCount ?? 0).ToString());
                actualSMSText = actualSMSText.Replace("{TravelKm}", decimal.Round(((dashboardData?.TotalDistanceInMeters ?? 0) / 1000), 1).ToString());
                actualSMSText = actualSMSText.Replace("{SmsDate}", currentIstDateTime.Format());

                // Add each message to the list
                msgs.messages.Add(new SmsText()
                {
                    number = sd.Phone,
                    text = PutMessageInTemplate(actualSMSText, Utils.SiteConfigData.SMSTemplate)
                });
            }

            long smsLogId = 0;
            Business.SendSMSAsBulk(tenantId, currentIstDateTime, "", smsType.TypeName, msgs, "Auto",
                smsRequestLogFile, smsResponseLogFile,
                out smsLogId);
        }

        private string PutMessageInTemplate(string inputMessage, string template)
        {
            inputMessage = inputMessage.Replace("<NL>", "\\r");
            return string.Format(template, inputMessage);
        }
    }
}
