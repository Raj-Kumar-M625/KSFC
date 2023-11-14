using BusinessLayer;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace TestConApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Association();
            //string response = SmsTestAtTime();
            //Console.WriteLine(response);

            //double sec = GetUnixTime();
            //Console.WriteLine(sec);

            //SmallBulkSmsTest();
            //LargeBulkSmsTest(1000);
            GetProductsTest();
        }

        private static void GetProductsTest()
        {
            //var resultSet = Business.GetDownloadProductsForArea("304", DateTime.Now);
            var resultSet2 = DatabaseLayer.DBLayer.GetDownloadProductsForArea("304");
            Console.WriteLine(resultSet2.Count());
        }

        private static string LargeBulkSmsTest(int numberOfSms)
        {
            string template = ConfigurationManager.AppSettings["SMSTemplate"];
            using (WebClient wc = new WebClient())
            {
                string url = "https://api.textlocal.in/bulk_json";

                SMSMessages msgs = new SMSMessages()
                {
                    messages = new List<SingleSms>(),
                    sender = ConfigurationManager.AppSettings["SMSSender"],
                    test = true
                };


                string phoneNumber = "8277217809";

                for (int i = 0; i < numberOfSms; i++)
                {
                    //phoneNumber += i;
                    string message = $"SMS Testing {phoneNumber} unscheduled at 10:22pm dated {DateTime.Now.ToLongDateString()}";
                    msgs.messages.Add(new SingleSms()
                    {
                        number = phoneNumber,
                        text = string.Format(template, message)
                    });
                }

                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(SMSMessages));
                string postingData = "";
                using (MemoryStream ms = new MemoryStream())
                {
                    js.WriteObject(ms, msgs);
                    ms.Position = 0;
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        postingData = sr.ReadToEnd();
                        sr.Close();
                        ms.Close();
                    }
                }

                NameValueCollection nvc = new NameValueCollection();
                nvc["apikey"] = ConfigurationManager.AppSettings["SMSApiKey"];
                nvc["data"] = postingData;

                Console.WriteLine(postingData);

                Stopwatch sw = new Stopwatch();
                sw.Reset();
                sw.Start();
                byte[] responseBytes = wc.UploadValues(url, "POST", nvc);
                sw.Stop();
                

                Console.WriteLine("================");
                string responseString = System.Text.Encoding.Default.GetString(responseBytes);
                Console.WriteLine(responseString);

                Console.WriteLine($"Posting Data Length = {postingData.Length}");
                Console.WriteLine($"Response Length = {responseString.Length}");
                Console.WriteLine($"Total Elapsed Time {sw.Elapsed.TotalMilliseconds} ms");
                return "";
            }
        }

        private static string SmallBulkSmsTest()
        {
            string template = ConfigurationManager.AppSettings["SMSTemplate"];

            using (WebClient wc = new WebClient())
            {
                string url = "https://api.textlocal.in/bulk_json";

                SMSMessages msgs = new SMSMessages()
                {
                    messages = new List<SingleSms>(),
                    sender = ConfigurationManager.AppSettings["SMSSender"],
                    test = true
                };

                string phoneNumber = "8277217809";
                string message = $"SMS Testing {phoneNumber} unscheduled at 10:05pm dated {DateTime.Now.ToLongDateString()}";
                msgs.messages.Add(new SingleSms()
                {
                    number = phoneNumber,
                    text = string.Format(template, message)
                });

                phoneNumber = "7447913189";
                message = $"SMS Testing {phoneNumber} unscheduled at 10:05pm dated {DateTime.Now.ToLongDateString()}";
                msgs.messages.Add(new SingleSms()
                {
                    number = phoneNumber,
                    text = message // string.Format(template, message)
                });

                DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(SMSMessages));
                string postingData = "";
                using (MemoryStream ms = new MemoryStream())
                {
                    js.WriteObject(ms, msgs);
                    ms.Position = 0;
                    using (StreamReader sr = new StreamReader(ms))
                    {
                        postingData = sr.ReadToEnd();
                        sr.Close();
                        ms.Close();
                    }
                }

                //postingData = postingData.Replace("\\u000d", "\u000c");

                NameValueCollection nvc = new NameValueCollection();
                nvc["apikey"] = ConfigurationManager.AppSettings["SMSApiKey"];
                nvc["data"] = postingData;

                Console.WriteLine(postingData);

                byte[] responseBytes = wc.UploadValues(url, "POST", nvc);

                Console.WriteLine("================");
                Console.WriteLine(System.Text.Encoding.Default.GetString(responseBytes));
                return "";
            }
        }

        private static double GetUnixTime(DateTime ed)
        {
            Console.WriteLine(DateTime.Now);
            Console.WriteLine(DateTime.UtcNow);
            DateTime sd = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            //DateTime ed = new DateTime(2018, 7, 7, 13, 30, 00, DateTimeKind.Local);

            ed = TimeZoneInfo.ConvertTimeToUtc(ed);

            TimeSpan ts = ed.Subtract(sd);
            return ts.TotalSeconds;
        }

        private static void SmsTest()
        {
            DateTime dt = DateTime.Now;
            IEnumerable<string> staffCodes = Business.GetStaffCodesForSms(1, dt, "SMS_GetStartDayStaffCode");

            Console.WriteLine(staffCodes.Count());
        }

        private static string SmsTestAtTime()
        {
            using (WebClient wc = new WebClient())
            {
                string url = "https://api.textlocal.in/send/";
                wc.QueryString.Clear();

                wc.QueryString.Add("apikey", ConfigurationManager.AppSettings["SMSApiKey"]);
                //wc.QueryString.Add("numbers", "9880182554,8978536641,9591064967,8277217809,8762313182");
                wc.QueryString.Add("numbers", "8277217809,9448913182");
                wc.QueryString.Add("sender", ConfigurationManager.AppSettings["SMSSender"]);
                double unixTime = GetUnixTime(new DateTime(2018, 07, 07, 14, 30, 00, DateTimeKind.Local));
                Console.WriteLine(unixTime);
                //wc.QueryString.Add("schedule_time", $"{unixTime}");

                string message = $"SMS Testing unscheduled at 2:34pm dated {DateTime.Now.ToLongDateString()}";
                string template = ConfigurationManager.AppSettings["SMSTemplate"];
                template = template?.Replace("\\r", "\r") ?? "";
                message = string.Format(template, message);

                string encodedMessage = WebUtility.UrlEncode(message);
                wc.QueryString.Add("message", encodedMessage);
                //wc.QueryString.Add("test", "true");
                return wc.DownloadString(url);
            }
        }

        private static void PerformanceTest()
        {
            Console.WriteLine($"Starting...{DateTime.Now.ToString("HH:mm:ss.fff")}");
            SearchCriteria searchCriteria = new SearchCriteria()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,
                ApplyDataStatusFilter = false,
                ApplyAmountFilter = false,
                ApplyDateFilter = true,
                DateFrom = new DateTime(2018, 02, 27),
                DateTo = new DateTime(2018, 03, 05),
                IsSuperAdmin = true,
                CurrentUserStaffCode = "SuperAdmin",
                ApplyActivityTypeFilter = false,
                ApplyClientTypeFilter = false,
                ApplyEmployeeCodeFilter = false,
                ApplyEmployeeNameFilter = false
            };

            Stopwatch sw = new Stopwatch();
            sw.Start();
            Business.GetActivityByTypeReportDataSet(searchCriteria);
            Console.WriteLine("---------------");
            Console.WriteLine($"Business.GetActivityByTypeReportDataSet took {sw.ElapsedMilliseconds} ms");
            sw.Stop();
            Console.WriteLine($"Stopping...{DateTime.Now.ToString("HH:mm:ss.fff")}");
        }

        private static void Association()
        {
            StaffFilter searchCriteria = new StaffFilter()
            {
                ApplyEmployeeCodeFilter = false,
                ApplyNameFilter = false,
                ApplyPhoneFilter = false,
                ApplyGradeFilter = false,
                ApplyStatusFilter = false,
                IsSuperAdmin = true,
                ApplyAssociationFilter = true,
                Association = true,
            };

            Business.GetSalesPersonData(searchCriteria);
            Console.Read();
        }

        public static DateTime ConvertUtcTimeToIst(DateTime utcDateTime)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }
    }
}
