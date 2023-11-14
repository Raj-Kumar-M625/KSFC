using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace EpicCrmWebApi
{
    /// <summary>
    /// This API is used to sync data from phone - August 4 2019
    /// apk version - 2.1.0205 or higher
    /// </summary>
    public class DataSync2Controller : DataSyncBaseController
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

            // tenant Id must match
            if (sqliteDataObject.TenantId != Utils.SiteConfigData.TenantId)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = $"Invalid Tenant {sqliteDataObject.TenantId}";
                response.EraseData = false;
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

            // PK UNCOMMENT BEFORE CHECKIN
            // user must be active
            if (!Business.IsUserAllowed(sqliteDataObject.EmployeeId))
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Content = $"User {sqliteDataObject.EmployeeId} does not exist or not active.";
                response.EraseData = true;
                Business.LogError($"{nameof(DataSync)}", $"Employee Id {sqliteDataObject.EmployeeId} | {response.Content}");
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

            if (sqliteDataObject == null)
            {
                Business.LogError(nameof(DataSync2Controller), $"{nameof(sqliteDataObject)} is null", " ");
            }

            if (sqliteDataObject.EmployeeId <= 0)
            {
                Business.LogError(nameof(DataSync2Controller), $"{nameof(sqliteDataObject.EmployeeId)} has invalid value {sqliteDataObject.EmployeeId}", " ");
            }

            if (String.IsNullOrEmpty(sqliteDataObject.BatchGuid))
            {
                Business.LogError(nameof(DataSync2Controller), $"{nameof(sqliteDataObject.BatchGuid)} has invalid value {sqliteDataObject.BatchGuid}", " ");
            }

            //if (sqliteDataObject.SqliteActionDataCollection == null)
            //{
            //    Business.LogError(nameof(DataSyncController), $"{nameof(sqliteDataObject.SqliteActionDataCollection)} is null", " ");
            //}

            try
            {
                // ensure that batch is not duplicate
                long existingBatchId = Business.GetSqliteBatchId(sqliteDataObject.EmployeeId, sqliteDataObject.BatchGuid);
                if (existingBatchId > 0)
                {
                    response.StatusCode = HttpStatusCode.OK;
                    response.Content = $"Batch {sqliteDataObject.BatchGuid} for EmployeeId {sqliteDataObject.EmployeeId} already exist";
                    response.EraseData = false;
                    response.BatchId = 0;
                    return response;
                }

                SqliteDomainData domainDataObject = new SqliteDomainData()
                {
                    BatchGuid = sqliteDataObject.BatchGuid,
                    EmployeeId = sqliteDataObject.EmployeeId,
                    ImageCount = sqliteDataObject?.Images?.Count() ?? 0,
                    ImageSaveCount = 0,
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
                    IsDataBatch = sqliteDataObject.IsDataBatch,
                    DataFileName = GetBatchDataFileName(),
                    DataFileSize = GetBatchDataFileSize(),
                    DomainSTRData = null,
                    DomainQuestionnaire = null, //Author: Rajesh V; Date24-06-2021 ; Purpose: dealer Questionnaire
                    DomainTask = null, 
                    DomainTaskAction = null 
                };

                DeviceInfo di = new DeviceInfo()
                {
                    IMEI = Utils.TruncateString(sqliteDataObject.DeviceInfo?.IMEI, 50),
                    Model = Utils.TruncateString(sqliteDataObject.DeviceInfo?.Model, 100),
                    OSVersion = Utils.TruncateString(sqliteDataObject.DeviceInfo?.OSVersion, 10),
                    AppVersion = Utils.TruncateString(sqliteDataObject.DeviceInfo?.AppVersion, 10),
                };

                if (sqliteDataObject.IsDataBatch)
                {
                    domainDataObject.DomainActions = FillDomainActions(sqliteDataObject, di, ImageFileNames);

                    // Create Domain object for Expenses
                    domainDataObject.DomainExpense = FillDomainExpenseObject(sqliteDataObject, ImageFileNames);
                    domainDataObject.DomainOrders = FillDomainOrderObject(sqliteDataObject, ImageFileNames);
                    domainDataObject.DomainPayments = FillDomainPaymentObject(sqliteDataObject, ImageFileNames);
                    domainDataObject.DomainReturnOrders = FillDomainOrderReturnsObject(sqliteDataObject);
                    domainDataObject.DomainLeaves = FillDomainLeavesObject(sqliteDataObject);
                    domainDataObject.DomainCancelledLeaves = FillDomainCancelledLeavesObject(sqliteDataObject);
                    domainDataObject.DomainEntities = FillDomainEntityObject(sqliteDataObject, ImageFileNames);
                    domainDataObject.DomainAgreements = FillDomainAgreements(sqliteDataObject);
                    domainDataObject.DomainSurveys = FillDomainSurveys(sqliteDataObject);

                    domainDataObject.DomainDayPlanTarget = FillDomainDayPlanTarget(sqliteDataObject);

                    domainDataObject.DomainBankDetails = FillDomainBankDetails(sqliteDataObject, ImageFileNames);

                    domainDataObject.DeviceLogs = FillDomainDeviceLogs(sqliteDataObject);
                    domainDataObject.DomainIssueReturns = FillDomainIssueReturns(sqliteDataObject);
                    domainDataObject.DomainAdvanceRequests = FillDomainAdvanceRequests(sqliteDataObject);

                    domainDataObject.DomainWorkFlowPageData = FillDomainWorkFlowPageData(sqliteDataObject);

                    domainDataObject.DomainTerminateAgreementData = FillDomainTerminateAgreementRequests(sqliteDataObject);

                    domainDataObject.DomainSTRData = FillDomainSTRData(sqliteDataObject, ImageFileNames);
                    //Author: Rajesh V; Date24-06-2021 ; Purpose: dealer Questionnaire
                    domainDataObject.DomainQuestionnaire = FillDomainQuestionnaire(sqliteDataObject);

                    domainDataObject.DomainTask = FillDomainTask(sqliteDataObject);
                    domainDataObject.DomainTaskAction = FillDomainTaskAction(sqliteDataObject);

                }

                // save images
                if (domainDataObject.ImageCount > 0)
                {
                    domainDataObject.ImageSaveCount = sqliteDataObject.Images.Select(x =>
                    {
                        return Business.SaveImageDataInFile(x.BinaryData, x.Id, "jpg");
                    }).Count(x => x);
                }

                response.BatchId = Business.SaveSqliteData(domainDataObject);
                response.StatusCode = HttpStatusCode.OK;

                // If it was an image data batch - we possibly can delete the post data file.
                // provided, all images are saved on disk;
                if (sqliteDataObject.IsImageBatch && 
                    domainDataObject.ImageCount == domainDataObject.ImageSaveCount &&
                    Utils.KeepImagePostDataFile == false)
                {
                    Helper.DeleteFile(domainDataObject.DataFileName);
                }
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(DataSync2Controller), ex.ToString(), " ");
                response.Content = ex.ToString();
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }

        private IEnumerable<string> ImageFileNames(SqliteBase sqliteBase)
        {
            if (sqliteBase?.ImageFileNames == null)
            {
                return null;
            }

            return sqliteBase.ImageFileNames.Select(x => $"{x}.jpg");
        }
    }
}
