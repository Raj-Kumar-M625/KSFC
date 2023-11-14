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
using System.Web.Http.Results;

namespace EpicCrmWebApi
{
    // this api is deprecated from 1.7xt
    public class StartDayController : CrmBaseController
    {
        [HttpPost]
        [ActionName("Start")]
        public JsonResult<StartDayResponse> PostStartDay(StartDayRequest startDay)
        {
            StartDayResponse sdr = new StartDayResponse()
            {
                DateTimeUtc = DateTime.UtcNow,
                Content = "Invalid Data",
                TimeIntervalInMillisecondsForTracking = 0,
                BatchId = -1
            };

            if (startDay == null)
            {
                return Json(sdr);
            }

            if (!ModelState.IsValid)
            {
                LogModelErrors(nameof(StartDayController));
                sdr.StatusCode = HttpStatusCode.BadRequest;
                return Json(sdr);
            }

            if (!Business.IsRequestSourceValid(startDay.EmployeeId, startDay.IMEI))
            {
                sdr.StatusCode = HttpStatusCode.BadRequest;
                sdr.Content = "Invalid Source";
                sdr.EraseData = true;
                return Json(sdr);
            }

            // user must be active
            if (!Business.IsUserAllowed(startDay.EmployeeId))
            {
                sdr.StatusCode = HttpStatusCode.BadRequest;
                sdr.Content = "User does not exist or not active.";
                sdr.EraseData = true;
                return Json(sdr);
            }

            // check if application version is supported
            string appVersion = String.IsNullOrEmpty(startDay?.DeviceInfo?.AppVersion) ? "legacy" : startDay.DeviceInfo.AppVersion;
            // user may fudge the time on the phone - so always check version support
            // based on current time on server;
            if (!Business.IsAppVersionSupported(appVersion, Helper.GetCurrentIstDateTime()))
            //if (!Business.IsAppVersionSupported(appVersion, startDay.At))
            {
                sdr.StatusCode = HttpStatusCode.BadRequest;
                sdr.Content = $"App version {appVersion} is not supported.";
                sdr.EraseData = true;
                return Json(sdr);
            }

            SqliteDomainData domainDataObject = new SqliteDomainData()
            {
                EmployeeId = startDay.EmployeeId,
                DomainActions = null,
                DomainPayments = null,
                DomainExpense = null,
                DomainOrders = null,
                DomainReturnOrders = null,
                DomainLeaves = null,
                DomainCancelledLeaves = null,
                DomainEntities = null,
                DataFileName = GetBatchDataFileName(),
                DataFileSize = GetBatchDataFileSize()
            };

            domainDataObject.DomainActions = new List<SqliteDomainAction>()
            {
                new SqliteDomainAction()
                {
                        Id = 0,
                        PhoneDbId = startDay.Id ?? "",
                        TimeStamp = startDay.At,
                        ActivityTrackingType = 1,  // start day
                        Latitude = startDay.Latitude,
                        Longitude = startDay.Longitude,
                        MNC = startDay.MNC,
                        MCC = startDay.MCC,
                        LAC = startDay.LAC,
                        CellId = startDay.CellId,
                        ActivityType = startDay.ActivityType ?? "",
                        ClientType = "",
                        ClientName = "",
                        ClientPhone = "",
                        Comments = "",
                        Images = null,
                        InstrumentId = "",
                        PhoneModel = Utils.TruncateString(startDay?.DeviceInfo?.Model, 100),
                        PhoneOS = Utils.TruncateString(startDay?.DeviceInfo?.OSVersion, 10),
                        AppVersion = Utils.TruncateString(appVersion,10)
                }
            };

            try
            {
                sdr.BatchId = Business.SaveSqliteData(domainDataObject);
                sdr.StatusCode = HttpStatusCode.OK;
                sdr.Content = "";

                TenantEmployee te = Business.GetTenantEmployee(startDay.EmployeeId);
                sdr.TimeIntervalInMillisecondsForTracking = te != null ? te.TimeIntervalInMillisecondsForTrcking : 0;
                sdr.SendLogs = te?.SendLogFromPhone ?? false;
                sdr.AutoUpload = te?.AutoUploadFromPhone ?? false;

                // these fields can later be maintained at TenantEmployee level - if so needed;
                sdr.MaxDiscountPercentage = te?.MaxDiscountPercentage ?? 7.0;
                sdr.DiscountType = te?.DiscountType ?? "Amount";

                sdr.ActivityPageName = te?.ActivityPageName ?? "";

                sdr.AvailablePaidLeaves = 5;
                sdr.AvailableCompOffs = 7;
                sdr.ShowAvailableLeaveData = Utils.SiteConfigData.ShowAvailableLeaveDataOnPhone;
                sdr.WorkflowActivityEntityType = Utils.SiteConfigData.WorkflowActivityEntityType;
            }
            catch (Exception ex)
            {
                sdr.Content = ex.ToString();
            }

            return Json(sdr);
        }
    }
}
