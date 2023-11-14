using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using DomainEntities.Questionnaire;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace EpicCrmWebApi
{
    [AuthorizeSuperAdminAttribute]
    public class SuperAdminController : BaseDashboardController
    {
        // GET: Super Admin Page
        public ActionResult Index()
        {
            ViewBag.IsSetupSuperAdmin = IsSetupSuperAdmin;
            return View();
        }

        public ActionResult ShowSearch()
        {
            ModelFilter modelFilter = new ModelFilter()
            {
                // this returns only applicable zones/Area/... for current user
                Zones = Business.GetZones(IsSuperAdmin, GetOfficeHierarchy()),
                Areas = Business.GetAreas(IsSuperAdmin, GetOfficeHierarchy()),
                Territories = Business.GetTerritories(IsSuperAdmin, GetOfficeHierarchy()),
                HeadQuarters = Business.GetHeadQuarters(IsSuperAdmin, GetOfficeHierarchy()),
            };

            ViewBag.SearchButtonText = "Search";
            ViewBag.ReportType = "Customer";
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            return View(modelFilter);
        }

        [SetupSuperAdmin]
        public ActionResult ShowConfig()
        {
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult GetSearchResult(SearchCriteria searchCriteria)
        {
            DomainEntities.SearchCriteria s = ParseSearchCriteria(searchCriteria);

            ViewBag.ReportType = searchCriteria.ReportType;

            return this.GetCustomers(s);
        }

        public ActionResult ShowSqliteBatches(long employeeId, bool onlyUnprocessedBatches = false,
                                bool onlyLockedBatches = false, int batchCount = -1,
                                long inRecentBatches = 0)
        {
            batchCount = (batchCount <= 0) ? 7 : batchCount;

            inRecentBatches = (inRecentBatches <= 0) ? 3000 : inRecentBatches;

            IEnumerable<SqliteDomainBatch> batches = Business.GetBatches(employeeId, onlyLockedBatches,
                                                    onlyUnprocessedBatches, inRecentBatches);

            ViewBag.BatchCount = batchCount;
            ViewBag.InputEmployeeId = employeeId;

            PutFeatureSetInViewBag();

            return View(batches);
        }

        private ActionResult ExcelUploadHistory(int startItem, int itemCount = 50)
        {
            IEnumerable<DomainEntities.ExcelUploadHistory> historyItems
                = Business.GetExcelUploadHistory(Utils.SiteConfigData.TenantId, startItem, itemCount);

            var modelHistoryItems = historyItems.Select(x => new ExcelUploadHistory()
            {
                Id = x.Id,
                TenantId = x.TenantId,
                UploadType = x.UploadType,
                UploadFileName = x.UploadFileName,
                IsCompleteRefresh = x.IsCompleteRefresh,
                RecordCount = x.RecordCount,
                RequestedBy = x.RequestedBy,
                RequestTimestamp = Helper.ConvertUtcTimeToIst(x.RequestTimestamp),
                PostingTimestamp = Helper.ConvertUtcTimeToIst(x.PostingTimestamp),
            });

            return PartialView("ExcelUploadHistory", modelHistoryItems);
        }

        private ActionResult ShowSqliteBatchProcessLog(int startItem, int itemCount = 50)
        {
            IEnumerable<SqliteActionProcessLog> logItems = Business.GetSqliteBatchProcessLog(startItem, itemCount);
            var model = logItems.Select(x => new SqliteActionProcessLogModel()
            {
                Id = x.Id,
                TenantId = x.TenantId,
                EmployeeId = x.EmployeeId,
                LockAcquiredStatus = x.LockAcquiredStatus,
                At = Helper.ConvertUtcTimeToIst(x.At),
                Timestamp = Helper.ConvertUtcTimeToIst(x.Timestamp),
                HasCompleted = x.HasCompleted,
                HasFailed = x.HasFailed
            }).ToList();
            return PartialView("ShowSqliteBatchProcessLog", model);
        }

        private ActionResult ShowDistanceCalcErrorLog(int startItem, int itemCount = 50)
        {
            IEnumerable<DistanceCalcErrorLog> logItems = Business.GetDistanceCalcErrorLog(startItem, itemCount);
            var model = logItems.Select(x => new DistanceCalcErrorLogModel()
            {
                Id = x.Id,
                APIName = x.APIName,
                EmployeeCode = x.EmployeeCode,
                EmployeeName = x.EmployeeName,
                ErrorText = Utils.TruncateString(x.ErrorText, 500),
                At = x.At,
                TrackingId = x.TrackingId,
                ActivityId = x.ActivityId,
                ActivityType = x.ActivityType
            }).ToList();
            return PartialView("ShowDistanceCalcErrorLog", model);
        }

        private ActionResult ShowEntityAgreements(int startItem, int itemCount = 50)
        {
            IEnumerable<EntityAgreement> logItems = Business.GetEntityAgreements(startItem, itemCount);
            var model = logItems.Select(x => new EntityAgreementModel()
            {
                Id = x.Id,
                AgreementNumber = x.AgreementNumber,
                TypeName = x.TypeName,
                LandSizeInAcres = x.LandSizeInAcres
            }).ToList();
            return PartialView("ShowEntityAgreements", model);
        }

        private ActionResult ShowDatafeedProcessLog(int startItem, int itemCount = 50)
        {
            IEnumerable<SqliteActionProcessLog> logItems = Business.GetDataFeedProcessLog(startItem, itemCount);
            var model = logItems.Select(x => new SqliteActionProcessLogModel()
            {
                Id = x.Id,
                TenantId = x.TenantId,
                EmployeeId = x.EmployeeId,
                LockAcquiredStatus = x.LockAcquiredStatus,
                At = Helper.ConvertUtcTimeToIst(x.At),
                Timestamp = Helper.ConvertUtcTimeToIst(x.Timestamp),
                HasCompleted = x.HasCompleted,
                HasFailed = x.HasFailed,
                ProcessName = x.ProcessName
            }).ToList();
            return PartialView("ShowDatafeedProcessLog", model);
        }

        private ActionResult ShowSmsJobLog(int startItem, int itemCount = 50)
        {
            IEnumerable<SqliteActionProcessLog> logItems = Business.GetSmsJobLog(startItem, itemCount);
            var model = logItems.Select(x => new SqliteActionProcessLogModel()
            {
                Id = x.Id,
                TenantId = x.TenantId,
                EmployeeId = x.EmployeeId,
                LockAcquiredStatus = x.LockAcquiredStatus,
                At = Helper.ConvertUtcTimeToIst(x.At),
                Timestamp = Helper.ConvertUtcTimeToIst(x.Timestamp),
                HasCompleted = x.HasCompleted,
                HasFailed = x.HasFailed
            }).ToList();
            return PartialView("ShowSmsJobLog", model);
        }

        private ActionResult ShowPurgeDataLog(int startItem, int itemCount = 50)
        {
            IEnumerable<PurgeDataLog> logItems = Business.GetPurgeDataLog(startItem, itemCount);
            var model = logItems.Select(x => new PurgeDataModel()
            {
                Id = x.Id,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                ActionPurged = x.ActionPurged,
                ActionDupPurged = x.ActionDupPurged,
                ExpensePurged = x.ExpensePurged,
                OrderPurged = x.OrderPurged,
                PaymentPurged = x.PaymentPurged,
                ReturnPurged = x.ReturnPurged,
                EntityPurged = x.EntityPurged,
                DateCreated = x.DateCreated
            }).ToList();
            return PartialView("ShowPurgeDataLog", model);
        }

        public ActionResult ShowSavedActionBatchItems(long batchId, string empName)
        {
            IEnumerable<SqliteActionDisplayData> sqliteActionDisplayData = Business.GetSavedBatchItems(batchId);

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(sqliteActionDisplayData);
        }

        public ActionResult ShowDupActionBatchItems(long batchId, string empName)
        {
            IEnumerable<SqliteActionDisplayData> sqliteActionDisplayData = Business.GetDupBatchItems(batchId);
            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(sqliteActionDisplayData);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult ShowSavedActionContacts(long actionId)
        {
            IEnumerable<SqliteActionContactData> sqliteActionContactData = Business.GetSavedSqliteActionContacts(actionId);

            var sqliteActionContactDisplayData = sqliteActionContactData.Select(x => new SqliteActionContactDisplayData()
            {
                Id = x.Id,
                SqliteActionId = x.SqliteActionId,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
                IsPrimary = x.IsPrimary
            }).ToList();

            return PartialView("_ShowSavedActionContacts", sqliteActionContactDisplayData);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult ShowSavedActionLocations(long actionId)
        {
            IEnumerable<SqliteActionLocationData> sqliteActionLocationData = Business.GetSavedSqliteActionLocations(actionId);

            var displayData = sqliteActionLocationData.Select(x => new SqliteActionLocationDisplayData()
            {
                Id = x.Id,
                SqliteActionId = x.SqliteActionId,
                Source = x.Source,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                At = Helper.ConvertUtcTimeToIst(x.UtcAt),
                LocationTaskStatus = x.LocationTaskStatus,
                LocationException = x.LocationException,
                IsGood = x.IsGood
            }).ToList();

            return PartialView("_ShowSavedActionLocations", displayData);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult ShowSavedEntityLocations(long entityId)
        {
            IEnumerable<SqliteEntityLocationData> sqliteActionLocationData = Business.GetSavedSqliteEntityLocations(entityId);

            var displayData = sqliteActionLocationData.Select(x => new SqliteEntityLocationDisplayData()
            {
                Id = x.Id,
                SqliteEntityId = x.SqliteEntityId,
                Source = x.Source,
                Latitude = x.Latitude,
                Longitude = x.Longitude,
                At = Helper.ConvertUtcTimeToIst(x.UtcAt),
                LocationTaskStatus = x.LocationTaskStatus,
                LocationException = x.LocationException,
                IsGood = x.IsGood
            }).ToList();

            return PartialView("_ShowSavedEntityLocations", displayData);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult ShowSavedSTRDWS(long sqliteSTRId)
        {
            IEnumerable<DomainEntities.SqliteDWSData> sqliteDWSData = Business.GetSavedSqliteSTRDWS(sqliteSTRId);

            var displayData = sqliteDWSData.Select(x => new SqliteDWSDisplayData()
            {
                Id = x.Id,
                SqliteSTRId = sqliteSTRId,
                IsProcessed = x.IsProcessed,
                DWSId = x.DWSId,
                EntityId = x.EntityId,
                EntityName = x.EntityName,
                AgreementId = x.AgreementId,
                Agreement = x.Agreement,
                EntityWorkFlowDetailId = x.EntityWorkFlowDetailId,
                TypeName = x.TypeName,
                TagName = x.TagName,
                DWSNumber = x.DWSNumber,
                BagCount = x.BagCount,
                FilledBagsWeightKg = x.FilledBagsWeightKg,
                EmptyBagsWeightKg = x.EmptyBagsWeightKg,
                TimeStamp = x.TimeStamp,
                ActivityId = x.ActivityId,
            }).ToList();

            return PartialView("_ShowSavedDWSData", displayData);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult ShowFollowUps(long ewfId)
        {
            IEnumerable<SqliteEntityWorkFlowFollowUp> followUps = Business.GetSavedSqliteWorkFlowFollowUps(ewfId);

            var displayData = followUps.Select(x => new SqliteEntityWorkFlowFollowUpDisplayData()
            {
                Phase = x.Phase,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                Notes = x.Notes
            }).ToList();

            return PartialView("_ShowSavedWorkFlowFollowUps", displayData);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult ShowItemsUsed(long ewfId)
        {
            IEnumerable<string> itemsUsed = Business.GetSavedSqliteWorkFlowItemsUsed(ewfId);

            return PartialView("_ShowSavedWorkFlowItemsUsed", itemsUsed);
        }

        public ActionResult ShowSavedExpenseItems(long batchId, string empName)
        {
            IEnumerable<SqliteExpenseData> sqliteExpenseData = Business.GetSavedExpenseItems(batchId);

            var sqliteExpenseDisplayData = sqliteExpenseData.Select(x => new SqliteExpenseDisplayData()
            {
                Id = x.Id,
                SqliteTableId = x.SqliteTableId,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                ExpenseType = x.ExpenseType,
                Amount = x.Amount,
                OdometerStart = x.OdometerStart,
                OdometerEnd = x.OdometerEnd,
                VehicleType = x.VehicleType,
                ImageCount = x.ImageCount,
                IsProcessed = x.IsProcessed,
                ExpenseItemId = x.ExpenseItemId,
                FuelType = x.FuelType,
                FuelQuantityInLiters = x.FuelQuantityInLiters,
                Comment = x.Comment
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(sqliteExpenseDisplayData);
        }

        public ActionResult ShowSavedOrders(long batchId, string empName)
        {
            IEnumerable<SqliteOrderData> sqliteOrderData = Business.GetSavedOrders(batchId);

            var sqliteOrdersDisplayData = sqliteOrderData.Select(x => new SqliteOrderDisplayData()
            {
                Id = x.Id,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                CustomerCode = x.CustomerCode,
                OrderDate = x.OrderDate,
                OrderType = x.OrderType,
                TotalAmount = x.TotalAmount,
                ItemCount = x.ItemCount,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                IsProcessed = x.IsProcessed,
                OrderId = x.OrderId,
                PhoneDbId = x.PhoneDbId,
                PhoneActivityId = x.PhoneActivityId,
                TotalGST = x.TotalGST,
                NetAmount = x.NetAmount,
                MaxDiscountPercentage = x.MaxDiscountPercentage,
                DiscountType = x.DiscountType,
                AppliedDiscountPercentage = x.AppliedDiscountPercentage,
                ImageCount = x.ImageCount
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(sqliteOrdersDisplayData);
        }

        public ActionResult ShowDeviceLog(long batchId, string empName)
        {
            IEnumerable<DomainEntities.SqliteDeviceLog> sqliteDeviceLogs = Business.GetDeviceLogs(batchId);

            var displayModel = sqliteDeviceLogs.Select(x => new SqliteDeviceLogDisplayData()
            {
                Id = x.Id,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                LogText = x.LogText,
                TimeStamp = x.TimeStamp

            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(displayModel);
        }

        public ActionResult ShowSavedReturns(long batchId, string empName)
        {
            IEnumerable<SqliteReturnOrderData> sqliteOrderData = Business.GetSavedReturns(batchId);

            var sqliteReturnsDisplayData = sqliteOrderData.Select(x => new SqliteReturnOrderDisplayData()
            {
                Id = x.Id,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                CustomerCode = x.CustomerCode,
                ReturnOrderDate = x.ReturnOrderDate,
                TotalAmount = x.TotalAmount,
                ItemCount = x.ItemCount,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                IsProcessed = x.IsProcessed,
                ReturnOrderId = x.ReturnOrderId,
                ReferenceNum = x.ReferenceNum,
                Comment = x.Comment,
                PhoneDbId = x.PhoneDbId,
                PhoneActivityId = x.PhoneActivityId
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(sqliteReturnsDisplayData);
        }

        public ActionResult ShowSavedOrderItems(long sqliteOrderId, string empName)
        {
            IEnumerable<SqliteOrderLineData> orderLines = Business.GetSavedOrderItems(sqliteOrderId);

            var orderDisplayLines = orderLines.Select(x => new SqliteOrderLineDisplayData()
            {
                Id = x.Id,
                SqliteOrderId = x.SqliteOrderId,
                SerialNumber = x.SerialNumber,
                ProductCode = x.ProductCode,
                UnitPrice = x.UnitPrice,
                UnitQuantity = x.UnitQuantity,
                Amount = x.Amount,
                DiscountPercent = x.DiscountPercent,
                DiscountedPrice = x.DiscountedPrice,
                ItemPrice = x.ItemPrice,
                GstPercent = x.GstPercent,
                GstAmount = x.GstAmount,
                NetPrice = x.NetPrice
            }).ToList();

            ViewBag.SqliteOrderId = sqliteOrderId;
            ViewBag.EmpName = empName;

            return View(orderDisplayLines);
        }

        public ActionResult ShowSavedReturnItems(long sqliteReturnOrderId, string empName)
        {
            IEnumerable<SqliteReturnOrderItemData> orderLines = Business.GetSavedReturnOrderItems(sqliteReturnOrderId);

            var model = orderLines.Select(x => new SqliteReturnOrderItemDisplayData()
            {
                Id = x.Id,
                SqliteReturnOrderId = x.SqliteReturnOrderId,
                SerialNumber = x.SerialNumber,
                ProductCode = x.ProductCode,
                UnitPrice = x.UnitPrice,
                UnitQuantity = x.UnitQuantity,
                Amount = x.Amount,
                Comment = x.Comment
            }).ToList();

            ViewBag.SqliteReturnOrderId = sqliteReturnOrderId;
            ViewBag.EmpName = empName;

            return View(model);
        }

        [SetupSuperAdmin]
        public ActionResult ClearData(long employeeId)
        {
            if (Helper.ShowTerminateAndDeleteLinksOnCrmUsersPage)
            {
                RemovePortalAccess(employeeId);
                Business.ClearData(employeeId);
            }

            return RedirectToAction("ShowRegisteredUsers", "Admin");
        }

        [AjaxOnly]
        [HttpGet]
        [SetupSuperAdmin]
        public ActionResult TogglePhoneLog(long empId)
        {
            Business.TogglePhoneLog(empId);
            return new EmptyResult();
        }

        //https://stackoverflow.com/questions/17657184/using-jquerys-ajax-method-to-retrieve-images-as-a-blob
        //https://stackoverflow.com/questions/26902067/load-jpeg-image-from-mvc-controller-via-javascript
        //https://www.codeproject.com/articles/33310/c-save-and-load-image-from-database
        //https://stackoverflow.com/questions/16386514/how-can-i-get-image-data-from-a-server-side-perl-program-and-display-in-a-div-us
        //[AjaxOnly]
        [HttpGet]
        public EmptyResult ImageData(long expenseId, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.ImageData(expenseId, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
            //return File(imageBytes, "image/jpeg");
        }

        [HttpGet]
        public EmptyResult PaymentImageData(long Id, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.PaymentImageData(Id, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        [HttpGet]
        public EmptyResult ActivityImageData(long Id, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.ActivityImageData(Id, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        public ActionResult ShowSavedPayments(long batchId, string empName)
        {
            IEnumerable<SqlitePaymentData> sqlitePaymentData = Business.GetSavedPayments(batchId);

            var sqlitePaymentDisplayData = sqlitePaymentData.Select(x => new SqlitePaymentDisplayData()
            {
                Id = x.Id,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                ImageCount = x.ImageCount,
                IsProcessed = x.IsProcessed,
                Comment = x.Comment,
                CustomerCode = x.CustomerCode,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                PaymentDate = x.PaymentDate,
                PaymentId = x.PaymentId,
                PaymentType = x.PaymentType,
                TotalAmount = x.TotalAmount,
                PhoneDbId = x.PhoneDbId,
                PhoneActivityId = x.PhoneActivityId
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(sqlitePaymentDisplayData);
        }

        public ActionResult ShowSavedEntities(long batchId, string empName)
        {
            IEnumerable<SqliteEntityData> sqliteEntityData = Business.GetSavedSqliteEntities(batchId);

            var sqliteEntityDisplayData = sqliteEntityData.Select(x => new SqliteEntityDisplayData()
            {
                Id = x.Id,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                PhoneDbId = x.PhoneDbId,
                AtBusiness = x.AtBusiness,
                //Consent = x.Consent,  //Swetha Made on changes on 24-11-2021
                EntityType = x.EntityType,
                EntityName = x.EntityName,
                Address = x.Address,
                City = x.City,
                State = x.State,
                Pincode = x.Pincode,
                LandSize = x.LandSize,
                TimeStamp = x.TimeStamp,
                Latitude = x.Latitude.ToString("N6"),
                Longitude = x.Longitude.ToString("N6"),
                LocationTaskStatus = x.LocationTaskStatus,
                LocationException = x.LocationException,
                MNC = x.MNC,
                MCC = x.MCC,
                LAC = x.LAC,
                CellId = x.CellId,
                IsProcessed = x.IsProcessed,
                EntityId = x.EntityId,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
                NumberOfContacts = x.NumberOfContacts,
                NumberOfCrops = x.NumberOfCrops,
                NumberOfImages = x.NumberOfImages,
                NumberOfLocations = x.NumberOfLocations,
                UniqueIdType = x.UniqueIdType,
                UniqueId = x.UniqueId,
                TaxId = x.TaxId,
                DerivedLocSource = x.DerivedLocSource,
                DerivedLatitude = x.DerivedLatitude,
                DerivedLongitude = x.DerivedLongitude,

                FatherHusbandName = x.FatherHusbandName,
                TerritoryCode = x.TerritoryCode,
                TerritoryName = x.TerritoryName,
                HQCode = x.HQCode,
                HQName = x.HQName,
                //MajorCrop = x.MajorCrop,
                //LastCrop = x.LastCrop,
                //WaterSource = x.WaterSource,
                //SoilType = x.SoilType,
                //SowingType = x.SowingType,
                //SowingDate = x.SowingDate

            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(sqliteEntityDisplayData);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult ShowSavedEntityContacts(long entityId)
        {
            IEnumerable<SqliteEntityContactData> sqliteEntityContactData = Business.GetSavedSqliteEntityContacts(entityId);

            var sqliteEntityContactDisplayData = sqliteEntityContactData.Select(x => new SqliteEntityContactDisplayData()
            {
                Id = x.Id,
                SqliteEntityId = x.SqliteEntityId,
                Name = x.Name,
                PhoneNumber = x.PhoneNumber,
                IsPrimary = x.IsPrimary
            }).ToList();

            return PartialView("_ShowSavedEntityContacts", sqliteEntityContactDisplayData);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult ShowSavedEntityCrops(long entityId)
        {
            IEnumerable<SqliteEntityCropData> sqliteEntityCropData = Business.GetSavedSqliteEntityCrops(entityId);

            var sqliteEntityCropDisplayData = sqliteEntityCropData.Select(x => new SqliteEntityCropDisplayData()
            {
                Id = x.Id,
                SqliteEntityId = x.SqliteEntityId,
                Name = x.Name
            }).ToList();

            return PartialView("_ShowSavedEntityCrops", sqliteEntityCropDisplayData);
        }

        public ActionResult ShowSavedLeaves(long batchId, string empName)
        {
            IEnumerable<SqliteLeaveData> sqliteLeaveData = Business.GetSavedSqliteLeaves(batchId);

            var sqliteLeaveDisplayData = sqliteLeaveData.Select(x => new SqliteLeaveDisplayData()
            {
                Id = x.Id,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                PhoneDbId = x.PhoneDbId,
                StartDate = x.StartDate,
                EndDate = x.EndDate,
                LeaveType = x.LeaveType,
                LeaveReason = x.LeaveReason,
                Comment = x.Comment,
                IsProcessed = x.IsProcessed,
                DaysCountExcludingHolidays=x.DaysCountExcludingHolidays,
                DaysCount =x.DaysCount,
                LeaveId = x.LeaveId
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(sqliteLeaveDisplayData);
        }

        public ActionResult ShowSavedCancelledLeaves(long batchId, string empName)
        {
            IEnumerable<SqliteCancelledLeaveData> sqliteCancelledLeaveData = Business.GetSavedSqliteCancelledLeaves(batchId);

            var sqliteCancelledLeaveDisplayData = sqliteCancelledLeaveData.Select(x => new SqliteCancelledLeaveDisplayData()
            {
                Id = x.Id,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                LeaveId = x.LeaveId,
                IsProcessed = x.IsProcessed
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(sqliteCancelledLeaveDisplayData);
        }

        public ActionResult ShowSavedWorkFlowData(long batchId, string empName)
        {
            IEnumerable<SqliteEntityWorkFlowData> data = Business.GetSavedSqliteWorkFlowData(batchId);

            var model = data.Select(x => new SqliteWorkFlowDisplayData()
            {
                Id = x.Id,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                EntityId = x.EntityId,
                EntityType = x.EntityType,
                EntityName = x.EntityName,
                AgreementId = x.AgreementId,
                Agreement = x.Agreement,
                EntityWorkFlowDetailId = x.EntityWorkFlowDetailId,
                TypeName = x.TypeName,
                Phase = x.Phase,
                FieldVisitDate = x.FieldVisitDate,
                IsStarted = x.IsStarted,
                Date = x.Date,
                MaterialType = x.MaterialType,
                MaterialQuantity = x.MaterialQuantity,
                GapFillingRequired = x.GapFillingRequired,
                GapFillingSeedQuantity = x.GapFillingSeedQuantity,
                LaborCount = x.LaborCount,
                PercentCompleted = x.PercentCompleted,
                IsProcessed = x.IsProcessed,
                EntityWorkFlowId = x.EntityWorkFlowId,
                DateCreated = x.DateCreated,
                Timestamp = x.Timestamp,
                FollowUpCount = x.FollowUpCount,

                BatchNumber = x.BatchNumber,
                LandSize = x.LandSize,
                DWSEntry = x.DWSEntry,
                ItemCount = x.ItemCount, // Plant Count or Nipping Count
                ItemsUsedCount = x.ItemsUsedCount,
                YieldExpected = x.YieldExpected,
                BagsIssued = x.BagsIssued,
                HarvestDate = x.HarvestDate
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(model);
        }

        public ActionResult ShowSavedAgreements(long batchId, string empName)
        {
            IEnumerable<SqliteAgreementData> sqliteAgreementData = Business.GetSavedAgreements(batchId);

            var model = sqliteAgreementData.Select(x => new SqliteAgreementDisplayData()
            {
                Id = x.Id,
                IsProcessed = x.IsProcessed,
                EntityAgreementId = x.EntityAgreementId,
                IsNewEntity = x.IsNewEntity,

                EntityId = x.EntityId,
                EntityName = x.EntityName,
                ParentReferenceId = x.ParentReferenceId,
                TimeStamp = x.TimeStamp,
                ActivityId = x.ActivityId,

                SeasonName = x.SeasonName,
                TypeName = x.TypeName,
                Acreage = x.Acreage,
                PhoneDbId = x.PhoneDbId,
                TerritoryCode = x.TerritoryCode,
                TerritoryName = x.TerritoryName,
                HQCode = x.HQCode,
                HQName = x.HQName
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(model);
        }

        public ActionResult ShowSavedSurveys(long batchId, string empName)
        {
            IEnumerable<SqliteSurveyData> sqliteSurveyData = Business.GetSavedSurveys(batchId);

            var model = sqliteSurveyData.Select(x => new SqliteSurveyDisplayData()
            {
                Id = x.Id,
                IsProcessed = x.IsProcessed,
                EntitySurveyId = x.EntitySurveyId,
                IsNewEntity = x.IsNewEntity,

                EntityId = x.EntityId,
                EntityName = x.EntityName,
                ParentReferenceId = x.ParentReferenceId,
                TimeStamp = x.TimeStamp,
                ActivityId = x.ActivityId,

                SeasonName = x.SeasonName,
                SowingType = x.SowingType,
                Acreage = x.Acreage,
                SowingDate = x.SowingDate,
                MajorCrop = x.MajorCrop,
                LastCrop = x.LastCrop,
                SoilType = x.SoilType,
                WaterSource = x.WaterSource,

                PhoneDbId = x.PhoneDbId
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(model);
        }

        public ActionResult ShowSavedIssueReturns(long batchId, string empName)
        {
            IEnumerable<SqliteIssueReturnData> sqliteIssueReturnData = Business.GetSavedSqliteIssueReturns(batchId);

            var model = sqliteIssueReturnData.Select(x => new SqliteIssueReturnDisplayData()
            {
                Id = x.Id,
                IsProcessed = x.IsProcessed,
                IssueReturnId = x.IssueReturnId,
                IsNewEntity = x.IsNewEntity,
                IsNewAgreement = x.IsNewAgreement,
                EntityId = x.EntityId,
                EntityName = x.EntityName,
                AgreementId = x.AgreementId,
                Agreement = x.Agreement,
                TranType = x.TranType,
                ItemId = x.ItemId,
                ItemCode = x.ItemCode,
                SlipNumber = x.SlipNumber,
                Acreage = x.Acreage,
                Quantity = x.Quantity,
                TimeStamp = x.TimeStamp,
                ActivityId = x.ActivityId,
                ParentReferenceId = x.ParentReferenceId,
                ItemRate = x.ItemRate
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(model);
        }

        public ActionResult ShowSavedAdvanceRequests(long batchId, string empName)
        {
            IEnumerable<SqliteAdvanceRequestData> sqliteAdvanceRequestData = Business.GetSavedSqliteAdvanceRequests(batchId);

            var model = sqliteAdvanceRequestData.Select(x => new SqliteAdvanceRequestDisplayData()
            {
                Id = x.Id,
                IsProcessed = x.IsProcessed,
                AdvanceRequestId = x.AdvanceRequestId,
                IsNewEntity = x.IsNewEntity,
                IsNewAgreement = x.IsNewAgreement,
                EntityId = x.EntityId,
                EntityName = x.EntityName,
                AgreementId = x.AgreementId,
                Agreement = x.Agreement,
                Amount = x.Amount,
                TimeStamp = x.TimeStamp,
                Notes = x.Notes,
                ActivityId = x.ActivityId,
                ParentReferenceId = x.ParentReferenceId,
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(model);
        }

        public ActionResult ShowSavedTerminateRequests(long batchId, string empName)
        {
            IEnumerable<SqliteTerminateAgreementEx> sqliteData = Business.GetSavedTerminateAgreements(batchId);

            var model = sqliteData.Select(x => new SqliteTerminateAgreementDisplayData()
            {
                Id = x.Id,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                IsProcessed = x.IsProcessed,
                TerminateAgreementId = x.TerminateAgreementId,
                EntityId = x.EntityId,
                EntityName = x.EntityName,
                AgreementId = x.AgreementId,
                Agreement = x.Agreement,
                TimeStamp = x.TimeStamp,
                TypeName = x.TypeName,
                Reason = x.Reason,
                Notes = x.Notes,
                ActivityId = x.ActivityId,
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(model);
        }

        public ActionResult ShowSavedSTRData(long batchId, string empName)
        {
            IEnumerable<DomainEntities.SqliteSTRData> sqliteData = Business.GetSavedSTRData(batchId);

            var model = sqliteData.Select(data => new SqliteSTRDisplayData()
            {
                Id = data.Id,
                BatchId = data.BatchId,
                EmployeeId = data.EmployeeId,
                IsProcessed = data.IsProcessed,
                STRId = data.STRId,
                ImageCount = data.ImageCount,
                DateCreated = data.DateCreated,
                DateUpdated = data.DateUpdated,

                PhoneDbId = data.PhoneDbId,
                STRNumber = data.STRNumber,
                VehicleNumber = data.VehicleNumber,
                DriverName = data.DriverName,
                DriverPhone = data.DriverPhone,
                DWSCount = data.DWSCount,
                BagCount = data.BagCount,
                GrossWeight = data.GrossWeight,
                NetWeight = data.NetWeight,
                StartOdometer = data.StartOdometer,
                EndOdometer = data.EndOdometer,
                STRDate = data.STRDate,
                IsNew = data.IsNew,
                IsTransferred = data.IsTransferred,
                TransfereeName = data.TransfereeName,
                TransfereePhone = data.TransfereePhone,

                TimeStamp = data.TimeStamp,
                ActivityId = data.ActivityId,
                TimeStamp2 = data.TimeStamp2,
                ActivityId2 = data.ActivityId2,
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(model);
        }

        public ActionResult ShowSavedBankDetails(long batchId, string empName)
        {
            IEnumerable<SqliteBankDetailData> sqliteData = Business.GetSavedBankDetails(batchId);

            var model = sqliteData.Select(x => new SqliteBankDetailDisplayData()
            {
                Id = x.Id,
                IsProcessed = x.IsProcessed,
                EntityBankDetailId = x.EntityBankDetailId,
                IsNewEntity = x.IsNewEntity,

                EntityId = x.EntityId,
                EntityName = x.EntityName,
                ParentReferenceId = x.ParentReferenceId,
                TimeStamp = x.TimeStamp,
                ActivityId = x.ActivityId,

                IsSelfAccount = x.IsSelfAccount,
                AccountHolderName = x.AccountHolderName,
                AccountHolderPAN = x.AccountHolderPAN,
                BankName = x.BankName,
                BankAccount = x.BankAccount,
                BankIFSC = x.BankIFSC,
                BankBranch = x.BankBranch,
                ImageCount = x.ImageCount,

                PhoneDbId = x.PhoneDbId
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(model);
        }

        // June 03 2021

        public ActionResult ShowSavedDayPlanTarget(long batchId, string empName)
        {
            IEnumerable<SqliteDayPlanTargetData> sqliteData = Business.GetSavedDayPlanTargetData(batchId);

            var model = sqliteData.Select(x => new SqliteDayPlanTargetDisplayData()
            {
                Id = x.Id,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                PlanTimeStamp = x.PlanTimeStamp,
                TargetSales = x.TargetSales,
                TargetCollection = x.TargetCollection,
                TargetVigoreSales = x.TargetVigoreSales,
                TargetNewDealerAppointment = x.TargetNewDealerAppointment,
                TargetDemoActivity = x.TargetDemoActivity,
                PhoneDbId = x.PhoneDbId,
                DayPlanTargetId = x.DayPlanTargetId,
                IsProcessed = x.IsProcessed,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(model);
        }

        // Oct 18 2021

        public ActionResult ShowSavedFollowUpTask(long batchId, string empName)
        {
            IEnumerable<SqliteTaskData> sqliteData = Business.GetSavedFollowUpTaskData(batchId);

            var model = sqliteData.Select(x => new SqliteFollowUpTaskDisplayData()
            {
                Id = x.Id,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                IsNewEntity = x.IsNewEntity,
                ParentReferenceId = x.ParentReferenceId,
                ProjectName = x.ProjectName,
                Description = x.Description,
                ActivityType = x.ActivityType,
                ClientType = x.ClientType,
                ClientName = x.ClientName,
                ClientCode = x.ClientCode,
                FollowUpStartDate = x.PlannedStartDate,
                FollowUpEndDate = x.PlannedEndDate,
                Comments = x.Comments,
                FollowUpStatus = x.Status,
                NotificationDate = x.NotificationDate,
                TimeStamp = x.TimeStamp,
                PhoneDbId = x.PhoneDbId,
                TaskId = x.TaskId,
                TaskAssignmentId = x.TaskAssignmentId,
                IsProcessed = x.IsProcessed,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(model);
        }

        public ActionResult ShowSavedFollowUpTaskAction(long batchId, string empName)
        {
            IEnumerable<SqliteTaskActionData> sqliteData = Business.GetSavedFollowUpTaskActionData(batchId);

            var model = sqliteData.Select(x => new SqliteFollowUpTaskActionDisplayData()
            {
                Id = x.Id,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                IsNewTask = x.IsNewTask,
                TaskId = x.TaskId,
                ParentReferenceTaskId = x.ParentReferenceTaskId,
                TaskAssignmentId = x.TaskAssignmentId,
                SqliteActionPhoneDbId = x.SqliteActionPhoneDbId,
                Status = x.Status,
                NotificationDate = x.NotificationDate,
                TimeStamp = x.TimeStamp,
                PhoneDbId = x.PhoneDbId,
                TaskActionId = x.TaskActionId,
                IsProcessed = x.IsProcessed,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated,
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(model);
        }

        public ActionResult UnifiedLogSearch()
        {
            List<string> AvailableLogTypes = new List<string>()
            {
                LogType.ErrorLog.ToString(),
                LogType.PhoneBatchProcessLog.ToString(),
                LogType.DataFeedProcessLog.ToString(),
                LogType.ExcelUploadHistory.ToString(),
                LogType.DistanceCalcErrorLog.ToString(),
                LogType.EntityAgreement.ToString()
            };

            if (IsSetupSuperAdmin)
            {
                AvailableLogTypes.Add(LogType.SMSLog.ToString());
                AvailableLogTypes.Add(LogType.SMSJobLog.ToString());
                AvailableLogTypes.Add(LogType.PurgeDataLog.ToString());
            }

            ViewBag.AvailableLogTypes = AvailableLogTypes;

            ViewBag.SearchResultAction = nameof(GetUnifiedLogSearchResult);
            ViewBag.Title = "Search Logs";

            return View();
        }

        public ActionResult GetTableList()
        {
            ICollection<string> AvailableTables = Business.GetTableList();
            ViewBag.AvailableTables = AvailableTables;

            ViewBag.SearchResultAction = nameof(ShowTableSchema);
            ViewBag.Title = "Get Table Data";

            return View();
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult ShowTableSchema(TableDataFilter searchCriteria)
        {
            ViewData["DataType"] = searchCriteria.TableName;
            ICollection<TableSchema> tableSchema = Business.GetTableSchema(searchCriteria.TableName);

            List<string> AvailableOperators = new List<string>()
            {
                OperatorType.EQ.ToString(),
                OperatorType.LE.ToString(),
                OperatorType.LT.ToString(),
                OperatorType.GE.ToString(),
                OperatorType.GT.ToString(),
                OperatorType.NE.ToString()
            };

            ViewBag.AvailableOperators = AvailableOperators;
            ViewBag.TableName = searchCriteria.TableName;

            return PartialView("_ShowTableSchema", tableSchema);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult GetTableData(TableDataQueryModel criteria)
        {
            // if only show query is opted - show query only;
            if (criteria.ShowQuery)
            {
                return PartialView("_CustomText", $"{criteria.GetTranslatedCountQuery()} | {criteria.GetTranslatedSelectQuery()}");
            }

            // Should we show download link
            ViewBag.ShowDownloadLink =
                    Helper.IsFeatureEnabled(FeatureEnum.ExtraAdminOption,
                                Helper.GetAvailableFeatures(CurrentUserStaffCode, IsSuperAdmin));

            ViewBag.TableName = criteria.TableName;

            // show results otherwise;
            try
            {
                if (criteria.GetCountOnly)
                {
                    string q = criteria.GetTranslatedCountQuery();
                    int rowCount = Business.ExcuteRawScalarQuery(q);
                    string msg = $"{q} | {rowCount} row(s)";
                    return PartialView("_CustomText", msg);
                }
                else
                {
                    DataTable dt = Business.ExcuteRawSelectQuery(criteria.GetTranslatedSelectQuery());
                    return PartialView("_ShowTableData", dt);
                }
            }
            catch (Exception ex)
            {
                return PartialView("_CustomText", ex.ToString());
            }
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult GetUnifiedLogSearchResult(UnifiedLogFilter searchCriteria)
        {
            DomainEntities.UnifiedLogFilter criteria = Helper.ParseUnifiedLogSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            if (criteria.LogType.Equals(LogType.ErrorLog.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return ShowErrorLog(criteria.StartItem, criteria.ItemCount, searchCriteria.ProcessFilter);
            }

            if (criteria.LogType.Equals(LogType.PhoneBatchProcessLog.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return ShowSqliteBatchProcessLog(criteria.StartItem, criteria.ItemCount);
            }

            if (criteria.LogType.Equals(LogType.DataFeedProcessLog.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return ShowDatafeedProcessLog(criteria.StartItem, criteria.ItemCount);
            }

            if (criteria.LogType.Equals(LogType.ExcelUploadHistory.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return ExcelUploadHistory(criteria.StartItem, criteria.ItemCount);
            }

            if (criteria.LogType.Equals(LogType.DistanceCalcErrorLog.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return ShowDistanceCalcErrorLog(criteria.StartItem, criteria.ItemCount);
            }

            if (criteria.LogType.Equals(LogType.EntityAgreement.ToString(), StringComparison.OrdinalIgnoreCase))
            {
                return ShowEntityAgreements(criteria.StartItem, criteria.ItemCount);
            }

            if (IsSetupSuperAdmin)
            {
                if (criteria.LogType.Equals(LogType.SMSLog.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return ShowSmsLog(criteria.StartItem, criteria.ItemCount);
                }

                if (criteria.LogType.Equals(LogType.SMSJobLog.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return ShowSmsJobLog(criteria.StartItem, criteria.ItemCount);
                }

                if (criteria.LogType.Equals(LogType.PurgeDataLog.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    return ShowPurgeDataLog(criteria.StartItem, criteria.ItemCount);
                }
            }

            //string msg = "Page is in construction";
            return new EmptyResult();
        }

        private ActionResult ShowErrorLog(int startItem, int itemCount, string processName = "")
        {
            if (startItem <= 0)
            {
                startItem = 1;
            }

            if (itemCount <= 0)
            {
                itemCount = 10;
            }

            IEnumerable<ErrorLog> errorList = Business.GetErrorLogData(startItem, itemCount, processName);
            IEnumerable<ErrorLogModel> errorLogModelList = errorList.Select(x => new ErrorLogModel()
            {
                Id = x.Id,
                Process = x.Process,
                LogText = x.LogText,
                LogSnip = x.LogSnip,
                At = Helper.ConvertUtcTimeToIst(x.At.Value)
            }).ToList();

            return PartialView("ShowErrorLog", errorLogModelList);
        }

        private DomainEntities.SearchCriteria ParseSearchCriteria(SearchCriteria searchCriteria)
        {
            var securityContext = Helper.GetSecurityContextUser(HttpContext.User);
            DomainEntities.SearchCriteria s = new DomainEntities.SearchCriteria()
            {
                ApplyZoneFilter = false,
                ApplyAreaFilter = false,
                ApplyTerritoryFilter = false,
                ApplyHQFilter = false,
                IsSuperAdmin = securityContext.Item1,
                CurrentUserStaffCode = securityContext.Item2
            };

            if (searchCriteria == null)
            {
                return s;
            }

            s.ApplyZoneFilter = IsValidSearchCriteria(searchCriteria.Zone);
            s.Zone = searchCriteria.Zone;

            s.ApplyAreaFilter = IsValidSearchCriteria(searchCriteria.Area);
            s.Area = searchCriteria.Area;

            s.ApplyTerritoryFilter = IsValidSearchCriteria(searchCriteria.Territory);
            s.Territory = searchCriteria.Territory;

            s.ApplyHQFilter = IsValidSearchCriteria(searchCriteria.HQ);
            s.HQ = searchCriteria.HQ;

            s.ReportType = searchCriteria.ReportType;
            return s;
        }

        private bool IsValidSearchCriteria(string criteria)
        {
            if (String.IsNullOrEmpty(criteria) || criteria.Equals("All", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }
            return true;
        }

        private ActionResult GetCustomers(DomainEntities.SearchCriteria s)
        {
            DomainEntities.CustomersFilter customersFilter = new DomainEntities.CustomersFilter();
            IEnumerable<DownloadCustomerExtend> customers = Business.GetCustomers(customersFilter);

            // apply filter here
            if (s.ApplyZoneFilter || s.ApplyAreaFilter || s.ApplyTerritoryFilter || s.ApplyHQFilter)
            {
                List<string> selectedHQCodes = null;
                if (s.ApplyHQFilter)
                {
                    selectedHQCodes = new List<string>() { s.HQ };
                }
                else
                {
                    IEnumerable<OfficeHierarchy> officeHierarchy = this.GetOfficeHierarchy();
                    if (s.ApplyTerritoryFilter)
                    {
                        officeHierarchy = officeHierarchy
                            .Where(x => x.TerritoryCode == s.Territory);
                        //.Select(x => x.HQCode).ToList();
                    }
                    if (s.ApplyAreaFilter)
                    {
                        officeHierarchy = officeHierarchy
                            .Where(x => x.AreaCode == s.Area);
                        //.Select(x => x.HQCode).ToList();
                    }
                    if (s.ApplyZoneFilter)
                    {
                        officeHierarchy = officeHierarchy
                            .Where(x => x.ZoneCode == s.Zone);
                        //.Select(x => x.HQCode).ToList();
                    }

                    selectedHQCodes = officeHierarchy.Select(x => x.HQCode).ToList();
                }

                customers = customers.Where(x => selectedHQCodes.Any(y => y == x.HQCode));
            }

            var model = customers.Select(x => new CustomerModel()
            {
                Code = x.Code,
                CreditLimit = x.CreditLimit,
                LongOutstanding = x.LongOutstanding,
                Name = x.Name,
                Outstanding = x.Outstanding,
                PhoneNumber = x.PhoneNumber,
                Type = x.Type,
                HQCode = x.HQCode,
                Target = x.Target,
                Sales = x.Sales,
                Payment = x.Payment,
                Active = x.Active
            }).ToList();

            return PartialView("_Customer", model);
        }

        [HttpGet]
        [SetupSuperAdmin]
        public ActionResult Rights()
        {
            ICollection<FeatureControl> rights = Business.GetVirtualSuperAdminWithRights();
            var model = rights.Select(source =>
            {
                FeatureControlModel target = new FeatureControlModel();
                Utils.CopyProperties(source, target);
                return target;
            }).ToList();

            return View(model);
        }

        [HttpGet]
        [SetupSuperAdmin]
        public ActionResult Create()
        {
            ViewBag.RegistrationAllowed = Business.DashboardRegistrationAllowed();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SetupSuperAdmin]
        public ActionResult Register(RegisterSuperAdminViewModel model)
        {
            if (Business.DashboardRegistrationAllowed() == false)
            {
                return RedirectToAction("Create");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            bool userRegisterStatus = Helper.CreateUser(model.UserName, model.Password, false, new string[] { "Admin", "Manager" });

            DateTime autoDisableUtcTimeForUser = DateTime.MinValue;
            if (model.Immortal == false)
            {
                autoDisableUtcTimeForUser = DateTime.UtcNow.AddMinutes(CRMUtilities.Utils.LifespanInMinutesForOnFlySuperAdminUser());
            }
            else
            {
                autoDisableUtcTimeForUser = DateTime.MaxValue;
            }

            if (userRegisterStatus)
            {
                // set Lockout end date which is used in a/c deletion automatically
                Helper.SetAutoDeletionTimeForUser(model.UserName, autoDisableUtcTimeForUser);

                // Create record in FeatureControl table
                Business.CreateFeatureControl(model.UserName);
            }

            if (userRegisterStatus)
            {
                string successMessage = $"Super Admin user {model.UserName} created with life until {Helper.ConvertUtcTimeToIst(autoDisableUtcTimeForUser).ToString("yyyy-MM-dd hh:mm:ss")} IST";
                Business.LogError(nameof(SuperAdminController), successMessage, "");
                return RedirectToAction("Rights");
            }
            else
            {
                ViewBag.Status = "New User Could not be Created";
                return View();
            }
        }

        [HttpGet]
        public ActionResult Impersonate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Impersonate(ImpersonateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            //ensure that user exists and is active
            if (!Business.IsUserAllowed(model.UserName))
            {
                ModelState.AddModelError("", "User must be an active user.");
                return View();
            }

            try
            {
                // https://stackoverflow.com/questions/28110934/asp-net-mvc-identity-login-without-password
                //
                var user = await UserManager.FindByNameAsync(model.UserName);
                if (user == null)
                {
                    ModelState.AddModelError("", "User has not signed up on web.");
                    return View();
                }

                Business.LogError(nameof(Impersonate), $"Impersonating {CurrentUserStaffCode} to {model.UserName}", ">");
                SignoutCurrentUser();
                await SignInManager.SignInAsync(user, true, true);
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(Impersonate), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return View();
            }
        }

        [HttpGet]
        public ActionResult ShowAspNetUser()
        {
            ICollection<AspNetUser> aspnetUsers = Business.GetAspNetUserData();
            return View(aspnetUsers);
        }

        [SetupSuperAdmin]
        public ActionResult SmsSchedule()
        {
            SMSScheduleModel model = new SMSScheduleModel()
            {
                Holidays = Business.GetTenantHolidays(),
                WorkDays = Business.GetTenantWorkDay(),
                SmsSchedule = Business.GetTenantSmsSchedule(),
                SmsTypes = Business.GetSmsTypes()
            };
            return View(model);
        }

        [SetupSuperAdmin]
        private ActionResult ShowSmsLog(int startItem, int itemCount)
        {
            if (startItem <= 0)
            {
                startItem = 1;
            }

            if (itemCount <= 0)
            {
                itemCount = 10;
            }

            IEnumerable<SmsLog> smsLog = Business.GetSmsLogData(startItem, itemCount);

            return PartialView("ShowSmsLog", smsLog);
        }

        /// <summary>
        /// Show stats as will be sent via SMS
        /// </summary>
        /// <param name="currentDateTime"></param>
        /// <returns></returns>
        public ActionResult PresentActivePeopleStats(DateTime todayDate, int rollupType = 2)
        {
            long tenantId = Utils.SiteConfigData.TenantId;

            ExecAppRollupEnum rollUpEnum = Business.ConvertToRollUpEnum(rollupType);

            IEnumerable<SmsSummaryDataModel> smsSummaryDataModel =
                Helper.GetModelForPeopleInField(tenantId, todayDate, rollUpEnum);

            return View(smsSummaryDataModel);
        }

        public ActionResult ShowAppVersions()
        {
            IEnumerable<AppVersion> appVersions = Business.GetAllReleasedAppVersions();
            return View(appVersions);
        }

        public ActionResult RefreshConfig()
        {
            string url = Helper.GetCurrentSiteUrl(Request.Url);
            Helper.RefreshConfiguration(url);

            Utils.SiteConfigData = HttpContext.Cache[Helper.ConfigKeyForSiteConfigurationData] as SiteConfigData;
            Utils.GlobalConfiguration = HttpContext.Cache[Helper.ConfigKeyForGlobalConfigurationData] as ICollection<GlobalConfigData>;
            Utils.DatabaseServerConfiguration = HttpContext.Cache[Helper.ConfigKeyForDatabaseServerData] as DBServer;

            Helper.CacheDbConnectionString();

            Utils.DBConnectionString = HttpContext.Cache[Helper.ConfigKeyForDbConnection] as String;
            Utils.EFConnectionString = HttpContext.Cache[Helper.ConfigKeyForEfConnection] as String;

            if (Helper.IsValidConfiguration())
            {
                return RedirectToAction("ShowConfig");
            }
            else
            {
                return Content("An error occured during refresh.");
            }
        }

        public ActionResult CheckS3Bucket()
        {
            Dictionary<long, string> s3Buckets = Business.GetS3Buckets();
            return View(s3Buckets);
        }

        [HttpGet]
        [AjaxOnly]
        public ActionResult S3BucketList(long internalBucketId)
        {
            string bucketName = Business.GetActualS3BucketName(internalBucketId);

            ICollection<string> s3Items = null;
            if (String.IsNullOrEmpty(bucketName) == false)
            {
                s3Items = Business.S3BucketEntries(bucketName);
            }

            return PartialView("_S3BucketList", s3Items);
        }

        public ActionResult LocalImageList()
        {
            ICollection<string> localImages = Business.LocalImageEntries();
            return View(localImages);
        }

        // Aug 14 2020 - Not Used
        //public ActionResult UploadImagesToS3()
        //{
        //    long tenantId = Utils.SiteConfigData.TenantId;
        //    Business.ProcessImageTransfer(tenantId, true, 1000);
        //    return RedirectToAction("S3BucketList");
        //}

        public ActionResult ConfigEmployee()
        {
            return View();
        }

        [AjaxOnly]
        [HttpGet]
        [SetupSuperAdmin]
        public ActionResult ConfigEmployeeData(ConfigEmployeeModel inputModel)
        {
            EmployeeRecord er = Business.GetTenantEmployee(inputModel?.EmployeeCode ?? "");

            if (er == null)
            {
                return Content("Employee Record not found");
            }

            var model = new EmployeeRecordModel()
            {
                EmployeeId = er.EmployeeId,
                ManagerId = er.ManagerId,
                Name = er.Name,
                TenantId = er.TenantId,
                EmployeeCode = er.EmployeeCode,
                IsActive = er.IsActive,
                IMEI = er.IMEI,
                AutoUploadFromPhone = er.AutoUploadFromPhone,
                ActivityPageName = er.ActivityPageName ?? "",
                LocationFromType = er.LocationFromType ?? "",
                EnhancedDebugEnabled = er.EnhancedDebugEnabled,
                TestFeatureEnabled = er.TestFeatureEnabled,
                VoiceFeatureEnabled = er.VoiceFeatureEnabled,
                ExecAppDetailLevel = er.ExecAppDetailLevel
            };

            SetActivityPageViewBag();

            return PartialView("_ConfigEmployeeData", model);
        }

        public void SetActivityPageViewBag()
        {
            string[] ActivityPages = Utils.ActivityPages.Split('|');

            ViewBag.ActivityPageType = ActivityPages;
        }

        [AjaxOnly]
        [HttpGet]
        [SetupSuperAdmin]
        public ActionResult SaveEmployeeConfigData(ConfigEmployeeModel inputModel)
        {
            if (inputModel == null)
            {
                Business.LogError(nameof(SaveEmployeeConfigData), "Input model is null");

                return new EmptyResult();
            }

            DomainEntities.ConfigEmployeeModel saveModel = new DomainEntities.ConfigEmployeeModel()
            {
                ActivityPageName = Utils.TruncateString(inputModel.ActivityPageName, 50),
                AutoUploadFromPhone = inputModel.AutoUploadFromPhone,
                EmployeeId = inputModel.EmployeeId,
                LocationFromType = inputModel.LocationFromType,
                EnhancedDebugEnabled = inputModel.EnhancedDebugEnabled,
                TestFeatureEnabled = inputModel.TestFeatureEnabled,
                VoiceFeatureEnabled = inputModel.VoiceFeatureEnabled,
                ExecAppDetailLevel = inputModel.ExecAppDetailLevel
            };

            try
            {
                bool status = Business.SaveEmployeeConfigData(saveModel);
                if (!status)
                {
                    Business.LogError(nameof(SaveEmployeeConfigData), "Save failed.");
                }
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(SaveEmployeeConfigData), ex);
            }

            return new EmptyResult();
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult DeleteEmployeeData(ConfigEmployeeModel inputModel)
        {
            if ((inputModel?.EmployeeId ?? 0) <= 0)
            {
                Business.LogError(nameof(DeleteEmployeeData), "Employee Id is 0");
                return new EmptyResult();
            }

            Business.LogError(nameof(DeleteEmployeeData), $"Delete request for EmployeeId = {inputModel.EmployeeId}");

            try
            {
                RemovePortalAccess(inputModel.EmployeeId);
                Business.ClearData(inputModel.EmployeeId);
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(SaveEmployeeConfigData), ex);
            }

            return new EmptyResult();
        }

        [SetupSuperAdmin]
        public ActionResult ShowCodeTableEntries()
        {
            var model = new DomainEntities.ConfigCodeTable();

            ViewBag.TenantList = Business.GetTenants();
            model.UniqueCodeTypes = Business.GetUniqueCodeTypes();

            return View(model);
        }

        [AjaxOnly]
        [HttpGet]
        [SetupSuperAdmin]
        public ActionResult ShowCodeTableEntries(ConfigCodeTableModel input)
        {
            DomainEntities.ConfigCodeTable searchCriteria = Helper.ParseCodeTableSearchCriteria(input);

            IEnumerable<ConfigCodeTable> result = Business.GetCodeTableData(searchCriteria);

            var model = result.Select(x => new ConfigCodeTableModel()
            {
                Id = x.Id,
                CodeType = x.CodeType,
                CodeValue = x.CodeValue,
                DisplaySequence = x.DisplaySequence,
                IsActive = x.IsActive,
                CodeName = x.CodeName,
                TenantId = x.TenantId,
            }).ToList();

            ViewBag.TenantId = input.TenantId;

            return PartialView("_ShowCodeTableEntries", model);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult EditCodeTableEntry(long codeTableId)
        {
            ConfigCodeTable result = Business.GetSingleCodeTableData(codeTableId);

            if (result == null)
            {
                return Content("Code Table record not found");
            }

            var model = new ConfigCodeTableModel()
            {
                Id = result.Id,
                CodeType = result.CodeType,
                CodeValue = result.CodeValue,
                DisplaySequence = result.DisplaySequence,
                IsActive = result.IsActive,
                CodeName = result.CodeName,
                TenantId = result.TenantId,
            };

            model.UniqueCodeTypes = Business.GetUniqueCodeTypes();
            SetCodeTableViewBag(result.TenantId);

            return PartialView("_EditCodeTableEntry", model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult EditCodeTableEntry(ConfigCodeTableModel model)
        {
            if (!ModelState.IsValid)
            {
                model.UniqueCodeTypes = Business.GetUniqueCodeTypes();
                SetCodeTableViewBag(model.TenantId);
                return PartialView("_EditCodeTableEntry", model);
            }

            ConfigCodeTable codeTableRec = new ConfigCodeTable()
            {
                Id = model.Id,
                CodeType = model.CodeType,
                CodeName = (model.CodeType.Equals("ActivityType") ? model.ddCodeName : model.CodeName ?? "").Trim(),
                CodeValue = model.CodeValue,
                DisplaySequence = model.DisplaySequence,
                IsActive = model.IsActive
            };

            try
            {
                Business.SaveCodeTableData(codeTableRec);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditCodeTableEntry), ex);
                ModelState.AddModelError("", ex.Message);
                model.UniqueCodeTypes = Business.GetUniqueCodeTypes();
                SetCodeTableViewBag(model.TenantId);
                return PartialView("_EditCodeTableEntry", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [SetupSuperAdmin]
        public ActionResult AddCodeTableEntry(long tenantId)
        {
            var model = new ConfigCodeTableModel();

            model.UniqueCodeTypes = Business.GetUniqueCodeTypes();
            SetCodeTableViewBag(tenantId);
            return PartialView("_AddCodeTableEntry", model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult AddCodeTableEntry(ConfigCodeTableModel model)
        {
            if (!ModelState.IsValid)
            {
                SetCodeTableViewBag(model.TenantId);
                model.UniqueCodeTypes = Business.GetUniqueCodeTypes();
                return PartialView("_AddCodeTableEntry", model);
            }

            ConfigCodeTable codeTableRec = new ConfigCodeTable()
            {
                CodeType = model.CodeType,
                CodeName = (model.CodeType.Equals("ActivityType") ? model.ddCodeName : model.CodeName ?? "").Trim(),
                CodeValue = model.CodeValue,
                DisplaySequence = model.DisplaySequence,
                IsActive = model.IsActive,
                TenantId = model.TenantId,
            };

            try
            {
                Business.CreateCodeTableData(codeTableRec);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AddCodeTableEntry), ex);
                ModelState.AddModelError("", ex.Message);
                model.UniqueCodeTypes = Business.GetUniqueCodeTypes();
                SetCodeTableViewBag(model.TenantId);
                return PartialView("_AddCodeTableEntry", model);
            }
        }

        private void SetCodeTableViewBag(long tenantID)
        {
            ViewBag.CustomerTypes = Business.GetCodeTable("CustomerType", tenantID);
        }

        public ActionResult ShowFAQ()
        {
            return View();
        }

        public ActionResult ShowExecAppIMEI()
        {
            IEnumerable<ExecAppImei> model = Business.GetAllExecAppImei();
            return View(model);
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult AddExecAppImei()
        {
            ExecAppImeiModel model = new ExecAppImeiModel()
            {
                EffectiveDate = DateTime.UtcNow,
                ExpiryDate = DateTime.MaxValue,
                ExecAppDetailLevel = 2
            };
            return PartialView(model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult AddExecAppImei(ExecAppImeiModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            if (model.ExpiryDate < model.EffectiveDate)
            {
                ModelState.AddModelError("", "End Date must be after start date.");
                return PartialView(model);
            }

            if (model.ExecAppDetailLevel < 1 || model.ExecAppDetailLevel > 4)
            {
                ModelState.AddModelError("", "Invalid Detail Level specified.");
                return PartialView(model);
            }

            // check if IMEI already exist
            IEnumerable<ExecAppImei> allExistingImei = Business.GetAllExecAppImei();
            if (allExistingImei.Any(x => x.IMEINumber.Equals(model.IMEINumber, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("", "Imei number already exist.");
                return PartialView(model);
            }

            ExecAppImei rec = new ExecAppImei()
            {
                Id = 0,
                IMEINumber = model.IMEINumber,
                Comment = model.Comment,
                EffectiveDate = model.EffectiveDate,
                ExpiryDate = model.ExpiryDate,
                IsSupportPerson = model.IsSupportPerson,
                EnableLog = model.EnableLog,
                ExecAppDetailLevel = model.ExecAppDetailLevel
            };

            try
            {
                Business.AddExecAppImeiRec(rec);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(SuperAdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView(model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult EditExecAppImei(long imeiRecId)
        {
            ExecAppImei rec = Business.GetSingleExecAppImei(imeiRecId);

            ExecAppImeiModel model = new ExecAppImeiModel()
            {
                Id = imeiRecId
            };

            if (rec != null)
            {
                model.Comment = rec.Comment;
                model.EffectiveDate = rec.EffectiveDate;
                model.ExpiryDate = rec.ExpiryDate;
                model.IMEINumber = rec.IMEINumber;
                model.IsSupportPerson = rec.IsSupportPerson;
                model.EnableLog = rec.EnableLog;
                model.ExecAppDetailLevel = rec.ExecAppDetailLevel;
            }

            return PartialView(model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult EditExecAppImei(ExecAppImeiModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            if (model.ExpiryDate < model.EffectiveDate)
            {
                ModelState.AddModelError("", "End Date must be after start date.");
                return PartialView(model);
            }

            if (model.ExecAppDetailLevel < 1 || model.ExecAppDetailLevel > 4)
            {
                ModelState.AddModelError("", "Invalid Detail Level specified.");
                return PartialView(model);
            }

            ExecAppImei rec = new ExecAppImei()
            {
                Id = model.Id,
                IMEINumber = model.IMEINumber,
                Comment = model.Comment,
                EffectiveDate = model.EffectiveDate,
                ExpiryDate = model.ExpiryDate,
                IsSupportPerson = model.IsSupportPerson,
                EnableLog = model.EnableLog,
                ExecAppDetailLevel = model.ExecAppDetailLevel
            };

            try
            {
                Business.SaveExecAppImeiRec(rec);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(SuperAdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView(model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [SetupSuperAdmin]
        public ActionResult EditRights(string userName)
        {
            FeatureControl fc = Business.GetVirtualSuperAdminWithRights(userName);
            FeatureControlModel model = new FeatureControlModel()
            {
                UserName = userName
            };

            if (fc != null)
            {
                Utils.CopyProperties(fc, model);
            }

            return PartialView(model);
        }

        [AjaxOnly]
        [HttpPost]
        [SetupSuperAdmin]
        public ActionResult EditRights(FeatureControlModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            // ensure that security context user is valid
            if (String.IsNullOrEmpty(model.SecurityContextUser) == false)
            {
                var rec = Business.GetSalesPerson(model.SecurityContextUser);
                //if ((rec?.IsActive ?? false) == false)
                // don't check for user's active status, as we may want to give
                // security context of a user who is not allowed to log in on web.
                if (rec == null)
                {
                    ModelState.AddModelError("", "Invalid user given as security context.");
                    return PartialView(model);
                }
            }
            else
            {
                model.SecurityContextUser = "";
            }

            FeatureControl fc = new FeatureControl();
            Utils.CopyProperties(model, fc);

            try
            {
                Business.SaveVirtualSuperAdminRights(fc);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(SuperAdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView(model);
            }
        }

        /// <summary>
        /// Delete virtual super admin
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        [SetupSuperAdmin]
        public ActionResult DeleteUser(string userName)
        {
            ICollection<FeatureControl> rights = Business.GetVirtualSuperAdminWithRights();
            if (rights?.Any(x => x.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)) ?? false)
            {
                Business.DeleteUserFeatureAccess(userName);
                Helper.DeletePortalLogin(userName);
            }

            return RedirectToAction("Rights");
        }

        public ActionResult DeleteExecAppImei(long execAppImeiId)
        {
            Business.DeleteExecAppImei(execAppImeiId);

            return RedirectToAction("ShowExecAppIMEI");
        }

        /// <summary>
        /// Dangerous method - deletes super admin user - user who is also
        /// logged in;  After this, the web server will have to be restarted
        /// at that time the super admin user will be recreated as per rights
        /// defined in web.config.
        /// </summary>
        /// <returns></returns>
        [SetupSuperAdmin]
        public ActionResult Delete()
        {
            Helper.DeletePortalLogin(Utils.SiteConfigData.SuperAdminUserName);
            return RedirectToAction("Index");
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult AddAppVersion()
        {
            AppVersionModel model = new AppVersionModel()
            {
                EffectiveDate = DateTime.UtcNow,
                ExpiryDate = DateTime.MaxValue
            };
            return PartialView(model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult AddAppVersion(AppVersionModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            if (model.ExpiryDate < model.EffectiveDate)
            {
                ModelState.AddModelError("", "End Date must be after start date.");
                return PartialView(model);
            }

            // check if IMEI already exist
            IEnumerable<AppVersion> existingAppVersions = Business.GetAllReleasedAppVersions();
            if (existingAppVersions.Any(x => x.Version.Equals(model.Version, StringComparison.OrdinalIgnoreCase)))
            {
                ModelState.AddModelError("", "Version number already exist.");
                return PartialView(model);
            }

            AppVersion rec = new AppVersion()
            {
                Id = 0,
                Version = model.Version,
                Comment = model.Comment,
                EffectiveDate = model.EffectiveDate,
                ExpiryDate = model.ExpiryDate
            };

            try
            {
                Business.AddAppVersion(rec);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(SuperAdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView(model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        public ActionResult EditAppVersion(long recId)
        {
            IEnumerable<AppVersion> existingAppVersions = Business.GetAllReleasedAppVersions();
            AppVersion rec = existingAppVersions.FirstOrDefault(x => x.Id == recId);

            AppVersionModel model = new AppVersionModel()
            {
                Id = recId
            };

            if (rec != null)
            {
                model.Comment = rec.Comment;
                model.EffectiveDate = rec.EffectiveDate;
                model.ExpiryDate = rec.ExpiryDate;
                model.Version = rec.Version;
            }

            return PartialView(model);
        }

        [AjaxOnly]
        [HttpPost]
        public ActionResult EditAppVersion(AppVersionModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView(model);
            }

            AppVersion rec = new AppVersion()
            {
                Id = model.Id,
                Version = model.Version,
                Comment = model.Comment,
                EffectiveDate = model.EffectiveDate,
                ExpiryDate = model.ExpiryDate
            };

            try
            {
                Business.SaveAppVersion(rec);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(SuperAdminController), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView(model);
            }
        }

        public ActionResult DeleteAppVersion(long recId)
        {
            Business.DeleteAppVersion(recId);

            return RedirectToAction("ShowAppVersions");
        }

        [HttpGet]
        public EmptyResult OrderImageData(long Id, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.OrderImageData(Id, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        [HttpGet]
        public EmptyResult SqliteEntityImageData(long Id, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.SqliteEntityImageData(Id, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        [HttpGet]
        public EmptyResult SqliteBankDetailImageData(long Id, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.SqliteBankDetailImageData(Id, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        [HttpGet]
        public EmptyResult SqliteSTRImageData(long Id, int imageItem)
        {
            this.Response.ContentType = "image/jpeg";

            byte[] imageBytes = Business.SqliteSTRImageData(Id, imageItem);
            this.Response.BinaryWrite(imageBytes);
            return new EmptyResult();
        }

        public ActionResult UploadBatchJson()
        {
            var tenantRecord = Business.GetTenantRecord(Utils.SiteConfigData.TenantId);
            ViewBag.TenantName = tenantRecord?.Name ?? "";

            return View();
        }

        [AjaxOnly]
        public JsonResult ProcessBatchJson(string inputData)
        {
            DataSyncResponse response = null;
            if (inputData == null)
            {
                response = new DataSyncResponse()
                {
                    BatchId = -1,
                    Content = "input is null",
                    StatusCode = HttpStatusCode.BadRequest,
                    EraseData = false,
                    DateTimeUtc = DateTime.UtcNow
                };
            }
            else
            {
                SqliteData sqliteDataObject = JsonConvert.DeserializeObject<SqliteData>(inputData);

                var controller = new DataSync2Controller();
                response = controller.DataSync(sqliteDataObject);
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        //Author:Pankaj K; Date:25/04/2021; Purpose:Upload Dealer Questionnaire;
        public ActionResult UploadQuestionnaire()
        {
            var tenantRecord = Business.GetTenantRecord(Utils.SiteConfigData.TenantId);
            ViewBag.TenantName = tenantRecord?.Name ?? "";

            return View();
        }

        /// <summary>
        ///  Author:Pankaj K; Date:25/04/2021; Purpose:Upload Dealer Questionnaire;
        /// </summary>
        /// <param name="questionPaperData"></param>
        /// <returns></returns>
        [AjaxOnly]
        [HttpPost]
        public JsonResult SaveQuestionnaire(string questionPaperData)
        {
            MessageOnDemandResponse response = new MessageOnDemandResponse();

            try
            {
                QuestionPaper inputObject = new QuestionPaper();
                using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(questionPaperData)))
                {
                    var ser = new DataContractJsonSerializer(typeof(QuestionPaper));
                    inputObject = ser.ReadObject(ms) as QuestionPaper;
                };

                response.Status = Business.SaveQuestionnaire(inputObject);
                response.Message = response.Status ? "" : "An error occured while saving questionnaire - please try again.";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                StringBuilder sb = new StringBuilder();
                while (e != null)
                {
                    sb.AppendLine(e.Message);
                    e = e.InnerException;
                }
                response.Status = false;
                response.Message = $"Requested action could not be performed. Please try again. {sb.ToString()}";
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// Author: Rajesh; Date: 13-07-2021; Purpose: Display Questionnaire Details
        /// </summary>
        /// <param name="batchId"></param>
        /// <param name="empName"></param>
        /// <returns></returns>
        public ActionResult ShowSavedQuestionnaire(long batchId, string empName)
        {
            IEnumerable<SqliteDomainQuestionnaireData> sqliteData = Business.GetSavedQuestionnaireData(batchId);

            var model = sqliteData.Select(x => new SqliteQuestionnaireDisplayData()
            {
                Id = x.Id,
                BatchId = x.BatchId,
                EmployeeId = x.EmployeeId,
                CustomerCode = x.CustomerCode,
                EntityName = x.EntityName,
                SqliteQuestionPaperName = x.SqliteQuestionPaperName,
                PhoneDbId = x.PhoneDbId,
                ActivityId = x.ActivityId,
                IsProcessed = x.IsProcessed,
                DateCreated = x.DateCreated,
                DateUpdated = x.DateUpdated
            }).ToList();

            ViewBag.BatchId = batchId;
            ViewBag.EmpName = empName;

            return View(model);
        }
    }
}