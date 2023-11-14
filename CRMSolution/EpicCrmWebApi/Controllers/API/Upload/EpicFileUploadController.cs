using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace EpicCrmWebApi
{
    /// <summary>
    /// This API is used to facilitate file upload
    /// </summary>
    public class EpicFileUploadController : CrmBaseController
    {
        [HttpPost]
        public FileUploadResponse EpicUpload(FileUploadRequest request)
        {
            FileUploadResponse response = new FileUploadResponse()
            {
                RequestId = -1,
                DateTimeUtc = DateTime.UtcNow,
                Content = "",
                StatusCode = HttpStatusCode.BadRequest
            };

            string processName = $"{nameof(EpicUpload)}";

            string logString = $"{processName} : {request.FileName} | {request.TableName} | {request.IsCompleteRefresh} | {request.FileContent?.Length ?? 0} bytes";
            Business.LogError(processName, logString);

            if (!ModelState.IsValid)
            {
                LogModelErrors(nameof(EpicFileUploadController));
                response.StatusCode = HttpStatusCode.BadRequest;
                long errorLogId = Business.LogError(processName, $"FileName {request.FileName} | Invalid Model State");

                Task.Run(async () =>
                {
                    await Business.LogAlert("EFileUpload", $"Invalid Model State; Log Id {errorLogId}");
                });

                return response;
            }

            if (String.IsNullOrEmpty(request.TableName) || String.IsNullOrEmpty(request.FileName) ||
                (request.FileContent?.Length ?? 0) == 0)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                long errorLogId = Business.LogError(processName, $"FileName {request.FileName} | Invalid Request Data");
                Task.Run(async () =>
                {
                    await Business.LogAlert("EFileUpload", $"Invalid Request; Log Id {errorLogId}");
                });

                return response;
            }

            string fileName = request.FileName;
            string fileType = fileName.Split(new char[] { '.' })[1];

            string serverFileName = Helper.NewSaveFileName(Utils.SiteConfigData.SiteName ?? "", request.TableName, fileType);
            Business.LogError(processName, $"{request.FileName} being saved as {serverFileName}");

            try
            {
                File.WriteAllBytes(serverFileName, request.FileContent);

                // save record to indicate progress, in db
                // Here we want to save the upload type that is displayed to the user
                // and not the table name;
                var optionsList = Business.GetCodeTable("ExcelUpload");
                var optionRec = optionsList?.Where(x => x.Code == request.TableName).FirstOrDefault();
                DomainEntities.ExcelUploadStatus eus = new DomainEntities.ExcelUploadStatus()
                {
                    TenantId = Utils.SiteConfigData.TenantId,
                    UploadType = optionRec?.CodeName ?? "",
                    UploadTable = request.TableName,
                    UploadFileName = serverFileName,
                    IsCompleteRefresh = request.IsCompleteRefresh,
                    RecordCount = 0,
                    RequestedBy = processName,
                    IsPosted = false,
                    LocalFileName = request.FileName,
                    IsParsed = false,
                    ErrorCount = 0,
                    IsLocked = false,
                    LockTimestamp = DateTime.MinValue
                };

                response.RequestId = Business.CreateExcelUploadStatus(eus);
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                long errorLogId = Business.LogError(nameof(EpicUpload), ex.ToString(), " ");

                Task.Run(async () =>
                {
                    await Business.LogAlert("EFileUpload", $"Error occured while storing file; Log Id {errorLogId}");
                });

                response.Content = ex.ToString();
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }
    }
}
