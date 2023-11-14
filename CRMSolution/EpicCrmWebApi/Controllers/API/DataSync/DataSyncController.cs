using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EpicCrmWebApi
{
    /// <summary>
    /// This API is used to sync data from phone
    /// </summary>
    public class DataSyncController : DataSyncBaseController
    {
        [HttpPost]
        public DataSyncResponse DataSync(SqliteData sqliteDataObject)
        {
            DataSyncResponse response = new DataSyncResponse()
            {
                BatchId = -1,
                DateTimeUtc = DateTime.UtcNow,
                Content = "",
                StatusCode = HttpStatusCode.BadRequest
            };

            if (!ModelState.IsValid)
            {
                LogModelErrors(nameof(DataSyncController));
                response.StatusCode = HttpStatusCode.BadRequest;
                Business.LogError($"{nameof(DataSync)}", $"Employee Id {sqliteDataObject.EmployeeId} | Invalid Model State");
                return response;
            }

            if (!Business.IsRequestSourceValid(sqliteDataObject.EmployeeId, sqliteDataObject.IMEI))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = $"App is not supported on this device: {sqliteDataObject.EmployeeId} | {sqliteDataObject.IMEI}";
                response.EraseData = true;
                Business.LogError($"{nameof(DataSync)}", $"Employee Id {sqliteDataObject.EmployeeId} | {response.Content}");
                return response;
            }

            // user must be active
            if (!Business.IsUserAllowed(sqliteDataObject.EmployeeId))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = "User does not exist or not active.";
                response.EraseData = true;
                Business.LogError($"{nameof(DataSync)}", $"Employee Id {sqliteDataObject.EmployeeId} | {response.Content}");
                return response;
            }

            string appVersion = String.IsNullOrEmpty(sqliteDataObject?.DeviceInfo?.AppVersion)
                ? "legacy"
                : sqliteDataObject.DeviceInfo.AppVersion;
            if (!Business.IsAppVersionSupportedOnUpload(appVersion, Helper.GetCurrentIstDateTime()))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = $"App version {appVersion} is not supported.";
                response.EraseData = true;
                Business.LogError($"{nameof(DataSync)}", $"Employee Id {sqliteDataObject.EmployeeId} | {response.Content}");
                return response;
            }

            if (sqliteDataObject == null)
            {
                Business.LogError(nameof(DataSyncController), $"{nameof(sqliteDataObject)} is null", " ");
            }

            if (sqliteDataObject.EmployeeId <= 0)
            {
                Business.LogError(nameof(DataSyncController), $"{nameof(sqliteDataObject.EmployeeId)} has invalid value {sqliteDataObject.EmployeeId}", " ");
            }

            //if (sqliteDataObject.SqliteActionDataCollection == null)
            //{
            //    Business.LogError(nameof(DataSyncController), $"{nameof(sqliteDataObject.SqliteActionDataCollection)} is null", " ");
            //}

            try
            {
                SqliteDomainData domainDataObject = new SqliteDomainData()
                {
                    BatchGuid = Guid.NewGuid().ToString(),
                    EmployeeId = sqliteDataObject.EmployeeId,
                    ImageCount = 0,
                    DomainActions = null,
                    DomainPayments = null,
                    DomainExpense = null,
                    DomainOrders = null,
                    DomainReturnOrders = null,
                    DomainLeaves = null,
                    DomainCancelledLeaves = null,
                    DomainEntities = null,
                    DomainIssueReturns = null,
                    DomainWorkFlowPageData = null,
                    DeviceLogs = null,
                    IsDataBatch = true,
                    ImageSaveCount = 0,
                    DataFileName = GetBatchDataFileName(),
                    DataFileSize = GetBatchDataFileSize()
                };

                DeviceInfo di = new DeviceInfo()
                {
                    IMEI = Utils.TruncateString(sqliteDataObject.DeviceInfo?.IMEI, 50),
                    Model = Utils.TruncateString(sqliteDataObject.DeviceInfo?.Model, 100),
                    OSVersion = Utils.TruncateString(sqliteDataObject.DeviceInfo?.OSVersion, 10),
                    AppVersion = Utils.TruncateString(sqliteDataObject.DeviceInfo?.AppVersion, 10),
                };

                //domainDataObject.DomainAction = 
                if (sqliteDataObject.SqliteActionDataCollection != null)
                {
                    domainDataObject.DomainActions = FillDomainActions(sqliteDataObject, di, ImageFileNames);
                }

                // Create Domain object for Expenses
                domainDataObject.DomainExpense = FillDomainExpenseObject(sqliteDataObject, ImageFileNames);
                domainDataObject.DomainOrders = FillDomainOrderObject(sqliteDataObject, ImageFileNames);
                domainDataObject.DomainPayments = FillDomainPaymentObject(sqliteDataObject, ImageFileNames);
                domainDataObject.DomainReturnOrders = FillDomainOrderReturnsObject(sqliteDataObject);
                domainDataObject.DomainLeaves = FillDomainLeavesObject(sqliteDataObject);
                domainDataObject.DomainCancelledLeaves = FillDomainCancelledLeavesObject(sqliteDataObject);
                domainDataObject.DomainEntities = FillDomainEntityObject(sqliteDataObject, ImageFileNames);
                domainDataObject.DeviceLogs = FillDomainDeviceLogs(sqliteDataObject);
                domainDataObject.DomainIssueReturns = FillDomainIssueReturns(sqliteDataObject);
                domainDataObject.DomainWorkFlowPageData = FillDomainWorkFlowPageData(sqliteDataObject);

                response.BatchId = Business.SaveSqliteData(domainDataObject);
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(DataSyncController), ex.ToString(), " ");
                response.Content = ex.ToString();
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }

        private IEnumerable<string> ImageFileNames(SqliteBase sqliteBase)
        {
            if (sqliteBase?.Images == null)
            {
                return null;
            }

            return sqliteBase.Images.Select(y =>
            {
                return Business.SaveImageDataInFile(y.BinaryData, Helper.ActivityImageFilePrefix);
            }).ToList();
        }
    }
}
