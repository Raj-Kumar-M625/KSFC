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
    // Process/Send End Day Sms to Agent
    public class EndDayAgentSms : ISMSType
    {
        public void Process(long tenantId, TenantSmsType smsType, DateTime currentIstDateTime,
            string smsRequestLogFile, string smsResponseLogFile)
        {
            if (smsType == null)
            {
                return;
            }

            string guid = Guid.NewGuid().ToString();
            Business.LogError("EndDayAgentSms", $"TenantId: {tenantId} | SmsType: {smsType.TypeName} | Guid: {guid}");

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

            ICollection<SmsDataEx> smsData = Business.GetSmsDetailData(tenantId, currentIstDateTime);
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
                TSpans = x.Select(y=> y.EndTime.Subtract(y.StartTime)).ToArray()
            }).ToList();


            // Process for Each
            foreach(var sd in smsData)
            {
                if (sd.IsInFieldToday && String.IsNullOrEmpty(sd.Phone) == false)
                {
                    long empId = Users.Where(x => x.EmployeeCode == sd.StaffCode).FirstOrDefault()?.EmployeeId ?? 0;
                    DashboardDataSet dashboardData = dashboardDataSet.Where(x => x.TenantEmployeeId == empId).FirstOrDefault();

                    var hd = hoursData.Where(x => x.TenantEmployeeId == empId).FirstOrDefault();
                    TimeSpan totalTimeSpan = TimeSpan.Zero;
                    if (hd != null)
                    {
                        foreach(var t in hd.TSpans)
                        {
                            totalTimeSpan = totalTimeSpan.Add(t);
                        }
                    }

                    string actualSMSText = messageTextFormat.Replace("{OrderTotal}", sd.TotalOrderAmount.Format());
                    actualSMSText = actualSMSText.Replace("{PaymentTotal}", sd.TotalPaymentAmount.Format());
                    actualSMSText = actualSMSText.Replace("{ExpenseTotal}", sd.TotalExpenseAmount.Format());
                    actualSMSText = actualSMSText.Replace("{WorkingHrs}", totalTimeSpan.Format());
                    actualSMSText = actualSMSText.Replace("{ActivityCount}", (dashboardData?.ActivityCount ?? 0).ToString());
                    actualSMSText = actualSMSText.Replace("{TravelKm}", decimal.Round(((dashboardData?.TotalDistanceInMeters ?? 0)/1000),1).ToString());
                    actualSMSText = actualSMSText.Replace("{SmsDate}", currentIstDateTime.Format());

                    List<string> phones = new List<string> { sd.Phone };
                    long smsLogId = 0;
                    Business.SendSMS(tenantId, phones, actualSMSText, currentIstDateTime, smsType.TypeName, "Auto",
                        Utils.SiteConfigData.SMSTemplate,
                        out smsLogId);
                }
            }
        }
    }
}
