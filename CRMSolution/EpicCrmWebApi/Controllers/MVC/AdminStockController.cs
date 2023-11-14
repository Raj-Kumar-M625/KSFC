using BusinessLayer;
using CRMUtilities;
using DomainEntities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
//using System.Web.Http;
using System.Web.Mvc;
using System.Web.UI.WebControls;

namespace EpicCrmWebApi
{
    [Authorize(Roles = "Admin")]
    public partial class AdminController : BaseDashboardController
    {
        // GET: Admin Page
        [CheckRightsAuthorize(Feature = FeatureEnum.StockReceive)]
        public ActionResult ReceiveStock()
        {
            ViewBag.Title = "Receive Stock";
            ViewBag.CalledFrom = StockStatus.Received.ToString();

            return ReceiveOrConfirmStockInput();
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(FeatureList = new FeatureEnum[] { FeatureEnum.StockReceive, FeatureEnum.StockReceiveConfirm })]
        public ActionResult GetSearchReceiveStock(StockFilter searchCriteria)
        {
            DomainEntities.StockFilter criteria = Helper.ParseStockSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<StockInputTag> items = Business.GetStockInputTags(criteria);

            ICollection<StockInputTagModel> model = items.Select(x => CreateStockInputTagModel(x)).ToList();

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            // this action is called from Receive and Confirm screens.
            // on results, we want to show Edit and Approve icons appropriately
            ViewBag.HasEditPermission = IsStockInputEditAllowed &&
                        StockStatus.Received.ToString().Equals(searchCriteria.CalledFrom, StringComparison.OrdinalIgnoreCase);

            ViewBag.HasConfirmPermission = IsStockInputConfirmAllowed &&
                        StockStatus.ReceiveApproved.ToString().Equals(searchCriteria.CalledFrom, StringComparison.OrdinalIgnoreCase);

            return PartialView("StockReceive/_ListTags", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(FeatureList = new FeatureEnum[] { FeatureEnum.StockReceive, FeatureEnum.StockReceiveConfirm })]
        public ActionResult GetDetailedSearchReceiveStockForDownload(StockFilter searchCriteria)
        {
            // List tags with items for detail download.
            DomainEntities.StockFilter criteria = Helper.ParseStockSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<StockInputTag> tags = Business.GetStockInputTags(criteria);
            ICollection<DomainEntities.StockInput> items = Business.GetStockInput(tags);

            ICollection<StockInputTagModel> model = tags.Select(x => CreateStockInputTagModel(x)).ToList();
            ICollection<StockInputModel> modelItems = items.Select(x => CreateStockInputModel(x)).ToList();

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            ViewBag.ModelItems = modelItems;

            return PartialView("StockReceive/_ListDetailTags", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(FeatureList = new FeatureEnum[] { FeatureEnum.StockReceive, FeatureEnum.StockReceiveConfirm })]
        public ActionResult AddEditStockInputTag(long stockInputTagId)
        {
            DomainEntities.StockInputTag stockInputTagRecord = null;

            if (!IsStockInputEditAllowed)
            {
                return PartialView("_CustomError", "The requested action is not available.");
            }

            if (stockInputTagId <= 0)
            {
                stockInputTagRecord = new StockInputTag()
                {
                    ReceiveDate = Helper.GetCurrentIstDateTime(),
                    VendorBillDate = Helper.GetCurrentIstDateTime(),
                    Status = StockStatus.Received.ToString()
                };
            }
            else
            {
                stockInputTagRecord = Helper.GetSingleStockInputTag(stockInputTagId);
                if (stockInputTagRecord == null)
                {
                    return PartialView("_CustomError", "The requested record does not exist.");
                }
            }

            StockInputTagModel model = CreateStockInputTagModel(stockInputTagRecord);

            // set vendors data
            ViewBag.Vendors = Business.GetVendors();

            return PartialView("StockReceive/_AddEditTag", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(FeatureList = new FeatureEnum[] { FeatureEnum.StockReceive, FeatureEnum.StockReceiveConfirm })]
        public ActionResult AddEditStockInputTag(StockInputTagModel model)
        {
            string viewPath = "StockReceive/_AddEditTag";
            if (!IsStockInputEditAllowed)
            {
                return PartialView("_CustomError", "The requested action is not available.");
            }

            ViewBag.Vendors = Business.GetVendors();

            if (!ModelState.IsValid)
            {
                return PartialView(viewPath, model);
            }

            StockInputTag tagRecord =
                (model.Id > 0) ? Helper.GetSingleStockInputTag(model.Id) : new StockInputTag();

            if (model.Id <= 0)
            {
                tagRecord.GRNNumber = Business.GetNextGRNNumber();
                tagRecord.Status = StockStatus.Received.ToString();
                tagRecord.StaffCode = "";
                if (String.IsNullOrEmpty(tagRecord.GRNNumber))
                {
                    ModelState.AddModelError("", "An error occured while generating GRN #. Please refresh the page and try again.");
                    return PartialView(viewPath, model);
                }
            }

            tagRecord.ReceiveDate = model.ReceiveDate;
            tagRecord.VendorName = model.VendorName;
            tagRecord.VendorBillNo = model.VendorBillNo;
            tagRecord.VendorBillDate = model.VendorBillDate;
            tagRecord.TotalItemCount = model.TotalItemCount;
            tagRecord.TotalAmount = model.TotalAmount;
            tagRecord.Notes = Utils.TruncateString(model.Notes, 100);
            tagRecord.ZoneCode = Utils.TruncateString(model.ZoneCode, 50);
            tagRecord.AreaCode = Utils.TruncateString(model.AreaCode, 50);
            tagRecord.TerritoryCode = Utils.TruncateString(model.TerritoryCode, 50);
            tagRecord.HQCode = Utils.TruncateString(model.HQCode, 50);

            tagRecord.CyclicCount = model.CyclicCount;
            tagRecord.CurrentUser = CurrentUserStaffCode;

            try
            {
                DBSaveStatus status = Business.SaveStockInputTagData(tagRecord);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView(viewPath, model);
                }
                return PartialView("ConfirmMessage", $" (GRN # {tagRecord.GRNNumber})");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AddEditStockInputTag), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView(viewPath, model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(FeatureList = new FeatureEnum[] { FeatureEnum.StockReceive, FeatureEnum.StockReceiveConfirm })]
        public ActionResult GetStockInputItems(long Id, string rowId, string parentRowId, string calledFrom) // here it is StockInputTag Id
        {
            StockInputTag tagRecord = Helper.GetSingleStockInputTag(Id);

            ICollection<DomainEntities.StockInput> items = Business.GetStockInput(Id);

            ICollection<StockInputModel> model = items.Select(x => CreateStockInputModel(x)).ToList();

            ViewBag.StockInputTagId = Id;
            ViewBag.GRNNumber = tagRecord?.GRNNumber ?? "";

            ViewBag.IsEditAllowed = tagRecord.IsEditAllowed && IsStockInputEditAllowed &&
                StockStatus.Received.ToString().Equals(calledFrom, StringComparison.OrdinalIgnoreCase);

            ViewBag.RowId = rowId;
            ViewBag.ParentRowId = parentRowId;

            return PartialView("StockReceive/_ListItem", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(FeatureList = new FeatureEnum[] { FeatureEnum.StockReceive, FeatureEnum.StockReceiveConfirm })]
        public ActionResult AddEditStockInputItem(long stockInputTagId, long stockInputId)
        {
            if (!IsStockInputEditAllowed)
            {
                return PartialView("_CustomError", "The requested action is not available.");
            }

            DomainEntities.StockInputTag stockInputTagRecord = Helper.GetSingleStockInputTag(stockInputTagId);
            DomainEntities.StockInput stockInputRecord = null;

            if ((stockInputTagRecord?.IsEditAllowed ?? false) == false)
            {
                return PartialView("_CustomError", "The requested action is not available. Please refresh the page and try again.");
            }

            if (stockInputId <= 0)
            {
                stockInputRecord = new StockInput()
                {
                    Id= 0,
                    StockInputTagId = stockInputTagId,
                };
            }
            else
            {
                stockInputRecord = Helper.GetSingleStockInputItem(stockInputTagId, stockInputId);
                if (stockInputRecord == null)
                {
                    return PartialView("_CustomError", "The requested record does not exist.");
                }
            }

            StockInputModel model = CreateStockInputModel(stockInputRecord);
            model.GRNNumber = stockInputTagRecord.GRNNumber;
            model.ParentCyclicCount = stockInputTagRecord.CyclicCount;
            model.IsEditAllowed = stockInputTagRecord.IsEditAllowed;

            // set vendors data
            //ViewBag.Vendors = Business.GetVendors();
            FillItemDataInViewBag();

            return PartialView("StockReceive/_AddEditItem", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(FeatureList = new FeatureEnum[] { FeatureEnum.StockReceive, FeatureEnum.StockReceiveConfirm })]
        public ActionResult AddEditStockInputItem(StockInputModel model)
        {
            string viewPath = "StockReceive/_AddEditItem";
            if (!IsStockInputEditAllowed)
            {
                return PartialView("_CustomError", "The requested action is not available.");
            }

            FillItemDataInViewBag();

            if (!ModelState.IsValid)
            {
                return PartialView(viewPath, model);
            }

            model.Rec.LineNumber = model.LineNumber;
            model.Rec.ItemMasterId = model.ItemMasterId;
            model.Rec.Quantity = model.Quantity;
            model.Rec.Rate = model.Rate;
            model.Rec.Amount = model.Amount;
            model.Rec.CurrentUser = CurrentUserStaffCode;
            model.Rec.DeleteMe = model.DeleteMe;

            try
            {
                DBSaveStatus status = Business.SaveStockInputItemData(model.Rec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView(viewPath, model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AddEditStockInputItem), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView(viewPath, model);
            }
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.StockReceiveConfirm)]
        public ActionResult ConfirmStock()
        {
            ViewBag.Title = "Confirm Stock";
            ViewBag.CalledFrom = StockStatus.ReceiveApproved.ToString();
            return ReceiveOrConfirmStockInput();
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequest)]
        public ActionResult RequestStock()
        {
            ViewBag.AvailableStatus = new List<string>()
            {
                StockStatus.Requested.ToString(),
                StockStatus.RequestApproved.ToString()
            };

            ViewBag.StockRequestType = StockRequestType.StockIssueRequest.ToString();

            ViewBag.Title = "Request Stock";
            return RequestOrRemoveStock();
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequest)]
        public ActionResult GetSearchRequestStock(StockRequestFilter searchCriteria)
        {
            DomainEntities.StockRequestFilter criteria = Helper.ParseStockRequestSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<StockRequestTag> items = Business.GetStockRequestTags(criteria);

            ICollection<StockRequestTagModel> model = items.Select(x => CreateStockRequestTagModel(x)).ToList();

            ViewBag.StockRequestType = searchCriteria.StockRequestType;

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            return PartialView("StockRequest/_ListTags", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequest)]
        public ActionResult AddEditStockRequestTag(long stockRequestTagId, string StockRequestType)
        {
            DomainEntities.StockRequestTag stockRequestTagRecord = null;

            if (stockRequestTagId <= 0)
            {
                stockRequestTagRecord = new StockRequestTag()
                {
                    RequestDate = Helper.GetCurrentIstDateTime(),
                    Status = StockStatus.Requested.ToString(),
                    RequestType = StockRequestType
                };

                if (StockRequestType == DomainEntities.StockRequestType.StockClearRequest.ToString())
                {
                    stockRequestTagRecord.Status = StockStatus.ClearRequest.ToString();
                }
                if (StockRequestType == DomainEntities.StockRequestType.StockAddRequest.ToString())
                {
                    stockRequestTagRecord.Status = StockStatus.AddRequest.ToString();
                }
            }
            else
            {
                stockRequestTagRecord = Helper.GetSingleStockRequestTag(stockRequestTagId);
                if (stockRequestTagRecord == null)
                {
                    return PartialView("_CustomError", "The requested record does not exist.");
                }
            }

            StockRequestTagModel model = CreateStockRequestTagModel(stockRequestTagRecord);
            model.RequestDateAsText = model.RequestDate.ToString("dd-MM-yyyy");

            return PartialView("StockRequest/_AddEditTag", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequest)]
        public ActionResult AddEditStockRequestTag(StockRequestTagModel model)
        {
            string viewPath = "StockRequest/_AddEditTag";
            if (!ModelState.IsValid)
            {
                return PartialView(viewPath, model);
            }

            StockRequestTag tagRecord =
                (model.Id > 0) ? Helper.GetSingleStockRequestTag(model.Id) : new StockRequestTag();

            if (model.Id <= 0)
            {
                tagRecord.RequestNumber = Business.GetNextRequestNumber();
                tagRecord.Status = StockStatus.Requested.ToString();
                tagRecord.RequestType = model.RequestType;

                if (String.IsNullOrEmpty(tagRecord.RequestNumber))
                {
                    ModelState.AddModelError("", "An error occured while generating Request Number. Please refresh the page and try again.");
                    return PartialView(viewPath, model);
                }

                if (model.RequestType == StockRequestType.StockClearRequest.ToString())
                {
                    tagRecord.Status = StockStatus.ClearRequest.ToString();
                }
                if (model.RequestType == StockRequestType.StockAddRequest.ToString())
                {
                    tagRecord.Status = StockStatus.AddRequest.ToString();
                }
            }

            tagRecord.RequestDate = model.RequestDate;
            tagRecord.Notes = Utils.TruncateString(model.Notes, 100);
            tagRecord.ZoneCode = Utils.TruncateString(model.ZoneCode, 50);
            tagRecord.AreaCode = Utils.TruncateString(model.AreaCode, 50);
            tagRecord.TerritoryCode = Utils.TruncateString(model.TerritoryCode, 50);
            tagRecord.HQCode = Utils.TruncateString(model.HQCode, 50);
            tagRecord.StaffCode = Utils.TruncateString(model.StaffCode, 10);

            tagRecord.CyclicCount = model.CyclicCount;
            tagRecord.CurrentUser = CurrentUserStaffCode;

            try
            {
                DBSaveStatus status = Business.SaveStockRequestTagData(tagRecord);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView(viewPath, model);
                }
                return PartialView("ConfirmMessage", $" (Request # {tagRecord.RequestNumber})");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AddEditStockRequestTag), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView(viewPath, model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequest)]
        public ActionResult GetStockRequestItems(long Id, string rowId, string parentRowId) // here it is StockRequestTag Id
        {
            StockRequestTag tagRecord = Helper.GetSingleStockRequestTag(Id);

            ICollection<DomainEntities.StockRequest> items = Business.GetStockRequest(Id);

            ICollection<StockRequestModel> model = items.Select(x => CreateStockRequestModel(x)).ToList();

            ViewBag.StockRequestTagId = Id;
            ViewBag.RequestNumber = tagRecord?.RequestNumber ?? "";

            ViewBag.IsEditAllowed = tagRecord.IsEditAllowed;

            ViewBag.RowId = rowId;
            ViewBag.ParentRowId = parentRowId;
            return PartialView("StockRequest/_ListItem", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequest)]
        public ActionResult AddEditStockRequestItem(long stockRequestTagId, long stockRequestId)
        {
            StockRequestTag stockRequestTagRecord = Helper.GetSingleStockRequestTag(stockRequestTagId);
            StockRequest stockRequestRecord = null;

            if ((stockRequestTagRecord?.IsEditAllowed ?? false) == false)
            {
                return PartialView("_CustomError", "The requested action is not available. Please refresh the page and try again.");
            }

            if (stockRequestId <= 0)
            {
                stockRequestRecord = new StockRequest()
                {
                    Id = 0,
                    StockRequestTagId = stockRequestTagId,
                };
            }
            else
            {
                stockRequestRecord = Helper.GetSingleStockRequestItem(stockRequestTagId, stockRequestId);
                if (stockRequestRecord == null)
                {
                    return PartialView("_CustomError", "The requested record does not exist.");
                }
            }

            StockRequestModel model = CreateStockRequestModel(stockRequestRecord);
            model.RequestNumber = stockRequestTagRecord.RequestNumber;
            model.ParentCyclicCount = stockRequestTagRecord.CyclicCount;
            model.IsEditAllowed = stockRequestTagRecord.IsEditAllowed;

            // set vendors data
            //ViewBag.Vendors = Business.GetVendors();
            FillItemDataInViewBag();

            return PartialView("StockRequest/_AddEditItem", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequest)]
        public ActionResult AddEditStockRequestItem(StockRequestModel model)
        {
            string viewPath = "StockRequest/_AddEditItem";
            FillItemDataInViewBag();

            if (!ModelState.IsValid)
            {
                return PartialView(viewPath, model);
            }

            model.Rec.ItemMasterId = model.ItemMasterId;
            model.Rec.Quantity = model.Quantity;
            model.Rec.CurrentUser = CurrentUserStaffCode;
            model.Rec.DeleteMe = model.DeleteMe;

            try
            {
                DBSaveStatus status = Business.SaveStockRequestItemData(model.Rec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView(viewPath, model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AddEditStockRequestItem), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView(viewPath, model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockReceiveConfirm)]
        public ActionResult ReviewStockInputTag(long stockInputTagId)
        {
            StockInputTag stockInputTagRecord = Helper.GetSingleStockInputTag(stockInputTagId);
            if (stockInputTagRecord == null)
            {
                return PartialView("_CustomError", "The requested record does not exist.");
            }

            // record has to be in edit state, to make it available for approve
            if (stockInputTagRecord.IsEditAllowed == false)
            {
                return PartialView("_CustomError", "The requested action is not available.");
            }

            StockInputTagModel model = CreateStockInputTagModel(stockInputTagRecord);

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            return PartialView("StockReceive/_Review", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockReceiveConfirm)]
        public ActionResult ReviewStockInputTag(StockInputReviewTagModel model)
        {
            if (!IsStockInputConfirmAllowed)
            {
                return PartialView("_CustomError", "The requested action is not available.");
            }

            if (!ModelState.IsValid)
            {
                return PartialView("_CustomError", "The data entered is not valid.  Please try again.");
            }

            StockInputTag tagRecord = Helper.GetSingleStockInputTag(model.Id);
            if (tagRecord == null)
            {
                return PartialView("_CustomError", "The record to be updated does not exist. Please refresh the page and try again. ");
            }

            tagRecord.ReviewNotes = Utils.TruncateString(model.ReviewNotes, 100);
            tagRecord.CyclicCount = model.CyclicCount;
            tagRecord.CurrentUser = CurrentUserStaffCode;

            tagRecord.Status = StockStatus.ReceiveApproved.ToString();

            try
            {
                DBSaveStatus status = Business.ReviewStockInputTagData(tagRecord);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    return PartialView("_CustomError", "An error occured while saving changes. Please refresh the page and try again.");
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(StockInputReviewTagModel), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("_CustomError", ex.Message);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequest)]
        public ActionResult ReviewStockRequestTag(long stockRequestTagId)
        {
            StockRequestTag tagRecord = Helper.GetSingleStockRequestTag(stockRequestTagId);
            if (tagRecord == null)
            {
                return PartialView("_CustomError", "The requested record does not exist.");
            }

            // record has to be in edit state, to make it available for approve
            if (tagRecord.IsEditAllowed == false)
            {
                return PartialView("_CustomError", "The requested action is not available.");
            }

            StockRequestTagModel model = CreateStockRequestTagModel(tagRecord);

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            return PartialView("StockRequest/_Review", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequest)]
        public ActionResult ReviewStockRequestTag(StockRequestReviewTagModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("_CustomError", "The data entered is not valid.  Please try again.");
            }

            StockRequestTag tagRecord = Helper.GetSingleStockRequestTag(model.Id);
            if (tagRecord == null)
            {
                return PartialView("_CustomError", "The record to be updated does not exist. Please refresh the page and try again. ");
            }

            tagRecord.CyclicCount = model.CyclicCount;
            tagRecord.CurrentUser = CurrentUserStaffCode;

            if (tagRecord.RequestType == DomainEntities.StockRequestType.StockIssueRequest.ToString())
            {
                tagRecord.Status = StockStatus.RequestApproved.ToString();
            }
            else if (tagRecord.RequestType == DomainEntities.StockRequestType.StockClearRequest.ToString())
            {
                tagRecord.Status = StockStatus.ClearApproved.ToString();
            }
            else if (tagRecord.RequestType == DomainEntities.StockRequestType.StockAddRequest.ToString())
            {
                tagRecord.Status = StockStatus.AddApproved.ToString();
            }
            else
            {
                tagRecord.Status = StockStatus.RequestApproved.ToString();
            }

            try
            {
                DBSaveStatus status = Business.ReviewStockRequestTagData(tagRecord);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    return PartialView("_CustomError", "An error occured while saving changes. Please refresh the page and try again.");
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(StockRequestReviewTagModel), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("_CustomError", ex.Message);
            }
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequestFulfill)]
        public ActionResult FulfillStockRequest()
        {
            ViewBag.Title = "Fulfill Stock Request(s)";

            ViewBag.StockRequestType = StockRequestType.StockIssueRequest.ToString();
            ViewBag.TagRecordStatus = StockStatus.RequestApproved.ToString();

            ViewBag.EditActionName = nameof(FulfillStockRequestItem);

            return Fulfill();
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequestFulfill)]
        public ActionResult GetSearchFulfillStockRequest(StockRequestFilter searchCriteria)
        {
            DomainEntities.StockRequestFilter criteria = Helper.ParseStockRequestSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<StockRequestFull> items = Business.GetStockRequestItems(criteria);

            ICollection<StockRequestFullModel> model = items.Select(x => CreateStockRequestFullModel(x)).ToList();

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            ViewBag.EditActionName = searchCriteria.EditActionName;
            return PartialView("FulfillStockRequest/_List", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequestFulfill)]
        public ActionResult FulfillStockRequestItem(long stockRequestId)
        {
            // retrieve the requested record
            DomainEntities.StockRequestFilter criteria = Helper.GetDefaultStockRequestFilter();
            criteria.StockRequestType = StockRequestType.StockIssueRequest.ToString();
            criteria.TagRecordStatus = StockStatus.RequestApproved.ToString();
            criteria.ApplyStockRequestIdFilter = true;
            criteria.StockRequestId = stockRequestId;
            StockRequestFull srf = Business.GetStockRequestItems(criteria)?.FirstOrDefault();
            if (srf == null)
            {
                return PartialView("_CustomError", "The record does not exist. Please refresh the page and try again. ");
            }

            // retrieve stock balances for the item

            DomainEntities.StockLedgerFilter slf = Helper.GetDefaultStockLedgerFilter();
            slf.ApplyItemIdFilter = true;
            slf.ItemId = srf.ItemMasterId;
            IEnumerable<StockBalance> balances = Business.GetStockBalance(slf);

            var requesterBalanceRecord = balances.Where(x => x.ZoneCode == srf.ZoneCode 
                                    && x.AreaCode == srf.AreaCode 
                                    && x.TerritoryCode == srf.TerritoryCode
                                    && x.HQCode == srf.HQCode 
                                    && x.StaffCode == srf.StaffCode).FirstOrDefault();
            var requesterBalanceRecordId = requesterBalanceRecord?.Id ?? 0;

            balances = balances.Where(x => x.StockQuantity > 0 && x.Id != requesterBalanceRecordId);

            if ((balances?.Count() ?? 0) == 0)
            {
                return PartialView("_CustomError", $"The requested item '{srf.ItemDesc}' does not exist in stock.");
            }

            StockRequestFullModel model = CreateStockRequestFullModel(srf);

            ViewBag.Balances = balances.Select(x => CreateStockBalanceModel(x)).ToList();
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            ViewBag.QuantityInHand = requesterBalanceRecord?.StockQuantity ?? 0;

            return PartialView("FulfillStockRequest/_Fulfill", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequestFulfill)]
        public ActionResult FulfillStockRequestItem(StockFulfillModel model)
        {
            if (!ModelState.IsValid)
            {
                string errorList = GetModelErrors();
                return PartialView("_CustomError", $"Validation error occured. Please refresh the page and try again. {errorList}");
            }

            StockFulfillmentData fulfillmentData = new StockFulfillmentData()
            {
                CurrentUser = CurrentUserStaffCode,
                CyclicCount = model.CyclicCount,
                StockRequestId = model.StockRequestId,
                Status = StockStatus.Complete.ToString(),
                StockBalances = model.FulfillItems.Select(x=> x.StockBalanceRec).ToList(),
                StockRequestTagId = model.StockRequestFullRec.StockRequestTagId,
                CurrentIstTime = Helper.GetCurrentIstDateTime(),
                IsConfirmClicked = true,
                IsDenyClicked = false,
                Notes = Utils.TruncateString(model.Notes, 100)
            };

            try
            {
                DBSaveStatus status = Business.PerformFulfillment(fulfillmentData);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    return PartialView("_CustomError", "An error occured while saving changes. Please refresh the page and try again.");
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(FulfillStockRequestItem), ex.ToString(), ">");
                return PartialView("_CustomError", ex.Message);
            }
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRequestFulfill)]
        public ActionResult DenyFulfillStockRequestItem(StockFulfillDenyModel model)
        {
            if (!ModelState.IsValid)
            {
                string errorList = GetModelErrors();
                return PartialView("_CustomError", $"Validation error occured. Please refresh the page and try again. {errorList}");
            }

            StockFulfillmentData fulfillmentData = new StockFulfillmentData()
            {
                CurrentUser = CurrentUserStaffCode,
                CyclicCount = model.CyclicCount,
                StockRequestId = model.StockRequestId,
                Status = StockStatus.Denied.ToString(),
                StockBalances = null,
                StockRequestTagId = model.StockRequestFullRec.StockRequestTagId,
                CurrentIstTime = Helper.GetCurrentIstDateTime(),
                IsConfirmClicked = false,
                IsDenyClicked = true,
                Notes = Utils.TruncateString(model.Notes, 100)
            };

            try
            {
                DBSaveStatus status = Business.PerformFulfillment(fulfillmentData);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    return PartialView("_CustomError", "An error occured while saving changes. Please refresh the page and try again.");
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(FulfillStockRequestItem), ex.ToString(), ">");
                return PartialView("_CustomError", ex.Message);
            }
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.StockLedger)]
        public ActionResult StockLedger()
        {
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            FillItemDataInViewBag();

            ViewBag.SearchResultAction = nameof(GetSearchStockLedger);
            ViewBag.Title = "Stock Ledger";

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("StockLedger/Index");
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockLedger)]
        public ActionResult GetSearchStockLedger(StockLedgerFilter searchCriteria)
        {
            DomainEntities.StockLedgerFilter criteria = Helper.ParseStockLedgerSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<StockLedger> items = Business.GetStockLedger(criteria);

            ICollection<StockLedgerModel> model = items.Select(x => CreateStockLedgerModel(x)).ToList();

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            return PartialView("StockLedger/_List", model);
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.StockBalance)]
        public ActionResult StockBalance()
        {
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            FillItemDataInViewBag();

            ViewBag.SearchResultAction = nameof(GetSearchStockBalance);
            ViewBag.Title = "Stock Balance";

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("StockBalance/Index");
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockBalance)]
        public ActionResult GetSearchStockBalance(StockLedgerFilter searchCriteria)
        {
            DomainEntities.StockLedgerFilter criteria = Helper.ParseStockLedgerSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<StockBalance> items = Business.GetStockBalance(criteria);

            ICollection<StockBalanceModel> model = items.Select(x => CreateStockBalanceModel(x)).ToList();

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            return PartialView("StockBalance/_List", model);
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.StockRemove)]
        public ActionResult StockRemove()
        {
            ViewBag.AvailableStatus = new List<string>()
            {
                StockStatus.ClearRequest.ToString(),
                StockStatus.ClearApproved.ToString(),
            };

            ViewBag.StockRequestType = StockRequestType.StockClearRequest.ToString();
            ViewBag.Title = "Clear Stock";
            return RequestOrRemoveStock();
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.StockRemoveConfirm)]
        public ActionResult ConfirmStockRemove()
        {
            ViewBag.Title = "Confirm Stock Remove";

            ViewBag.StockRequestType = StockRequestType.StockClearRequest.ToString();
            ViewBag.TagRecordStatus = StockStatus.ClearApproved.ToString();

            ViewBag.EditActionName = nameof(ConfirmStockItemRemove);

            return Fulfill();
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRemoveConfirm)]
        public ActionResult ConfirmStockItemRemove(long stockRequestId)
        {
            // retrieve the requested record
            DomainEntities.StockRequestFilter criteria = Helper.GetDefaultStockRequestFilter();
            criteria.StockRequestType = StockRequestType.StockClearRequest.ToString();
            criteria.TagRecordStatus = StockStatus.ClearApproved.ToString();
            criteria.ApplyStockRequestIdFilter = true;
            criteria.StockRequestId = stockRequestId;
            StockRequestFull srf = Business.GetStockRequestItems(criteria)?.FirstOrDefault();
            if (srf == null)
            {
                return PartialView("_CustomError", "The record does not exist. Please refresh the page and try again. ");
            }

            // retrieve stock balances for the item

            DomainEntities.StockLedgerFilter slf = Helper.GetDefaultStockLedgerFilter();
            slf.ApplyItemIdFilter = true;
            slf.ItemId = srf.ItemMasterId;
            IEnumerable<StockBalance> balances = Business.GetStockBalance(slf);

            StockBalance requesterBalanceRecord = balances.Where(x => x.ZoneCode == srf.ZoneCode
                                    && x.AreaCode == srf.AreaCode
                                    && x.TerritoryCode == srf.TerritoryCode
                                    && x.HQCode == srf.HQCode
                                    && x.StaffCode == srf.StaffCode).FirstOrDefault();

            if (requesterBalanceRecord == null)
            {
                return PartialView("_CustomError", $"The requested item '{srf.ItemDesc}' does not exist in stock.");
            }

            if (requesterBalanceRecord.StockQuantity < srf.Quantity)
            {
                return PartialView("_CustomError", $"There is not enough stock of the requested item '{srf.ItemDesc}' | Stock in Hand {requesterBalanceRecord.StockQuantity} | Opted Quantity {srf.Quantity}");
            }

            StockRequestFullModel model = CreateStockRequestFullModel(srf);

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            ViewBag.RequesterBalanceRecord = requesterBalanceRecord;

            return PartialView("FulfillStockRequest/_Remove", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockRemoveConfirm)]
        public ActionResult ConfirmStockItemRemove(StockClearModel model)
        {
            if (!ModelState.IsValid)
            {
                string errorList = GetModelErrors();
                return PartialView("_CustomError", $"Validation error occured. Please refresh the page and try again. {errorList}");
            }

            StockFulfillmentData fulfillmentData = new StockFulfillmentData()
            {
                CurrentUser = CurrentUserStaffCode,
                CyclicCount = model.CyclicCount,
                StockRequestId = model.StockRequestId,
                Status = model.IsConfirmClicked ? StockStatus.Complete.ToString() : StockStatus.Denied.ToString(),
                StockBalances =  null,
                StockBalanceRec = model.StockBalanceRec,
                StockRequestTagId = model.StockRequestFullRec.StockRequestTagId,
                CurrentIstTime = Helper.GetCurrentIstDateTime(),
                IsConfirmClicked = model.IsConfirmClicked,
                IsDenyClicked = !model.IsConfirmClicked,
                Notes = Utils.TruncateString(model.Notes, 100)
            };

            try
            {
                DBSaveStatus status = Business.PerformStockClear(fulfillmentData);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    return PartialView("_CustomError", "An error occured while saving changes. Please refresh the page and try again.");
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(ConfirmStockItemRemove), ex.ToString(), ">");
                return PartialView("_CustomError", ex.Message);
            }
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.StockAdd)]
        public ActionResult StockAdd()
        {
            ViewBag.AvailableStatus = new List<string>()
            {
                StockStatus.AddRequest.ToString(),
                StockStatus.AddApproved.ToString()
            };

            ViewBag.StockRequestType = StockRequestType.StockAddRequest.ToString();
            ViewBag.Title = "Add Stock";
            return RequestOrRemoveStock();
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.StockAddConfirm)]
        public ActionResult ConfirmStockAdd()
        {
            ViewBag.Title = "Confirm Stock Add";

            ViewBag.StockRequestType = StockRequestType.StockAddRequest.ToString();
            ViewBag.TagRecordStatus = StockStatus.AddApproved.ToString();

            ViewBag.EditActionName = nameof(ConfirmStockItemAdd);

            return Fulfill();
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockAddConfirm)]
        public ActionResult ConfirmStockItemAdd(long stockRequestId)
        {
            // retrieve the requested record
            DomainEntities.StockRequestFilter criteria = Helper.GetDefaultStockRequestFilter();
            criteria.StockRequestType = StockRequestType.StockAddRequest.ToString();
            criteria.TagRecordStatus = StockStatus.AddApproved.ToString();
            criteria.ApplyStockRequestIdFilter = true;
            criteria.StockRequestId = stockRequestId;
            StockRequestFull srf = Business.GetStockRequestItems(criteria)?.FirstOrDefault();
            if (srf == null)
            {
                return PartialView("_CustomError", "The record does not exist. Please refresh the page and try again. ");
            }

            // retrieve stock balances for the item

            DomainEntities.StockLedgerFilter slf = Helper.GetDefaultStockLedgerFilter();
            slf.ApplyItemIdFilter = true;
            slf.ItemId = srf.ItemMasterId;
            IEnumerable<StockBalance> balances = Business.GetStockBalance(slf);

            StockBalance requesterBalanceRecord = balances.Where(x => x.ZoneCode == srf.ZoneCode
                                    && x.AreaCode == srf.AreaCode
                                    && x.TerritoryCode == srf.TerritoryCode
                                    && x.HQCode == srf.HQCode
                                    && x.StaffCode == srf.StaffCode).FirstOrDefault();

            StockRequestFullModel model = CreateStockRequestFullModel(srf);
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            ViewBag.RequesterBalanceRecord = requesterBalanceRecord;

            return PartialView("FulfillStockRequest/_Add", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.StockAddConfirm)]
        public ActionResult ConfirmStockItemAdd(StockAddModel model)
        {
            if (!ModelState.IsValid)
            {
                string errorList = GetModelErrors();
                return PartialView("_CustomError", $"Validation error occured. Please refresh the page and try again. {errorList}");
            }

            StockFulfillmentData fulfillmentData = new StockFulfillmentData()
            {
                CurrentUser = CurrentUserStaffCode,
                CyclicCount = model.CyclicCount,
                StockRequestId = model.StockRequestId,
                Status = model.IsConfirmClicked ? StockStatus.Complete.ToString() : StockStatus.Denied.ToString(),
                StockBalances = null,
                StockBalanceRec = model.StockBalanceRec,
                StockRequestTagId = model.StockRequestFullRec.StockRequestTagId,
                CurrentIstTime = Helper.GetCurrentIstDateTime(),
                IsConfirmClicked = model.IsConfirmClicked,
                IsDenyClicked = !model.IsConfirmClicked,
                Notes = Utils.TruncateString(model.Notes, 100)
            };

            try
            {
                DBSaveStatus status = Business.PerformStockAdd(fulfillmentData);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    return PartialView("_CustomError", "An error occured while saving changes. Please refresh the page and try again.");
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(ConfirmStockItemAdd), ex.ToString(), ">");
                return PartialView("_CustomError", ex.Message);
            }
        }

        private static StockInputTagModel CreateStockInputTagModel(StockInputTag inputRec)
        {
            var m = new StockInputTagModel()
            {
                Id = inputRec.Id,
                GRNNumber = inputRec.GRNNumber,
                ReceiveDate = inputRec.ReceiveDate,
                VendorName = inputRec.VendorName,
                VendorBillNo = inputRec.VendorBillNo,
                VendorBillDate = inputRec.VendorBillDate,
                TotalItemCount = inputRec.TotalItemCount,
                TotalAmount = inputRec.TotalAmount,
                Notes = inputRec.Notes,
                Status = inputRec.Status,
                ItemCountFromLines = inputRec.ItemCountFromLines ?? 0,
                AmountTotalFromLines = inputRec.AmountTotalFromLines ?? 0,
                CyclicCount = inputRec.CyclicCount,
                IsEditAllowed = inputRec.IsEditAllowed,
                ReviewDate = inputRec.ReviewDate,
                ReviewedBy = inputRec.ReviewedBy,
                ReviewNotes = inputRec.ReviewNotes,
                ReceiveDateAsText = inputRec.ReceiveDate.ToString("dd-MM-yyyy"),
                VenderBillDateAsText = inputRec.VendorBillDate.ToString("dd-MM-yyyy")
            };

            FillCommonTagModelData(inputRec, m);

            return m;
        }

        private static StockRequestTagModel CreateStockRequestTagModel(StockRequestTag inputRec)
        {
            var m = new StockRequestTagModel()
            {
                Id = inputRec.Id,
                RequestNumber = inputRec.RequestNumber,
                RequestDate = inputRec.RequestDate,
                Notes = inputRec.Notes,
                Status = inputRec.Status,
                ItemCountFromLines = inputRec.ItemCountFromLines ?? 0,
                CyclicCount = inputRec.CyclicCount,
                IsEditAllowed = inputRec.IsEditAllowed,
                RequestType = inputRec.RequestType,
            };

            FillCommonTagModelData(inputRec, m);

            return m;
        }

        public static void FillCommonTagModelData(Stock inputRec, StockTagModel target)
        {
            target.ZoneCode = inputRec.ZoneCode;
            target.AreaCode = inputRec.AreaCode;
            target.TerritoryCode = inputRec.TerritoryCode;
            target.HQCode = inputRec.HQCode;
            target.StaffCode = inputRec.StaffCode;
            target.EmployeeName = inputRec.EmployeeName;
            target.DateCreated = Helper.ConvertUtcTimeToIst(inputRec.DateCreated);
            target.CreatedBy = inputRec.CreatedBy;
            target.DateUpdated = Helper.ConvertUtcTimeToIst(inputRec.DateUpdated);
            target.UpdatedBy = inputRec.UpdatedBy;
        }

        private static StockLedgerModel CreateStockLedgerModel(StockLedger inputRec)
        {
            var m = new StockLedgerModel()
            {
                Id = inputRec.Id,
                ItemMasterId = inputRec.ItemMasterId,
                TransactionDate = inputRec.TransactionDate,
                ReferenceNo = inputRec.ReferenceNo,
                LineNumber = inputRec.LineNumber,
                IssueQuantity = inputRec.IssueQuantity,
                ReceiveQuantity = inputRec.ReceiveQuantity,
                ItemCode = inputRec.ItemCode,
                ItemDesc = inputRec.ItemDesc,
                Category = inputRec.Category,
                Unit = inputRec.Unit,
                ZoneCode = inputRec.ZoneCode,
                AreaCode = inputRec.AreaCode,
                TerritoryCode = inputRec.TerritoryCode,
                HQCode = inputRec.HQCode,
                StaffCode = inputRec.StaffCode,
                EmployeeName = inputRec.EmployeeName
            };

            FillCommonTagModelData(inputRec, m);

            return m;
        }

        private static StockBalanceModel CreateStockBalanceModel(StockBalance inputRec)
        {
            var m = new StockBalanceModel()
            {
                Id = inputRec.Id,
                ItemMasterId = inputRec.ItemMasterId,
                StockQuantity = inputRec.StockQuantity,
                ItemCode = inputRec.ItemCode,
                ItemDesc = inputRec.ItemDesc,
                Category = inputRec.Category,
                Unit = inputRec.Unit,
                CyclicCount = inputRec.CyclicCount
            };

            FillCommonTagModelData(inputRec, m);

            return m;
        }

        private StockInput GetSingleStockInputItem(long stockInputTagId, long stockInputId)
        {
            ICollection<StockInput> items = Business.GetStockInput(stockInputTagId);
            return items.FirstOrDefault(x => x.Id == stockInputId);
        }

        private static StockInputModel CreateStockInputModel(StockInput inputRec)
        {
            return new StockInputModel()
            {
                Id = inputRec.Id,
                StockInputTagId = inputRec.StockInputTagId,
                LineNumber = inputRec.LineNumber,
                ItemMasterId = inputRec.ItemMasterId,
                Quantity = inputRec.Quantity,
                Rate = inputRec.Rate,
                Amount = inputRec.Amount,
                ItemCode = inputRec.ItemCode,
                ItemDesc = inputRec.ItemDesc,
                Category = inputRec.Category,
                Unit = inputRec.Unit,
                CyclicCount = inputRec.CyclicCount
            };
        }

        private static StockRequestModel CreateStockRequestModel(StockRequest inputRec)
        {
            return new StockRequestModel()
            {
                Id = inputRec.Id,
                StockRequestTagId = inputRec.StockRequestTagId,
                ItemMasterId = inputRec.ItemMasterId,
                Quantity = inputRec.Quantity,
                ItemCode = inputRec.ItemCode,
                ItemDesc = inputRec.ItemDesc,
                Category = inputRec.Category,
                Unit = inputRec.Unit,
                Status = inputRec.Status,
                CyclicCount = inputRec.CyclicCount,
                QuantityIssued = inputRec.QuantityIssued,
                IssueDate = inputRec.IssueDate,
                StockLedgerId = inputRec.StockLedgerId,
                UpdatedBy = inputRec.UpdatedBy,
                DateUpdated = Helper.ConvertUtcTimeToIst(inputRec.DateUpdated),
                ReviewNotes = inputRec.ReviewNotes
            };
        }

        private static StockRequestFullModel CreateStockRequestFullModel(StockRequestFull inputRec)
        {
            var m = new StockRequestFullModel()
            {
                Id = inputRec.Id,
                StockRequestTagId = inputRec.StockRequestTagId,
                ItemMasterId = inputRec.ItemMasterId,
                RequestNumber = inputRec.RequestNumber,
                RequestDate = inputRec.RequestDate,
                Notes = inputRec.Notes,

                Quantity = inputRec.Quantity,

                ItemCode = inputRec.ItemCode,
                ItemDesc = inputRec.ItemDesc,
                Category = inputRec.Category,
                Unit = inputRec.Unit,

                Status = inputRec.Status,

                CyclicCount = inputRec.CyclicCount,
                IsPendingFulfillment = inputRec.IsPendingFulfillment,

                QuantityIssued = inputRec.QuantityIssued,
                IssueDate = inputRec.IssueDate,
                StockLedgerId = inputRec.StockLedgerId,

                RequestType = inputRec.RequestType,
                ReviewNotes = inputRec.ReviewNotes
            };

            FillCommonTagModelData(inputRec, m);
            return m;
        }

        private void FillItemDataInViewBag()
        {
            ViewBag.ItemTypes = Business.GetCodeTable("ItemType");

            ICollection<DomainEntities.ItemMaster> itemsData = Business.GetAllItemMaster();
            ViewBag.ItemMaster = itemsData.Select(x => new DownloadItemMaster()
            {
                ItemId = x.Id,
                ItemCode = x.ItemCode.Replace('\'', ' '),
                ItemDesc = x.ItemDesc,
                Category = x.Category,
                Unit = x.Unit,
                Classification = x.Classification
            })
            .OrderBy(x=> x.ItemCode)
            .ToList();
        }

        public ActionResult ReceiveOrConfirmStockInput()
        {
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            ICollection<Vendor> vendors = Business.GetVendors();
            ViewBag.Vendors = vendors;

            ViewBag.AvailableStatus = new List<string>()
            {
                StockStatus.Received.ToString(),
                StockStatus.ReceiveApproved.ToString()
            };

            ViewBag.SearchResultAction = nameof(GetSearchReceiveStock);

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("StockReceive/Index");
        }

        private ActionResult RequestOrRemoveStock()
        {
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            ViewBag.SearchResultAction = nameof(GetSearchRequestStock);
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("StockRequest/Index");
        }

        private ActionResult Fulfill()
        {
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            ViewBag.AvailableStatus = new List<string>()
            {
                StockStatus.Pending.ToString(),
                StockStatus.Complete.ToString(),
                StockStatus.Denied.ToString()
            };

            ViewBag.SearchResultAction = nameof(GetSearchFulfillStockRequest);
            FillItemDataInViewBag();

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("FulfillStockRequest/Index");
        }

        private bool IsStockInputEditAllowed =>
            Helper.GetAvailableFeatures(CurrentUserStaffCode, IsSuperAdmin).StockReceiveOption;

        private bool IsStockInputConfirmAllowed => 
            Helper.GetAvailableFeatures(CurrentUserStaffCode, IsSuperAdmin).StockReceiveConfirmOption;
    }
}
