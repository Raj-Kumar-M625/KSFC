using CRMUtilities;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;

namespace CrmAlert
{
    public class SmsAlert : IAlert
    {
        public string Send(string message, IEnumerable<string> phoneNumbers)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (phoneNumbers == null || phoneNumbers.Count() == 0)
            {
                throw new ArgumentNullException(nameof(phoneNumbers));
            }

            string smsRecipients = String.Join(",", phoneNumbers.ToArray());

            using (WebClient wc = new WebClient())
            {
                string url = "https://api.textlocal.in/send/";
                wc.QueryString.Clear();

                wc.QueryString.Add("apikey", Utils.SmsApiKey);
                wc.QueryString.Add("numbers", smsRecipients);
                wc.QueryString.Add("sender", Utils.SmsSender);
                string encodedMessage = WebUtility.UrlEncode(message);
                wc.QueryString.Add("message", encodedMessage);

                if (Utils.IsSMSTestMode())
                {
                    wc.QueryString.Add("test", "true");
                }

                return wc.DownloadString(url);
            }
        }

        public bool ParseSendResponse(string responseReceived)
        {
            if (String.IsNullOrEmpty(responseReceived))
            {
                return false;
            }

            return (responseReceived.EndsWith("\"status\":\"success\"}"));
        }

        //public string PutMessageInTemplate(string inputMessage, string templateName)
        //{
        //    string template = Utils.SMSTemplate(templateName);
        //    template = template?.Replace("\\r", "\r") ?? "";
        //    inputMessage = inputMessage.Replace("<NL>", "\r");
        //    return string.Format(template, inputMessage);
        //}

        public string PutMessageInTemplate(string inputMessage, string template)
        {
            template = template?.Replace("\\r", "\r") ?? "";
            inputMessage = inputMessage.Replace("<NL>", "\r");
            return string.Format(template, inputMessage);
        }

        public SmsResponseFormat SendBulk(SMSMessages smsMessages, string smsRequestLogFile, string smsResponseLogFile)
        {
            if (smsMessages == null || smsMessages.messages == null
                || smsMessages.messages.Count == 0)
            {
                throw new ArgumentException(nameof(smsMessages));
            }

            // set test mode if configured in web.config
            smsMessages.test = Utils.IsSMSTestMode();

            // Create JSON
            DataContractJsonSerializer js = new DataContractJsonSerializer(typeof(SMSMessages));
            string postingDataJson = "";
            using (MemoryStream ms = new MemoryStream())
            {
                js.WriteObject(ms, smsMessages);
                ms.Position = 0;
                using (StreamReader sr = new StreamReader(ms))
                {
                    postingDataJson = sr.ReadToEnd();
                    sr.Close();
                    ms.Close();
                }
            }

            // write request to a file
            if (!String.IsNullOrEmpty(smsRequestLogFile))
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(smsRequestLogFile))
                    {
                        sw.Write(postingDataJson);
                        sw.Close();
                    }
                }
                catch(Exception)
                {
                    
                }
            }

            NameValueCollection nvc = new NameValueCollection();
            nvc["apikey"] = Utils.SmsApiKey;
            nvc["data"] = postingDataJson;

            string url = "https://api.textlocal.in/bulk_json";
            string responseAsString = "";

            using (WebClient wc = new WebClient())
            {
                byte[] responseBytes = wc.UploadValues(url, "POST", nvc);
                responseAsString = System.Text.Encoding.Default.GetString(responseBytes);
            }

            // save complete response in file;
            if (!String.IsNullOrEmpty(smsResponseLogFile))
            {
                try
                {
                    using (StreamWriter sw = new StreamWriter(smsResponseLogFile))
                    {
                        sw.Write(responseAsString);
                        sw.Close();
                    }
                }
                catch (Exception)
                {

                }
            }

            // convert response to object
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(responseAsString)))
            {
                DataContractJsonSerializer js2 = new DataContractJsonSerializer(typeof(SmsResponseFormat));
                SmsResponseFormat smsResponseObject = js2.ReadObject(ms) as SmsResponseFormat;
                ms.Close();
                return smsResponseObject;
            }
        }
    }
}
