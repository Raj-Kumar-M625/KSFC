using EDCS_TG.API.Data.Models;
using EDCS_TG.API.Data.Repository.Interfaces;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;


namespace EDCS_TG.API.Helpers
{
    public class SmsService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _repository;

        public SmsService(IConfiguration configuration, IUnitOfWork repository)
        {
            _configuration = configuration;
            _repository = repository;
        }

        public async Task<string> SendOtp(string mobileNo, string messageText, string templateId, string OTP)
        {
            var username = _configuration.GetValue<string>("SmsSettings:UserName");
            var password = _configuration.GetValue<string>("SmsSettings:Password");
            var senderid = _configuration.GetValue<string>("SmsSettings:SenderId");
            var secureKey = _configuration.GetValue<string>("SmsSettings:SecureKey");
            var isEnabled = Boolean.Parse(_configuration.GetValue<string>("SmsSettings:IsEnabled"));
            var smsRequestURL = _configuration.GetValue<string>("SmsSettings:URL");
            if (isEnabled)
            {
                Stream dataStream;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; //forcing .Net framework to use TLSv1.2
                var request = (HttpWebRequest)WebRequest.Create(smsRequestURL);
                request.ProtocolVersion = HttpVersion.Version10;
                request.KeepAlive = false;
                request.ServicePoint.ConnectionLimit = 1;
                request.UserAgent = "Mozilla/4.0 (compatible; MSIE 5.0; Windows 98; DigExt)";
                request.Method = "POST";
                ServicePointManager.ServerCertificateValidationCallback += delegate { return true; };
                var encryptedPassword = EncryptedPassword(password);
                var message = messageText!.Replace("{OTP}", OTP);
                var key = HashGenerator(username.Trim(), senderid.Trim(), message.Trim(), secureKey.Trim());
                var smsservicetype = "otpmsg"; //For OTP message.
                var query = "username=" + HttpUtility.UrlEncode(username.Trim()) +
                            "&password=" + HttpUtility.UrlEncode(encryptedPassword) +
                            "&smsservicetype=" + HttpUtility.UrlEncode(smsservicetype) +
                            "&content=" + HttpUtility.UrlEncode(message.Trim()) +
                            "&mobileno=" + HttpUtility.UrlEncode(mobileNo) +
                            "&senderid=" + HttpUtility.UrlEncode(senderid.Trim()) +
                            "&key=" + HttpUtility.UrlEncode(key.Trim()) +
                            "&templateid=" + HttpUtility.UrlEncode(templateId.Trim());
                var byteArray = Encoding.ASCII.GetBytes(query);
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = byteArray.Length;
                dataStream = request.GetRequestStream();
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
                var response = request.GetResponse();
                var Status = ((HttpWebResponse)response).StatusDescription;
                dataStream = response.GetResponseStream();
                StreamReader reader = new(dataStream);
                var responseFromServer = reader.ReadToEnd();
                reader.Close();
                dataStream.Close();
                response.Close();
                string[] result = responseFromServer.Split(',');

                if (result.First() == "402")
                {

                    var smsLog = new SMSLog()
                    {
                        SmsId = result[1],
                        SmsDateTime = DateTime.Now,
                        SmsText = message,
                        ApiStatus = Status,
                        HasCompleted = true,
                        HasFailed = false,
                        ApiResponse = responseFromServer
                    };
                    var newSmsLog = await _repository.SMSLog.Create(smsLog);

                    return responseFromServer;
                }
                else
                {
                    var smsLog = new SMSLog()
                    {
                        SmsId = result[1],
                        SmsDateTime = DateTime.Now,
                        SmsText = message,
                        ApiStatus = Status,
                        HasCompleted = false,
                        HasFailed = true,
                        ApiResponse = responseFromServer
                    };
                    var newSmsLog = await _repository.SMSLog.Create(smsLog);
                    return responseFromServer;
                }
            }
            return "";
        }

        public string EncryptedPassword(string password)
        {
            var encPwd = Encoding.UTF8.GetBytes(password);
            var sha1 = HashAlgorithm.Create("SHA1");
            var pp = sha1.ComputeHash(encPwd);
            var sb = new StringBuilder();
            foreach (var b in pp) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        public string HashGenerator(string Username, string sender_id, string message, string secure_key)
        {
            var sb = new StringBuilder();
            sb.Append(Username).Append(sender_id).Append(message).Append(secure_key);
            var genkey = Encoding.UTF8.GetBytes(sb.ToString());
            var sha1 = HashAlgorithm.Create("SHA512");
            var sec_key = sha1.ComputeHash(genkey);
            var sb1 = new StringBuilder();
            for (var i = 0; i < sec_key.Length; i++) sb1.Append(sec_key[i].ToString("x2"));
            return sb1.ToString();
        }
    }
}
