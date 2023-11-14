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
    /// This API is used to ping for batch check from phone - August 4 2019
    /// apk version - 2.1.0205 or higher
    /// </summary>
    public class DataSyncPingController : CrmBaseController
    {
        [HttpPost]
        public DataSyncResponse Ping(SqliteData sqliteDataObject)
        {
            DataSyncResponse response = new DataSyncResponse()
            {
                BatchId = -1,
                DateTimeUtc = DateTime.UtcNow,
                Content = "",
                StatusCode = HttpStatusCode.BadRequest,
                EraseData = false
            };

            if (!ModelState.IsValid)
            {
                LogModelErrors(nameof(DataSyncPingController));
                response.StatusCode = HttpStatusCode.BadRequest;
                return response;
            }

            if (sqliteDataObject == null)
            {
                Business.LogError(nameof(DataSyncPingController), $"{nameof(sqliteDataObject)} is null", " ");
            }

            if (sqliteDataObject.EmployeeId <= 0)
            {
                Business.LogError(nameof(DataSyncPingController), $"{nameof(sqliteDataObject.EmployeeId)} has invalid value {sqliteDataObject.EmployeeId}", " ");
            }

            if (String.IsNullOrEmpty(sqliteDataObject.BatchGuid))
            {
                Business.LogError(nameof(DataSyncPingController), $"{nameof(sqliteDataObject.BatchGuid)} has invalid value {sqliteDataObject.BatchGuid}", " ");
            }

            //if (sqliteDataObject.SqliteActionDataCollection == null)
            //{
            //    Business.LogError(nameof(DataSyncController), $"{nameof(sqliteDataObject.SqliteActionDataCollection)} is null", " ");
            //}

            // ensure that batch is not duplicate

            try
            {
                long batchId = Business.GetSqliteBatchId(sqliteDataObject.EmployeeId, sqliteDataObject.BatchGuid);
                if (batchId > 0)
                {
                    response.Content = $"Batch {sqliteDataObject.BatchGuid} for EmployeeId {sqliteDataObject.EmployeeId} already exist";
                }
                else
                {
                    response.Content = "";
                }

                response.BatchId = batchId;
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(DataSyncPingController), ex.ToString(), " ");
                response.Content = ex.ToString();
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }
    }
}
