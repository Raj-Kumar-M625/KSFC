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
    // this api is used from app version 1.7xt to start day
    // basically - it has location coordinates from multiple sources
    public class StartDay2Controller : CrmBaseController
    {
        [HttpPost]
        [ActionName("Start")]
        public JsonResult<StartDayResponse> PostStartDay(SqliteData sqliteDataObject)
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

            // retrieve first sqliteaction record
            SqliteAction firstActionRecord = sqliteDataObject?.SqliteActionDataCollection?.FirstOrDefault();
            if (firstActionRecord == null)
            {
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
                sdr.EraseData = false;
                return Json(sdr);

                // apk 2.1.0260 onwards - Dec 23 2019
                // changed erase data from true to false, to support download scenario
                // so user can stay on start day page and press download/install
            }

            SqliteDomainData domainDataObject = new SqliteDomainData()
            {
                BatchGuid = Guid.NewGuid().ToString(),
                EmployeeId = sqliteDataObject.EmployeeId,
                DomainActions = null,
                DomainPayments = null,
                DomainExpense = null,
                DomainOrders = null,
                DomainReturnOrders = null,
                DomainLeaves = null,
                DomainCancelledLeaves = null,
                DomainEntities = null,
                IsDataBatch = true,
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

            domainDataObject.DomainActions = sqliteDataObject.SqliteActionDataCollection.Select(x => new SqliteDomainAction()
            {
                Id = 0,
                PhoneDbId = x.Id ?? "",
                TimeStamp = x.TimeStamp,
                ActivityTrackingType = x.ActivityTrackingType,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                MNC = x.MNC,
                MCC = x.MCC,
                LAC = x.LAC,
                CellId = x.CellId,
                ActivityType = x.ActivityType,
                ClientType = x.ClientType,
                ClientName = x.ClientName,
                ClientPhone = x.ClientPhone,
                Comments = x.Comments,
                Images = x.Images != null ? x.Images.Select(y => {
                    return Business.SaveImageDataInFile(y.BinaryData, Helper.ActivityImageFilePrefix);
                }
                ).ToList() : null,
                Contacts = x.Contacts?.Select(y => new SqliteDomainActionContact()
                {
                    Name = y.Name,
                    PhoneNumber = y.PhoneNumber,
                    IsPrimary = y.IsPrimary
                }).ToList(),
                IMEI = di.IMEI,
                AtBusiness = x.AtBusiness,
                LocationTaskStatus = Utils.TruncateString(x.LocationTaskStatus, 50),
                LocationException = Utils.TruncateString(x.LocationException, 256),
                ClientCode = Utils.TruncateString(x.ClientCode, 50),
                InstrumentId = Utils.TruncateString(x.InstrumentId, 50),
                ActivityAmount = x.ActivityAmount,
                PhoneModel = di.Model,
                PhoneOS = di.OSVersion,
                AppVersion = di.AppVersion,
                Locations = x.Locations?.Select(y => new SqliteDomainActionLocation()
                {
                    Source = Utils.TruncateString(y.Source, 50),
                    Latitude = y.Latitude,
                    Longitude = y.Longitude,
                    UtcAt = y.UtcAt,
                    LocationTaskStatus = Utils.TruncateString(y.LocationTaskStatus, 50),
                    LocationException = Utils.TruncateString(y.LocationException, 256),
                    IsGood = y.IsGood
                }).ToList()
            }).ToList();

            try
            {
                sdr.BatchId = Business.SaveSqliteData(domainDataObject);
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
