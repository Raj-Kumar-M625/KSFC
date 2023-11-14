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
    // this api is used to get user configuration
    public class UserConfigController : CrmBaseController
    {
        [HttpPost]
        public JsonResult<StartDayResponse> Config(SqliteData sqliteDataObject)
        {
            StartDayResponse sdr = new StartDayResponse()
            {
                DateTimeUtc = DateTime.UtcNow,
                Content = "Invalid Data",
                TimeIntervalInMillisecondsForTracking = 0,
                BatchId = -1
            };

            if (sqliteDataObject == null)
            {
                return Json(sdr);
            }

            if (!ModelState.IsValid)
            {
                LogModelErrors(nameof(StartDay2Controller));
                sdr.StatusCode = HttpStatusCode.BadRequest;
                return Json(sdr);
            }

            if (!Business.IsRequestSourceValid(sqliteDataObject.EmployeeId, sqliteDataObject.IMEI))
            {
                sdr.StatusCode = HttpStatusCode.BadRequest;
                sdr.Content = "Invalid Source";
                sdr.EraseData = true;
                return Json(sdr);
            }

            // user must be active
            if (!Business.IsUserAllowed(sqliteDataObject.EmployeeId))
            {
                sdr.StatusCode = HttpStatusCode.BadRequest;
                sdr.Content = "User does not exist or not active.";
                sdr.EraseData = true;
                return Json(sdr);
            }

            // check if application version is supported
            string appVersion = String.IsNullOrEmpty(sqliteDataObject.DeviceInfo?.AppVersion) ? "legacy" : sqliteDataObject.DeviceInfo.AppVersion;
            //if (!Business.IsAppVersionSupported(appVersion, firstActionRecord.TimeStamp))
            // user may fudge the time on the phone - so always check version support
            // based on current time on server;
            if (!Business.IsAppVersionSupported(appVersion, Helper.GetCurrentIstDateTime() ))
            {
                sdr.StatusCode = HttpStatusCode.BadRequest;
                sdr.Content = $"App version {appVersion} is not supported.";
                sdr.EraseData = true;
                return Json(sdr);
            }

            try
            {
                sdr.StatusCode = HttpStatusCode.OK;
                sdr.Content = "";

                FillUserConfiguration(sqliteDataObject.EmployeeId, sdr);
            }
            catch (Exception ex)
            {
                sdr.Content = ex.ToString();
            }

            return Json(sdr);
        }
    }
}
