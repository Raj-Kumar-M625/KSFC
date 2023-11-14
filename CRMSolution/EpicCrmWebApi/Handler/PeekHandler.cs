using BusinessLayer;
using CRMUtilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EpicCrmWebApi
{
    /// <summary>
    /// Handler to look into incoming request - used for debugging;
    /// </summary>
    public class PeekHandler : DelegatingHandler
    {
        // https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/http-message-handlers
        protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool logRequest = false;
            string callRefId = $"{Guid.NewGuid().ToString()}";
            bool configValue = Utils.SiteConfigData.DumpRequestContentToFile; 
            long logErrorId = 0;
            if (request.Method == HttpMethod.Post && configValue)
            {
                logRequest = true;
                try
                {
                    Business.LogError($"Http:Post", request.RequestUri.PathAndQuery, "");
                    string data = await request.Content.ReadAsStringAsync();
                    //string logFileName = HttpContext.Current.Server.MapPath("~/App_Data/" + Guid.NewGuid().ToString() + ".txt");

                    //string logFileName = HttpContext.Current.Server.MapPath(
                    //    $"~/App_Data/{Utils.SiteConfigData.SiteName.Replace(' ', '_')}_{Guid.NewGuid().ToString()}.txt"
                    //    );

                    string fileName = $"{Utils.SiteConfigData.SiteName.Replace(' ', '_')}_{ Guid.NewGuid().ToString()}.txt";
                    string logFileName = Helper.GetStorageFileNameWithPath(fileName);

                    if (!String.IsNullOrEmpty(data))
                    {
                        logErrorId = Business.LogError($"Request-{callRefId}", $"{logFileName} ({data.Length} bytes)", Utils.GetSnipForLog(data));
                        using (StreamWriter sw = new StreamWriter(logFileName))
                        {
                            sw.Write(data);
                            sw.Close();
                        }
                    }

                    // put file name and size in Request header - so as to retrieve it in controller/action
                    HttpContext.Current.Items.Add(Utils.PostDataFileNameKey, logFileName);
                    HttpContext.Current.Items.Add(Utils.PostDataFileSizeKey, $"{data.Length}");
                }
                catch (Exception ex)
                {
                    Business.LogError($"HandlerEx-{callRefId}", ex.ToString(), " ");
                }
            }
            else
            {
                //bool logGetRequest = Utils.ParseBoolString(ConfigurationManager.AppSettings["LogGetRequest"]);
                bool logGetRequest = Utils.SiteConfigData.LogGetRequest;
                if (request.Method == HttpMethod.Get && logGetRequest)
                {
                    Business.LogError($"Http:Get", request.RequestUri.PathAndQuery, "");
                }
            }

            // Call the inner handler.
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            // log response as well
            if (logRequest)
            {
                try
                {
                    string data = await response.Content.ReadAsStringAsync();
                    Business.LogError($"Response for {logErrorId}", data, " ");
                }
                catch(Exception ex)
                {
                    Business.LogError($"HandlerEx-{callRefId}", ex.ToString(), " ");
                }
            }

            return response;
        }
    }
}