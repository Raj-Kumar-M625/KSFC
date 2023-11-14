using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Diagnostics;

namespace EpicUpload
{
    public static class Helper
    {
        public const string UploadAPI = "UploadAPI";
        public const string SourceLocation = "SourceLocation";
        public const string BackupLocation = "BackupLocation";
        public const string AlertAPI = "AlertAPI";
        public const string IsCompleteRefresh = "IsCompleteRefresh";

        public static string GetUploadAPI => GetConfigValue(UploadAPI);
        public static string GetSourceLocation => GetConfigValue(SourceLocation);
        public static string GetBackupLocation => GetConfigValue(BackupLocation);
        public static string GetAlertAPI => GetConfigValue(AlertAPI);
        public static string GetIsCompleteRefresh => GetConfigValue(IsCompleteRefresh);


        public static string GetConfigValue(string key)
        {
            //System.Configuration.ConfigurationSettings.AppSettings.AllKeys;
            string [] allKeys = ConfigurationManager.AppSettings.AllKeys;

            if ((allKeys?.Count() ?? 0) == 0)
            {
                return "";
            }

            if (allKeys.Any(x=> x.Equals(key, StringComparison.OrdinalIgnoreCase)) == false)
            {
                return "";
            }

            return ConfigurationManager.AppSettings.Get(key);
        }

        public static bool IsFolderExist(string folderName)
        {
            return Directory.Exists(folderName);
        }

        public static void EnsureFolderExist(string folderName)
        {
            if (IsFolderExist(folderName) == false)
            {
                Directory.CreateDirectory(folderName);
            }
        }

        public static string[] GetFileList(string folderName)
        {
            return Directory.GetFiles(folderName);
        }

        public static async Task<FileUploadResponse> UploadFile(FileUploadRequest fur)
        {
            string apiName = GetUploadAPI;
            FileUploadResponse uploadResponse = null;

            try
            {
                var js = new JavaScriptSerializer();
                js.MaxJsonLength = Int32.MaxValue;
                var serializedData = js.Serialize(fur);

                HttpClient httpClient = new HttpClient();
                httpClient.Timeout = new TimeSpan(0, 10, 0);
                HttpResponseMessage response = await httpClient.PostAsync(apiName, new StringContent(serializedData, Encoding.UTF8, "application/json"));

                if ((response?.IsSuccessStatusCode ?? false) == false)
                {
                    uploadResponse = new FileUploadResponse()
                    {
                        Content = $"An error occured while invoking API {apiName}, {response.ReasonPhrase}",
                        StatusCode = System.Net.HttpStatusCode.BadGateway
                    };

                    return uploadResponse;
                }

                uploadResponse = js.Deserialize<FileUploadResponse>(response.Content.ReadAsStringAsync().Result);
                return uploadResponse;
            }
            catch (Exception ex)
            {
                uploadResponse = new FileUploadResponse()
                {
                    Content = $"An error occured while invoking API {apiName}, {ex.Message}",
                    StatusCode = System.Net.HttpStatusCode.BadGateway
                };

                return uploadResponse;
            }
        }

        public static string ExtractFileNameFromFullPath(string fileNameWithPath)
        {
            if (File.Exists(fileNameWithPath))
            {
                string[] fileNameParts = fileNameWithPath.Split(new char[] { '/', '\\' });
                if ((fileNameParts?.Length ?? 0) > 0)
                {
                    string fileName = fileNameParts[fileNameParts.Length - 1];
                    return fileName;
                }
            }

            return "";
        }

        public static void MoveFile(string sourceFileNameWithPath, string destinationFolder)
        {
            if (File.Exists(sourceFileNameWithPath))
            {
                string fileName = ExtractFileNameFromFullPath(sourceFileNameWithPath);
                string destFileNameWithPath = Path.Combine(destinationFolder, fileName);
                File.Copy(sourceFileNameWithPath, destFileNameWithPath, true);

                File.Delete(sourceFileNameWithPath);
            }
        }

        public static string GetTableNameForFileName(string fileNameWithPath)
        {
            if (File.Exists(fileNameWithPath))
            {
                string fileName = ExtractFileNameFromFullPath(fileNameWithPath);
                return GetConfigValue(fileName.ToLower());
            }

            return "";
        }

        public static byte[] ReadFileBytes(string fileNameWithPath)
        {
            if (File.Exists(fileNameWithPath))
            {
                return File.ReadAllBytes(fileNameWithPath);
            }

            return null;
        }

        public static void LogError(string message, bool createAlert = true)
        {
            WriteEvent(message, EventLogEntryType.Error);

            if (createAlert)
            {
                // also log an alert
                long alertId = LogAlert(message).Result;
            }
        }

        public static void LogInfo(string message) => WriteEvent(message, EventLogEntryType.Information);

        private static string EventSourceName => "EpicUpload"; // AppDomain.CurrentDomain.FriendlyName;

        public static void EnsureEventSourceExist()
        {
            if (EventLog.SourceExists(EventSourceName) == false)
            {
                EventLog.CreateEventSource(EventSourceName, "Application");
            }
        }

        private static void WriteEvent(string message, EventLogEntryType logEntryType)
        {
            // http://www.daveoncsharp.com/2009/08/writing-to-the-windows-event-log-using-csharp/
            //
            using (EventLog eventLog = new EventLog("Application"))
            {
                eventLog.Source = EventSourceName;
                eventLog.WriteEntry(message, logEntryType, 1001, 1);
            }
        }

        // Also log an alert in case of failure
        private static async Task<long> LogAlert(string errorMessage)
        {
            string apiName = GetAlertAPI;

            if (string.IsNullOrEmpty(apiName))
            {
                return -1;
            }

            SiteAlert sa = new SiteAlert()
            {
                SiteId = 0,
                SiteName = EventSourceName,
                ErrorCode = "AutoUpload",
                ErrorDesc = errorMessage,
                UTCAT = DateTime.UtcNow,
            };

            try
            {
                var js = new JavaScriptSerializer();
                var serializedData = js.Serialize(sa);

                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.PostAsync(apiName, new StringContent(serializedData, Encoding.UTF8, "application/json"));

                if ((response?.IsSuccessStatusCode ?? false) == false)
                {
                    return -1;
                }

                long alertId = js.Deserialize<long>(response.Content.ReadAsStringAsync().Result);

                return alertId;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }
    }
}

// Publish
// https://stackoverflow.com/questions/29896769/c-sharp-2013-default-certificate-could-not-be-created-publish-aborting
//
