using KAR.KSFC.Components.Common.Dto.SMS;

using Newtonsoft.Json;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KAR.KSFC.Components.Common.Utilities.Templates.Email
{
    public static class EmailTemplate
    {
        public static string GetNewPassword(string password, string EmployeeName)
        {
            var listTemplate = GetEmailTemplates();
            string data = listTemplate.Where(x => x.Key == "ForgotPassword").FirstOrDefault().Value;
            return data.Replace("_user", EmployeeName).Replace("_newPassword", password);
        }

        public static string GetEnquirySubmission(int refnumber) {
            var listTemplate = GetEmailTemplates();
            string data = listTemplate.Where(x => x.Key == "EnquirySubmission").FirstOrDefault().Value;
            return data.Replace("_ref", refnumber.ToString());
        }

        private static List<SMSTemplateDTO> GetEmailTemplates()
        {
            var jsonSerializerSettings = new JsonSerializerSettings();
            jsonSerializerSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            var data = JsonConvert.DeserializeObject<List<SMSTemplateDTO>>(File.ReadAllText("EmailTemplate.json"), jsonSerializerSettings);
            return data;
        }
    }
}
