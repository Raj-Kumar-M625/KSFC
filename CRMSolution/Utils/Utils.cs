using Amazon.S3;
using DomainEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CRMUtilities
{
    public static class Utils
    {
        private static Lazy<IAmazonS3> _s3Client;
        public static IAmazonS3 S3Client => _s3Client.Value;

        //private static Lazy<FileImageStore> _fileImageStore;
        //public static FileImageStore FileImageStore => _fileImageStore.Value;

        //private static Lazy<S3ImageStore> _s3ImageStore;
        //public static S3ImageStore S3ImageStore => _s3ImageStore.Value;


        static Utils()
        {
            _s3Client = new Lazy<IAmazonS3>(() =>
                new AmazonS3Client(Utils.S3AccessKeyId, Utils.S3SecretAccessKey, Amazon.RegionEndpoint.USEast1));

            //_fileImageStore = new Lazy<FileImageStore>(() => new FileImageStore());

            //_s3ImageStore = new Lazy<S3ImageStore>(() => new S3ImageStore());
        }

        public static string TruncateString(string inputString, int length)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                return "";
            }

            int currentLength = inputString.Length;
            if (currentLength <= length)
            {
                return inputString.Trim();
            }

            return inputString.Substring(0, length).Trim();
        }

        public static string GetSnipForLog(string logText)
        {
            if (String.IsNullOrEmpty(logText))
            {
                return "";
            }
            return logText.Substring(0, logText.Length > 150 ? 150 : logText.Length);
        }

        public static bool ParseBoolString(string inputValue)
        {
            bool returnValue = false;

            if (Boolean.TryParse(inputValue, out returnValue))
            {
                return returnValue;
            }

            return false;
        }

        public static string GoogleMapApiKey => Utils.SiteConfigData.GoogleMapAPIKey;

        public static string SmsApiKey => Utils.SiteConfigData.SMSApiKey;
        public static string SmsSender => Utils.SiteConfigData.SMSSender;
        public static string SmsRecipient => Utils.SiteConfigData.SMSRecipient;
        //public static string SMSTemplate(string templateName) => ConfigurationManager.AppSettings[templateName];

        public static bool IsSMSTestMode() => Utils.SiteConfigData.SMSTestMode;

        /// <summary>
        /// Input date is in dd/mm/yyyy format - convert it to dateTime
        /// </summary>
        /// <param name="inputDateAsString"></param>
        /// <returns></returns>
        public static DateTime ConvertStringToDate(string inputDateAsString)
        {
            DateTime dt;
            bool status = false;
            status = DateTime.TryParse(inputDateAsString,
                            System.Globalization.CultureInfo.CreateSpecificCulture("en-GB"),
                            System.Globalization.DateTimeStyles.None,
                            out dt
                );

            if (!status)
            {
                dt = DateTime.MinValue;
            }

            return dt;
        }

        public static int ConvertStringToInt(string inputString, int defaultValue)
        {
            int v;
            bool status = false;
            status = Int32.TryParse(inputString, out v);

            if (!status)
            {
                v = defaultValue;
            }

            return v;
        }

        /// <summary>
        /// input date is in Monday, May 6, 2019 5:28:50 PM
        /// [ Output as DateTime.UtcNow.ToString("F") ]
        /// </summary>
        /// <param name="inputDateAsString"></param>
        /// <returns></returns>
        public static DateTime ConvertLongStringToDate(string inputDateAsString)
        {
            DateTime dt;
            bool status = false;
            status = DateTime.TryParse(inputDateAsString, out dt);

            if (!status)
            {
                dt = DateTime.MinValue;
            }

            return dt;
        }

        public static int FileUploadErrorStopLimit()
        {
            int configValue = Utils.SiteConfigData.FileUploadErrorStopLimit;
            if (configValue <= 0)
            {
                configValue = 10;
            }
            return configValue;
        }

        public static char[] FileUploadCSVSeparator()
        {
            string configValue = Utils.SiteConfigData.FileUploadCSVSeparator;

            if (string.IsNullOrEmpty(configValue) == false)
            {
                return configValue.ToArray();
            }
            else
            {
                return new char[] { ',' };
            }
        }

        public static char[] FileUploadTrimChars()
        {
            string configValue = Utils.SiteConfigData.FileUploadTrimChars;

            if (string.IsNullOrEmpty(configValue) == false)
            {
                // add space also
                configValue = configValue + ' ';
                return configValue.ToArray();
            }
            else
            {
                return new char[] { ' ' };
            }
        }

        public static int GetTopItemCount()
        {
            int topVal = Utils.SiteConfigData.MaxTopItemCountForDashboard;
            if (topVal <= 0)
            {
                topVal = 5;
            }
            return topVal;
        }

        public static int MaxSMSTextSize()
        {
            int size = Utils.SiteConfigData.MaxSMSTextSize;
            if (size <= 0)
            {
                size = 150;
            }
            return size;
        }

        /// <summary>
        /// Get the number of months in two dates
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int MonthsInDates(DateTime startDate, DateTime endDate)
        {
            int totalMonths = 0;

            if (startDate == DateTime.MinValue || endDate == DateTime.MinValue)
            {
                return -1;
            }

            // start date - set it to 1st of the month
            startDate = new DateTime(startDate.Year, startDate.Month, 1);

            // end date - set it to first of next month
            endDate = endDate.AddMonths(1);
            endDate = new DateTime(endDate.Year, endDate.Month, 1);

            while (startDate < endDate)
            {
                totalMonths++;
                startDate = startDate.AddMonths(1);
            }
            return totalMonths;
        }

        public static int LifespanInMinutesForOnFlySuperAdminUser()
        {
            int lifespan = Utils.SiteConfigData.LifespanInMinutesForOnFlySuperAdminUser;

            if (lifespan <= 0)
            {
                lifespan = 240;
            }
            return lifespan;
        }

        public static string CrmTrim(this string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                return "";
            }

            return inputString.Trim();
        }

        public static string GetConfigValue(string configName, string defaultValue)
        {
            return ConfigurationManager.AppSettings[configName] ?? defaultValue;
        }

        public static SiteConfigData SiteConfigData { get; set; }
        public static ICollection<GlobalConfigData> GlobalConfiguration { get; set; }
        public static DBServer DatabaseServerConfiguration { get; set; }
        public static string DBConnectionString { get; set; }
        public static string EFConnectionString { get; set; }

        // configurable values in UrlResolve - Global Configuration
        public static string S3AccessKeyId => GlobalConfiguration
                    .Where(x => x.ConfigKey.Equals("S3AccessKeyId", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.ConfigValue ?? "";

        public static string S3SecretAccessKey => GlobalConfiguration
                    .Where(x => x.ConfigKey.Equals("S3SecretAccessKey", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.ConfigValue ?? "";

        public static string S3DebugUploadBucket => GlobalConfiguration
                    .Where(x => x.ConfigKey.Equals("S3DebugUploadBucket", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.ConfigValue ?? "";

        private static string KeepImagePostDataOnServer => GlobalConfiguration
                    .Where(x => x.ConfigKey.Equals("KeepImagePostDataOnServer", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.ConfigValue ?? "False";

        public static string SiteAlertLogApi => GlobalConfiguration
                    .Where(x => x.ConfigKey.Equals("AlertAPI", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.ConfigValue ?? "";

        public static string ActivityPages => GlobalConfiguration
                    .Where(x => x.ConfigKey.Equals("ActivityPages", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.ConfigValue ?? "";

        public static int AutoReleaseLockTimeInMinutes
        {
            get
            {
                string s = GlobalConfiguration
                    .Where(x => x.ConfigKey.Equals("AutoReleaseLockTimeInMinutes", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.ConfigValue ?? "15";

                int v = ConvertStringToInt(s, 15);

                if (v <= 0)
                {
                    v = 15;
                }

                return v;
            }
        }

        public static bool KeepImagePostDataFile
        {
            get
            {
                bool configValue = false;
                bool status = Boolean.TryParse(KeepImagePostDataOnServer, out configValue);
                if (status == false)
                {
                    configValue = false;
                }
                return configValue;
            }
        }

        public static double DownloadLinkTimeoutInMinutes
        {
            get
            {
                string timeOutAsString = GlobalConfiguration
                    .Where(x => x.ConfigKey.Equals("DownloadLinkTimeoutInMinutes", StringComparison.OrdinalIgnoreCase))
                    .FirstOrDefault()?.ConfigValue ?? "";

                double timeOut = 0;
                double.TryParse(timeOutAsString, out timeOut);

                if (timeOut <= 0)
                {
                    timeOut = 15.0;
                }

                return timeOut;
            }
        }

        public static string PostDataFileNameKey => "PostDataFileNameKey";
        public static string PostDataFileSizeKey => "PostDataFileSizeKey";

        public static void CopyProperties(object source, object target)
        {
            PropertyInfo[] targetPropertyInfo = target.GetType().GetProperties();
            PropertyInfo[] sourcePropertyInfo = source.GetType().GetProperties();

            for (int i = 0; i < targetPropertyInfo.Length; i++)
            {
                // check if property exist in source
                if (targetPropertyInfo[i].CanWrite)
                {
                    PropertyInfo spi = sourcePropertyInfo.Where(x => x.Name == targetPropertyInfo[i].Name).FirstOrDefault();

                    if (spi != null && spi.CanRead &&
                        spi.PropertyType.FullName.Equals(targetPropertyInfo[i].PropertyType.FullName,
                                                        StringComparison.OrdinalIgnoreCase)
                        )
                    {
                        targetPropertyInfo[i].SetValue(target, spi.GetValue(source));
                    }
                }
            }
        }
    }
}
