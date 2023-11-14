using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace EpicCrmWebApi
{
    public class CompressDownloadController : ApiController
    {
        /// <summary>
        /// Get Compressed download data, in Base 64 format
        /// </summary>
        /// <param name="employeeId">Tenant Employee Id</param>
        /// <returns></returns>
        [HttpGet]
        public CompressDownloadDataResponse GetData(long employeeId, string IMEI = "", string areaCode = "", string appVersion = "")
        {
            // get the response to be sent
            DownloadDataResponse response = DownloadController.CreateDownloadObject(
                                    employeeId, IMEI, areaCode, appVersion);

            // compress and send
            try
            {
                // convert it to json
                string responseAsString = JsonConvert.SerializeObject(response);

                // compress it
                // 1. make a byte array
                byte[] inputBytes = Encoding.UTF8.GetBytes(responseAsString);

                // 2. Create GZip
                using (MemoryStream ms = new MemoryStream())
                {
                    using (var gzip = new GZipStream(ms, CompressionMode.Compress))
                    {
                        gzip.Write(inputBytes, 0, inputBytes.Length);
                    }
                    string compressedBase64Response = Convert.ToBase64String(ms.ToArray());

                    return new CompressDownloadDataResponse()
                    {
                        EncryptedData = compressedBase64Response,
                        Status = true,
                        UtcTimestamp = DateTime.UtcNow,
                        ErrorMessage = ""
                    };
                }
            }
            catch(Exception ex)
            {
                Business.LogError($"{nameof(CompressDownloadController)}", ex);
                return new CompressDownloadDataResponse()
                {
                    EncryptedData = "",
                    Status = false,
                    ErrorMessage = ex.Message,
                    UtcTimestamp = DateTime.UtcNow
                };
            }
        }
    }
}
