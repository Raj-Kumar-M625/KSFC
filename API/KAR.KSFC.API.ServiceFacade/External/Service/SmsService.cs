using KAR.KSFC.API.ServiceFacade.External.Interface;
using KAR.KSFC.Components.Common.Dto;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.SMS;
using KAR.KSFC.Components.Common.Security;
using KAR.KSFC.Components.Common.Utilities;

using Microsoft.Extensions.Configuration;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace KAR.KSFC.API.ServiceFacade.External.Service
{
    public class SmsService : ISmsService
    {
        private readonly IConfiguration _configureure;
        private readonly IHttpClientFactory _clientFactory;
        private readonly List<SMSTemplateDTO> _templateList;
        public SmsService(IConfiguration configure, IHttpClientFactory clientFactory)
        {
            this._configureure = configure;
            this._clientFactory = clientFactory;
            _templateList = GetSMSTemplates();
        }

        private int GenarateOtp()
        {
            Random random = new();
            bool isSmsServiceAvailable = Convert.ToBoolean(_configureure["SMS:IsSmsServiceAvailable"]); //Added by Rahul to Bypass SMS service if not available
            if (!isSmsServiceAvailable)
            {
                int defaultOtp = Convert.ToInt32(_configureure["SMS:DefaultOTP"]);
                return defaultOtp;
            }
            return random.Next(100000, 999999);
        }

        private string CreateSmsMessage(int otp, string process)
        {
            var message = _templateList.Where(x => x.Key.Equals(process)).FirstOrDefault();
            return message.Value.Replace("_otp", otp.ToString());
        }

        public async Task<SmsInfoDTO> SendSMS(string mobileNo, string process)
        {
            try
            {
                SmsInfoDTO smsResponseDTO;
                var otp = GenarateOtp();
                var message = CreateSmsMessage(otp, process);
                bool isSmsServiceAvailable = Convert.ToBoolean(_configureure["SMS:IsSmsServiceAvailable"]);
                if (!isSmsServiceAvailable)
                {
                    return smsResponseDTO = new SmsInfoDTO()
                    {
                        otp = Convert.ToString(otp),
                        Mobile = mobileNo,
                        Status = "InActive"
                    };
                }//Code to bypass unavailable SMS service ends here
                string username = _configureure["SMS:Username"];
                string password = _configureure["SMS:Password"];
                string senderid = _configureure["SMS:Sender_id"];
                string secureKey = _configureure["SMS:API_service_key"];
                string templateId = _configureure["SMS:template_id"];
                string url = _configureure["SMS:Url"];

                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, errs) => true;

                var request = (HttpWebRequest)WebRequest.Create(url);

                request.ProtocolVersion = HttpVersion.Version10;
                request.KeepAlive = false;
                request.ServicePoint.ConnectionLimit = 1;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0;Windows 98; DigExt)";
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";

                var query = new StringBuilder();

                query.Append("username=").Append(HttpUtility.UrlEncode(username));
                query.Append("&password=").Append(HttpUtility.UrlEncode(SMSSecurity.HashData(password, "SHA1")));
                query.Append("&smsservicetype=").Append(HttpUtility.UrlEncode("singlemsg"));
                query.Append("&content=").Append(HttpUtility.UrlEncode(message));
                query.Append("&mobileno=").Append(HttpUtility.UrlEncode(mobileNo));
                query.Append("&senderid=").Append(HttpUtility.UrlEncode(senderid));
                query.Append("&key=").Append(HttpUtility.UrlEncode(
                    SMSSecurity.HashData(username + senderid + message + secureKey, "SHA512")));////"SHA15"-Secure Hash Algorithms, also known as SHA, are a family of cryptographic functions designed to keep data secured. 
                query.Append("&templateid=").Append(HttpUtility.UrlEncode(templateId));


                var bytes = Encoding.ASCII.GetBytes(query.ToString());

                request.ContentLength = bytes.Length;

                using (var rs = request.GetRequestStream())
                {
                    rs.Write(bytes);
                }

                using (var response = await request.GetResponseAsync())
                {
                    using (var dataStream = response.GetResponseStream())
                    {
                        using (var reader = new StreamReader(dataStream))
                        {
                            return smsResponseDTO = new SmsInfoDTO()
                            {
                                otp = Convert.ToString(otp),
                                Mobile = mobileNo,
                                Status = "OK"
                            };
                        }
                    }
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<string> SendSms(SmsDataModel sdm)
        {
            string username = _configureure["SMS:Username"];
            string password = _configureure["SMS:Password"];
            string senderid = _configureure["SMS:Sender_id"];
            string secureKey = _configureure["SMS:API_service_key"];
            string templateid = _configureure["SMS:template_id"];
            string url = _configureure["SMS:Url"];

            bool isSmsServiceAvailable = Convert.ToBoolean(_configureure["SMS:IsSmsServiceAvailable"]);
            if (!isSmsServiceAvailable)
            {
                return "Ok";
            }//Code to bypass unavailable SMS service ends here

            //Latest Generated Secure Key
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;//Transport Layer Security (TLS) 1.2 is the successor to Secure Sockets Layer (SSL) used by endpoint devices and applications to authenticate and encrypt data securely when transferred over a network.
            //forcing .Net framework to use TLSv1.2
            ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, errs) => true;

            var request = (HttpWebRequest)WebRequest.Create(url);

            request.ProtocolVersion = HttpVersion.Version10;
            request.KeepAlive = false;
            request.ServicePoint.ConnectionLimit = 1;
            request.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0;Windows 98; DigExt)";
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            var query = new StringBuilder();

            query.Append("username=").Append(HttpUtility.UrlEncode(username));
            query.Append("&password=").Append(HttpUtility.UrlEncode(SMSSecurity.HashData(password, "SHA1")));
            query.Append("&smsservicetype=").Append(HttpUtility.UrlEncode("singlemsg"));
            query.Append("&content=").Append(HttpUtility.UrlEncode(sdm.Message));
            query.Append("&mobileno=").Append(HttpUtility.UrlEncode(sdm.MobileNumber));
            query.Append("&senderid=").Append(HttpUtility.UrlEncode(senderid));
            query.Append("&key=").Append(HttpUtility.UrlEncode(
                SMSSecurity.HashData(username + senderid + sdm.Message + secureKey, "SHA512")));////"SHA15"-Secure Hash Algorithms, also known as SHA, are a family of cryptographic functions designed to keep data secured. 
            query.Append("&templateid=").Append(HttpUtility.UrlEncode(templateid));
            query.Append("&templateid=").Append(HttpUtility.UrlEncode(templateid));

            var bytes = Encoding.ASCII.GetBytes(query.ToString());

            request.ContentLength = bytes.Length;

            using (var rs = request.GetRequestStream())
            {
                rs.Write(bytes);
            }

            using (var response = request.GetResponse())
            // string Status = ((HttpWebResponse)response).StatusDescription;
            using (var dataStream = response.GetResponseStream())
            using (var reader = new StreamReader(dataStream))
            {

                //return reader.ReadToEnd();
                var match = Regex.Match(reader.ReadToEnd(), "MsgID[ ]*=[ ]*([0-9]*)" + username);
                if (!match.Success)
                {
                    return "Error while sending SMS. Response from server";
                }
                return match.Groups[1].Value.ToString();// new Response { MsgId = match.Groups[1].Value };
            }


        }

        private List<SMSTemplateDTO> GetSMSTemplates()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            var data = JsonConvert.DeserializeObject<List<SMSTemplateDTO>>(File.ReadAllText("SMSTemplate.json"), jsonSerializerSettings);
            return data;
        }
    }
}