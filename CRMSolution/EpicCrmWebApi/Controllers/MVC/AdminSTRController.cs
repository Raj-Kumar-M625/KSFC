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

        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult STR()
        {
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("STR/Index");
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult AddSTRTag()
        {
            STRTagModel model =
                         new STRTagModel()
                         {
                             Id = 0,
                             STRNumber = "",
                             STRDate = Helper.GetCurrentIstDateTime()
                         };

            return PartialView("STR/_AddSTRTag", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult AddSTRTag(STRTagModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("STR/_AddSTRTag", model);
            }

            model.STRNumber = Utils.TruncateString(model.STRNumber, 50);

            DomainEntities.STRTag rec = new DomainEntities.STRTag()
            {
                STRNumber = model.STRNumber,
                STRDate = model.STRDate,
                CurrentUser = CurrentUserStaffCode,
                Status = STRDWSStatus.Pending.ToString(),
            };

            try
            {
                Business.CreateSTRTagData(rec);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AddSTRTag), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("STR/_AddSTRTag", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult EditSTRTag(long strTagId)
        {
            DomainEntities.STRTag strTagRec = Business.GetSingleSTRTag(strTagId);

            // retrieve strWeight record to see if it is in pending state or not
            ViewBag.IsEditAllowed = IsStrEditAllowed(strTagId, strTagRec);

            ICollection<DomainEntities.STR> items = Business.GetSTR(strTagId);
            ViewBag.VehicleNumberCount = items.Select(x => x.VehicleNumber).Distinct().Count();
            var STRVehicleNumbers = items.Select(x => x.VehicleNumber).Distinct().ToList();
            ViewBag.STRVehicleNumber = String.Join(",", STRVehicleNumbers);

            STRTagModel model =
                         new STRTagModel()
                         {
                             Id = strTagRec.Id,
                             STRNumber = strTagRec.STRNumber,
                             STRDate = strTagRec.STRDate,
                             DWSCount = strTagRec.DWSCount.HasValue ? strTagRec.DWSCount.Value : 0,
                             BagCount = strTagRec.BagCount.HasValue ? strTagRec.BagCount.Value : 0,
                             GrossWeight = strTagRec.GrossWeight.HasValue ? strTagRec.GrossWeight.Value : 0,
                             NetWeight = strTagRec.NetWeight.HasValue ? strTagRec.NetWeight.Value : 0,
                             StartOdo = strTagRec.StartOdo.HasValue ? strTagRec.StartOdo.Value : 0,
                             STRCount = strTagRec.STRCount.HasValue ? strTagRec.STRCount.Value : 0,
                             CyclicCount = strTagRec.CyclicCount
                         };

            model.STRWeight = Business.GetSTRWeight(model.STRNumber);
            model.STRWeightCyclicCount = model.STRWeight?.CyclicCount ?? 0;

            model.STRDateAsText = model.STRDate.ToString("dd-MM-yyyy");
            return PartialView("STR/_EditSTRTag", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult EditSTRTag(STRTagModel model)
        {
            // in case of invalid model state, we have to show STRWeight data
            // and we will show the data of original str number (as user may have changed 
            // str number in current edit screen)
            DomainEntities.STRTag origRec = Business.GetSingleSTRTag(model.Id);
            //model.STRWeight = Business.GetSTRWeight(origRec.STRNumber);
            ViewBag.IsEditAllowed = true;

            if (!ModelState.IsValid)
            {
                return PartialView("STR/_EditSTRTag", model);
            }

            origRec.STRNumber = Utils.TruncateString(model.STRNumber, 50);
            origRec.STRDate = model.STRDate;
            origRec.CurrentUser = CurrentUserStaffCode;
            origRec.CyclicCount = model.CyclicCount;
            origRec.IsFinal = model.IsFinal;
            origRec.IsCancel = model.IsCancel;

            origRec.STRWeightCyclicCount = model.STRWeightCyclicCount;

            if (model.IsFinal && model.STRWeight != null)
            {
                // this is used in updates in BusinessLayer
                origRec.STRWeightRecId = model.STRWeight.Id;
            }

            try
            {
                DBSaveStatus status = Business.SaveSTRTagData(origRec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView("STR/_EditSTRTag", model);
                }

                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditSTRTag), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("STR/_EditSTRTag", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult AddSTR(long strTagId, string strNumber)
        {
            var strTagRec = Business.GetSingleSTRTag(strTagId);
            if (!IsStrEditAllowed(strTagId, strTagRec))
            {
                var msg = $"Edit option is no longer available for STR # {strNumber}.";
                return PartialView("_CustomError", msg);
            }

            STRModel model =
                         new STRModel()
                         {
                             Id = 0,
                             STRTagId = strTagId,
                             STRNumber = strNumber,
                             StrTagCyclicCount = strTagRec.CyclicCount
                         };

            FillEditSTRViewBag();
            return PartialView("STR/_AddSTR", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult AddSTR(STRModel model)
        {
            FillEditSTRViewBag();
            if (!ModelState.IsValid)
            {
                return PartialView("STR/_AddSTR", model);
            }

            // check that Employee Code does exist
            EmployeeRecord empRec = Business.GetTenantEmployee(model.EmployeeCode);

            DomainEntities.STR rec = new DomainEntities.STR()
            {
                STRTagId = model.STRTagId,
                EmployeeId = empRec.EmployeeId,
                VehicleNumber = Utils.TruncateString(model.VehicleNumber, 50),
                DriverName = Utils.TruncateString(model.DriverName, 50),
                DriverPhone = Utils.TruncateString(model.DriverPhone, 50),
                CurrentUser = CurrentUserStaffCode,
                StrTagCyclicCount = model.StrTagCyclicCount,
                Status = STRDWSStatus.Pending.ToString(),
                StartOdometer = model.StartOdometer,
                EndOdometer = model.EndOdometer
            };

            try
            {
                DBSaveStatus status = Business.CreateSTR(rec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView("STR/_AddSTR", model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AddSTR), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("STR/_AddSTR", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult EditSTR(long strId)
        {
            DomainEntities.STR strRec = Business.GetSingleSTR(strId);

            ViewBag.IsEditAllowed = IsStrEditAllowed(strRec.STRTagId);

            STRModel model =
                         new STRModel()
                         {
                             Id = strRec.Id,
                             STRTagId = strRec.STRTagId,
                             EmployeeId = strRec.EmployeeId,
                             VehicleNumber = strRec.VehicleNumber,
                             DriverName = strRec.DriverName,
                             DriverPhone = strRec.DriverPhone,
                             DWSCount = strRec.DWSCount,
                             BagCount = strRec.BagCount,
                             GrossWeight = strRec.GrossWeight,
                             NetWeight = strRec.NetWeight,
                             StartOdometer = strRec.StartOdometer,
                             EndOdometer = strRec.EndOdometer,
                             IsNew = strRec.IsNew,
                             IsTransferred = strRec.IsTransferred,
                             TransfereeName = strRec.TransfereeName,
                             TransfereePhone = strRec.TransfereePhone,
                             ImageCount = strRec.ImageCount,
                             ActivityId = strRec.ActivityId,
                             ActivityId2 = strRec.ActivityId2,
                             EmployeeCode = strRec.EmployeeCode,
                             EmployeeName = strRec.EmployeeName,
                             EmployeePhone = strRec.EmployeePhone,
                             STRNumber = strRec.STRNumber,
                             StrTagCyclicCount = strRec.StrTagCyclicCount
                         };

            FillEditSTRViewBag();
            return PartialView("STR/_EditSTR", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult EditSTR(STRModel model)
        {
            FillEditSTRViewBag();
            ViewBag.IsEditAllowed = true;
            if (!ModelState.IsValid)
            {
                return PartialView("STR/_EditSTR", model);
            }

            DomainEntities.STR origRec = Business.GetSingleSTR(model.Id);

            if (!origRec.EmployeeCode.Equals(model.EmployeeCode, StringComparison.OrdinalIgnoreCase))
            {
                EmployeeRecord empRec = Business.GetTenantEmployee(model.EmployeeCode);
                origRec.EmployeeId = empRec.EmployeeId;
            }

            origRec.VehicleNumber = model.VehicleNumber;

            // user changed str # - reassignment case
            if (!origRec.STRNumber.Equals(model.STRNumber, StringComparison.OrdinalIgnoreCase))
            {
                var rec = Business.GetSTRTag(model.STRNumber);
                origRec.ChangeSTRNumber = true;
                origRec.MoveToStrTagId = rec.Id;
            }

            origRec.StartOdometer = model.StartOdometer;
            origRec.EndOdometer = model.EndOdometer;

            origRec.CurrentUser = CurrentUserStaffCode;
            origRec.StrTagCyclicCount = model.StrTagCyclicCount;

            try
            {
                DBSaveStatus status = Business.SaveSTRData(origRec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView("STR/_EditSTR", model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditSTR), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("STR/_EditSTR", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult AddDWS([System.Web.Http.FromUri]DWSModel model)
        {
            ModelState.Clear();

            STRTag strTagRec = Business.GetSingleSTRTag(model.STRTagId);
            if (!IsStrEditAllowed(model.STRTagId, strTagRec))
            {
                var msg = $"Edit option is no longer available for STR # {model.STRNumber}.";
                return PartialView("_CustomError", msg);
            }

            model.StrTagCyclicCount = strTagRec.CyclicCount;

            return PartialView("STR/_AddDWS", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult AddDWSPost(DWSModel model)
        {
            if (!ModelState.IsValid)
            {
                return PartialView("STR/_AddDWS", model);
            }

            ICollection<DomainEntities.EntityWorkFlow> entityWorkFlows =
                        Business.GetEntityWorkFlow(model.Agreement);

            try
            {
                DomainEntities.EntityWorkFlow entityWFRec = entityWorkFlows.First();

                DomainEntities.DWS rec = new DomainEntities.DWS()
                {
                    STRTagId = model.STRTagId,
                    STRId = model.STRId,
                    DWSNumber = model.DWSNumber,
                    DWSDate = model.DWSDate,
                    BagCount = model.BagCount,
                    FilledBagsWeightKg = model.FilledBagsWeightKg,
                    EmptyBagsWeightKg = model.EmptyBagsWeightKg,
                    EntityId = entityWFRec.EntityId,
                    AgreementId = entityWFRec.AgreementId,
                    Agreement = entityWFRec.Agreement,
                    EntityWorkFlowDetailId = 0,
                    TypeName = entityWFRec.TypeName,
                    TagName = "",
                    ActivityId = 0,
                    CurrentUser = CurrentUserStaffCode,
                    Comments = Utils.TruncateString(model.Comments, 100),
                    Status = STRDWSStatus.Pending.ToString(),
                    CyclicCount = 1,
                    StrTagCyclicCount = model.StrTagCyclicCount,
                };

                DBSaveStatus status = Business.CreateDWS(rec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView("STR/_AddDWS", model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AddDWS), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("STR/_AddDWS", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult EditDWS(long dwsId)
        {
            DomainEntities.DWS dwsRec = Business.GetSingleDWS(dwsId);

            ViewBag.IsEditAllowed = IsStrEditAllowed(dwsRec.STRTagId);

            DWSModel model = CreateDWSModel(dwsRec);

            if (model.IsPendingStatus)
            {
                return PartialView("STR/_EditDWS", model);
            }
            else
            {
                return PartialView("STR/_ViewDWSAdvanced", model);
            }
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult EditDWS(DWSModel model)
        {
            ViewBag.IsEditAllowed = true;
            if (!ModelState.IsValid)
            {
                return PartialView("STR/_EditDWS", model);
            }

            DomainEntities.DWS origRec = model.OrigRec;

            DomainEntities.EntityWorkFlow entityWFRec = model.EntityWFRec;
            //if (model.ActivityId == 0 && origRec.Agreement != model.Agreement)
            //{
            //    ICollection<DomainEntities.EntityWorkFlow> entityWorkFlows =
            //                Business.GetEntityWorkFlow(model.Agreement);

            //    entityWFRec = entityWorkFlows.First();
            //}

            // Detect change
            if (origRec.DWSNumber == model.DWSNumber &&
                origRec.BagCount == model.BagCount &&
                origRec.FilledBagsWeightKg == model.FilledBagsWeightKg &&
                origRec.EmptyBagsWeightKg == model.EmptyBagsWeightKg &&
                origRec.DWSDate == model.DWSDate &&
                origRec.Comments.Equals(model.Comments) &&
                origRec.Agreement.Equals(model.Agreement)
                )
            {
                ;
            }
            else
            {
                origRec.ChangeDWSData = true;
                origRec.DWSNumber = model.DWSNumber;
                origRec.BagCount = model.BagCount;
                origRec.FilledBagsWeightKg = model.FilledBagsWeightKg;
                origRec.EmptyBagsWeightKg = model.EmptyBagsWeightKg;
                origRec.DWSDate = model.DWSDate;
                origRec.Comments = Utils.TruncateString(model.Comments, 100);
                origRec.StrTagCyclicCount = model.StrTagCyclicCount;

                if (entityWFRec != null)
                {
                    origRec.ChangeAgreementDetails = true;
                    origRec.EntityId = entityWFRec.EntityId;
                    origRec.EntityName = entityWFRec.EntityName;
                    origRec.AgreementId = entityWFRec.AgreementId;
                    origRec.Agreement = entityWFRec.Agreement;
                    origRec.EntityWorkFlowDetailId = 0;
                    origRec.TypeName = entityWFRec.TypeName;
                    origRec.TagName = "";
                }
            }

            if (!origRec.STRNumber.Equals(model.STRNumber, StringComparison.OrdinalIgnoreCase))
            {
                var rec = Business.GetSTRTag(model.STRNumber);
                origRec.ChangeSTRNumber = true;
                origRec.MoveToStrTagId = rec.Id;
            }

            origRec.CurrentUser = CurrentUserStaffCode;

            try
            {
                DBSaveStatus status = Business.SaveDWSData(origRec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView("STR/_EditDWS", model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditDWS), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("STR/_EditDWS", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRWeighControl)]
        public ActionResult EditSTRWeight(long strWeightId)
        {
            DomainEntities.STRWeight rec = Business.GetSingleSTRWeight(strWeightId);

            ViewBag.IsEditAllowed = rec.IsEditAllowed;

            STRWeightModel model =
                         new STRWeightModel()
                         {
                             Id = rec.Id,
                             STRNumber = rec.STRNumber,
                             STRDate = rec.STRDate,
                             EntryWeight = rec.EntryWeight,
                             ExitWeight = rec.ExitWeight,
                             SiloNumber = rec.SiloNumber,
                             SiloIncharge = rec.SiloIncharge,
                             UnloadingIncharge = rec.UnloadingIncharge,
                             ExitOdometer = rec.ExitOdometer,
                             BagCount = rec.BagCount,
                             Notes = rec.Notes,
                             Status = rec.Status,
                             DeductionPercent = rec.DeductionPercent,
                             CyclicCount = rec.CyclicCount,
                             IsEditAllowed = rec.IsEditAllowed,
                             DWSCount = rec.DWSCount,
                             VehicleNumber = rec.VehicleNumber
                         };

            model.STRDateAsText = model.STRDate.ToString("dd-MM-yyyy");
            FillEditSTRWeightViewBag();
            return PartialView("STRWeigh/_Edit", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRWeighControl)]
        public ActionResult EditSTRWeight(STRWeightModel model)
        {
            FillEditSTRWeightViewBag();
            ViewBag.IsEditAllowed = true;
            if (!ModelState.IsValid)
            {
                return PartialView("STRWeigh/_Edit", model);
            }

            DomainEntities.STRWeight rec = (model.Id > 0) ?
                                Business.GetSingleSTRWeight(model.Id) :
                                new DomainEntities.STRWeight();

            rec.STRNumber = Utils.TruncateString(model.STRNumber, 50);
            rec.STRDate = model.STRDate;
            rec.EntryWeight = model.EntryWeight;
            rec.ExitWeight = model.ExitWeight;
            rec.SiloNumber = model.SiloNumber;
            rec.SiloIncharge = Utils.TruncateString(model.SiloIncharge, 50);
            rec.UnloadingIncharge = Utils.TruncateString(model.UnloadingIncharge, 50);
            rec.ExitOdometer = model.ExitOdometer;
            rec.BagCount = model.BagCount;
            rec.Notes = Utils.TruncateString(model.Notes, 1000);

            rec.DWSCount = model.DWSCount;
            rec.VehicleNumber = Utils.TruncateString(model.VehicleNumber, 50);

            rec.CurrentUser = CurrentUserStaffCode;

            rec.DeductionPercent = model.DeductionPercent;
            rec.CyclicCount = model.CyclicCount;

            rec.Status = (model.Id > 0) ? rec.Status : STRDWSStatus.Pending.ToString();

            try
            {
                DBSaveStatus status = Business.SaveSTRWeightData(rec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    ModelState.AddModelError("", "An error occured while saving changes. Please refresh the page and try again.");
                    return PartialView("STRWeigh/_Edit", model);
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(EditSTRWeight), ex.ToString(), ">");
                ModelState.AddModelError("", ex.Message);
                return PartialView("STRWeigh/_Edit", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRWeighControl)]
        public ActionResult AddSTRWeight()
        {
            STRWeightModel model =
                         new STRWeightModel()
                         {
                             Id = 0,
                             STRNumber = "",
                             STRDate = Helper.GetCurrentIstDateTime(),
                             CyclicCount = 0
                         };

            ViewBag.IsEditAllowed = true;
            model.STRDateAsText = model.STRDate.ToString("dd-MM-yyyy");
            FillEditSTRWeightViewBag();
            return PartialView("STRWeigh/_Edit", model);
        }


        [CheckRightsAuthorize(Feature = FeatureEnum.STRWeighControl)]
        public ActionResult STRWeigh()
        {
            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("STRWeigh/Index");
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.DWSApproveWeightOption)]
        public ActionResult DWSApproveWeight()
        {
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            ViewBag.AvailableDWSStatus = new List<string>()
            {
                STRDWSStatus.SiloChecked.ToString(),
                STRDWSStatus.WeightApproved.ToString()
            };

            ViewBag.SearchResultAction = nameof(GetSearchDWSForWeightApproval);
            ViewBag.Title = "Approve DWS Wt.";

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("STRApprove");
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.DWSApproveAmountOption)]
        public ActionResult DWSApproveAmount()
        {
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            ViewBag.AvailableDWSStatus = new List<string>()
            {
                STRDWSStatus.WeightApproved.ToString(),
                STRDWSStatus.AmountApproved.ToString()
            };

            ViewBag.SearchResultAction = nameof(GetSearchDWSForAmountApproval);
            ViewBag.Title = "Approve DWS Amount";

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("STRApprove");
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.DWSPaymentOption)]
        public ActionResult DWSPreparePaymentFile()
        {
            ViewBag.OfficeHierarchy = GetOfficeHierarchy();

            ViewBag.AvailableDWSStatus = new List<string>()
            {
                STRDWSStatus.AmountApproved.ToString(),
                STRDWSStatus.Paid.ToString(),
            };

            ViewBag.SearchResultAction = nameof(GetSearchDWSForPayment);
            ViewBag.Title = "Make DWS Payment";

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("STRApprove");
        }

        [CheckRightsAuthorize(Feature = FeatureEnum.DWSPaymentOption)]
        public ActionResult DWSDownloadPaymentFile()
        {
            ViewBag.SearchResultAction = nameof(GetPaymentReferences);
            ViewBag.DownloadAction = nameof(DownloadPaymentReference);
            ViewBag.Title = "Download Payment File";

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("DWSDownloadPaymentFile/Index");
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult GetSearchSTR(STRFilter searchCriteria)
        {
            DomainEntities.STRFilter criteria = Helper.ParseSTRSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<STRTag> items = Business.GetSTRTag(criteria);

            ICollection<STRTagModel> model =
                         items.Select(x => new STRTagModel()
                         {
                             Id = x.Id,
                             STRNumber = x.STRNumber,
                             STRDate = x.STRDate,
                             DWSCount = x.DWSCount.HasValue ? x.DWSCount.Value : 0,
                             BagCount = x.BagCount.HasValue ? x.BagCount.Value : 0,
                             GrossWeight = x.GrossWeight.HasValue ? x.GrossWeight.Value : 0,
                             NetWeight = x.NetWeight.HasValue ? x.NetWeight.Value : 0,
                             STRCount = x.STRCount.HasValue ? x.STRCount.Value : 0,
                             Status = x.Status,
                             IsEditAllowed = x.IsEditAllowed
                         }).ToList();

            return PartialView("STR/_ListSTRTags", model);
        }

        
        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRWeighControl)]
        public ActionResult GetSearchSTRWeigh(STRFilter searchCriteria)
        {
            DomainEntities.STRFilter criteria = Helper.ParseSTRSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<STRWeight> items = Business.GetSTRWeight(criteria);

            ICollection<STRWeightModel> model =
                         items.Select(x => new STRWeightModel()
                         {
                             Id = x.Id,
                             STRNumber = x.STRNumber,
                             STRDate = x.STRDate,
                             EntryWeight = x.EntryWeight,
                             ExitWeight = x.ExitWeight,
                             SiloNumber = x.SiloNumber,
                             SiloIncharge = x.SiloIncharge,
                             UnloadingIncharge = x.UnloadingIncharge,
                             ExitOdometer = x.ExitOdometer,
                             BagCount = x.BagCount,
                             Notes = x.Notes,
                             Status = x.Status,
                             DeductionPercent = x.DeductionPercent,
                             IsEditAllowed = x.IsEditAllowed,
                             DWSCount = x.DWSCount,
                             VehicleNumber = x.VehicleNumber
                         }).ToList();

            return PartialView("STRWeigh/_List", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.DWSApproveWeightOption)]
        public ActionResult GetSearchDWSForWeightApproval(DWSFilter searchCriteria)
        {
            DomainEntities.DWSFilter criteria = Helper.ParseDWSSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<DWS> items = Business.GetDWS(criteria);

            ICollection<DWSModel> model = items.Select(x => CreateDWSModel(x)).ToList();

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            return PartialView("DWSApproveWeight/_List", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.DWSApproveAmountOption)]
        public ActionResult GetSearchDWSForAmountApproval(DWSFilter searchCriteria)
        {
            DomainEntities.DWSFilter criteria = Helper.ParseDWSSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<DWS> items = Business.GetDWS(criteria);

            ICollection<DWSModel> model = items.Select(x => CreateDWSModel(x)).ToList();

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            return PartialView("DWSApproveAmount/_List", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.DWSPaymentOption)]
        public ActionResult GetSearchDWSForPayment(DWSFilter searchCriteria)
        {
            DomainEntities.DWSFilter criteria = Helper.ParseDWSSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            IEnumerable<DWS> items = Business.GetDWS(criteria);

            ICollection<DWSModel> model = items.Select(x => CreateDWSModel(x)).ToList();

            ViewBag.OfficeHierarchy = GetOfficeHierarchy();
            return PartialView("DWSMakePayment/_List", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.DWSPaymentOption)]
        public ActionResult GetPaymentReferences(PaymentReferenceFilter searchCriteria)
        {
            DomainEntities.PaymentReferenceFilter criteria = Helper.ParsePaymentRefSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            ICollection<DWSPaymentReference> items = Business.GetPaymentReferences(criteria);

            ICollection<DWSPaymentReferenceModel> model = items.Select(x => new DWSPaymentReferenceModel()
            {
                Id = x.Id,
                Comments = x.Comments,
                DWSCount = x.DWSCount,
                DWSNumbers = x.DWSNumbers,
                PaymentReference = x.PaymentReference,
                TotalNetPayable = x.TotalNetPayable,
                CreatedBy = x.CreatedBy,
                DateCreated = x.LocalTimeStamp,

                AccountNumber = x.AccountNumber,
                AccountName = x.AccountName,
                AccountAddress = x.AccountAddress,
                AccountEmail = x.AccountEmail,
                PaymentType = x.PaymentType,
                SenderInfo = x.SenderInfo

            }).ToList();

            return PartialView("DWSDownloadPaymentFile/_List", model);
        }

        


        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.DWSPaymentOption)]
        public ActionResult DownloadPaymentReference(PaymentReferenceFilter searchCriteria)
        {
            DomainEntities.DWSFilter dwsFilter = new DomainEntities.DWSFilter()
            {
                ApplyPaymentReferenceFilter = true,
                IsExactPaymentReferenceMatch = true,
                PaymentReference = Utils.TruncateString(searchCriteria.PaymentReference, 50),
                IsSuperAdmin = true
            };

            // retrieve dws items.
            ICollection<DWS> dwsItems = Business.GetDWS(dwsFilter);

            // retrieve payment reference record as well.
            DomainEntities.PaymentReferenceFilter criteria = Helper.ParsePaymentRefSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            ICollection<DWSPaymentReference> dwsPaymentReferenceItems = Business.GetPaymentReferences(criteria);

            if ((dwsPaymentReferenceItems?.Count ?? 0) != 1)
            {
                Business.LogError($"{nameof(DownloadPaymentReference)}", $"Error: There are not exactly one record for Payment Reference {searchCriteria.PaymentReference}");
                return new EmptyResult();
            }

            DWSPaymentReference singleDwsPaymentReference = dwsPaymentReferenceItems.First();

            int sno = 0;
            // Create Output Model
            IEnumerable<PaymentFileDownloadModel> model = dwsItems.Select(x => new PaymentFileDownloadModel()
            {
                SNo = ++sno,
                BankIFSC = x.BankIFSC,
                NetPayable = x.NetPayable,
                RemitterAccount = singleDwsPaymentReference.AccountNumber,
                RemitterName = singleDwsPaymentReference.AccountName,
                RemitterAddress = singleDwsPaymentReference.AccountAddress,
                BankAccount = x.BankAccount,
                BankAccountName = x.BankAccountName,
                BankName = x.BankName,
                BankBranch = x.BankBranch,
                PaymentDetails = singleDwsPaymentReference.PaymentType,
                SenderToReceiverInfo = singleDwsPaymentReference.SenderInfo,
                RemitterEmail = singleDwsPaymentReference.AccountEmail
            }).ToList();

            return PartialView("DWSDownloadPaymentFile/_Download", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult GetSTR(long Id, string rowId, string parentRowId) // here it is strTagId
        {
            STRTag strTagRec = Business.GetSingleSTRTag(Id);
            ICollection<DomainEntities.STR> items = Business.GetSTR(Id);

            ICollection<STRModel> model =
                         items.Select(x => new STRModel()
                         {
                             Id = x.Id,
                             STRTagId = x.STRTagId,
                             EmployeeId = x.EmployeeId,
                             VehicleNumber = x.VehicleNumber,
                             DriverName = x.DriverName,
                             DriverPhone = x.DriverPhone,
                             DWSCount = x.DWSCount,
                             BagCount = x.BagCount,
                             GrossWeight = x.GrossWeight,
                             NetWeight = x.NetWeight,
                             StartOdometer = x.StartOdometer,
                             EndOdometer = x.EndOdometer,
                             IsNew = x.IsNew,
                             IsTransferred = x.IsTransferred,
                             TransfereeName = x.TransfereeName,
                             TransfereePhone = x.TransfereePhone,
                             ImageCount = x.ImageCount,
                             ActivityId = x.ActivityId,
                             ActivityId2 = x.ActivityId2,
                             EmployeeCode = x.EmployeeCode,
                             EmployeeName = x.EmployeeName,
                             EmployeePhone = x.EmployeePhone,
                             STRNumber = x.STRNumber
                         }).ToList();

            ViewBag.STRTagId = Id;
            ViewBag.STRNumber = strTagRec?.STRNumber ?? "";

            ViewBag.IsEditAllowed = strTagRec.IsEditAllowed;

            ViewBag.RowId = rowId;
            ViewBag.ParentRowId = parentRowId;
            return PartialView("STR/_ListSTR", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.STRFeature)]
        public ActionResult GetDWS(long Id, string rowId, string parentRowId) // here it is strId
        {
            DomainEntities.STR strRec = Business.GetSingleSTR(Id);
            ViewBag.STRRec = strRec;

            STRTag strTagRec = Business.GetSingleSTRTag(strRec.STRTagId);
            ViewBag.STRTagRec = strTagRec;

            ICollection<DomainEntities.DWS> items = Business.GetDWS(Id);
            ICollection<DWSAudit> auditItems = Business.GetDWSAudit(Id);

            ICollection<DWSModel> model = items.Select(x => CreateDWSModel(x)).ToList();

            ViewBag.RowId = rowId;
            ViewBag.ParentRowId = parentRowId;
            ViewBag.AuditItems = auditItems;

            bool isDWSPending = strTagRec.IsEditAllowed;

            if (isDWSPending)
            {
                return PartialView("STR/_ListDWS", model);
            }
            else
            {
                return PartialView("STR/_ListDWSAdvanced", model);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.DWSApproveWeightOption)]
        public ActionResult ApproveDWSWeight(long dwsId)
        {
            DomainEntities.DWS dwsRec = Business.GetSingleDWS(dwsId);

            if (!dwsRec.IsApproveWeightAllowed)
            {
                return PartialView("_CustomError", $"Requested Action is not available on DWS # {dwsRec.DWSNumber}.  Please refresh the page and try again.");
            }

            DWSModel model = CreateDWSModel(dwsRec);

            // retrieve Silo Record
            string siloNotes = Business.GetSTRWeight(model.STRNumber)?.Notes ?? "";
            ViewBag.SiloNotes = siloNotes;

            ViewBag.MaxDWSNotesLength = 500;

            return PartialView("DWSApproveWeight/_Approve", model);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.DWSApproveWeightOption)]
        public ActionResult ApproveDWSWeight(DWSApproveWeightModel model)
        {
            // validations are performed on client side - here just save the record;
            if (!ModelState.IsValid)
            {
                string errors = GetModelErrors();
                return PartialView("_CustomError", $"Validation error occured while saving changes. Please refresh the page and try again. {errors}");
            }

            DomainEntities.DWS origRec = model.OrigRec;

            origRec.CyclicCount = model.CyclicCount;
            origRec.Comments = Utils.TruncateString(model.Comments, 500);
            origRec.SiloDeductWtOverride = model.SiloDeductWtOverride;
            origRec.CurrentUser = CurrentUserStaffCode;

            try
            {
                DBSaveStatus status = Business.SaveDWSApprovedWeightData(origRec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    return PartialView("_CustomError", "An error occured while saving changes. Please refresh the page and try again.");
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(ApproveDWSWeight), ex.ToString(), ">");
                return PartialView("_CustomError", ex.Message);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.DWSApproveAmountOption)]
        public ActionResult ApproveDWSAmount(long dwsId)
        {
            DomainEntities.DWS dwsRec = Business.GetSingleDWS(dwsId);

            if (!dwsRec.IsApproveAmountAllowed)
            {
                return PartialView("_CustomError", $"Requested Action is not available on DWS # {dwsRec.DWSNumber}.  Please refresh the page and try again.");
            }

            DWSModel dwsModel = CreateDWSModel(dwsRec);

            // retrieve entity bank details
            ICollection<EntityBankDetail> bankAccounts = Business.GetEntityBankDetails(dwsRec.EntityId);

            // count the number of active and approved accounts
            dwsModel.EntityBankAccounts = bankAccounts.Where(x => x.IsActive && x.IsApproved)
                .Select(x => CreateEntityBankDetailModel(x))
                .ToList();

            if (!dwsModel.HasBankAccounts)
            {
                return PartialView("_CustomError", $"Requested Action can't be completed, because the beneficiary '{dwsRec.EntityName}' does not have any approved/active bank account.");
            }

            ViewBag.MaxDWSNotesLength = 500;

            // retrieve all DWS for this Entity Id and Agreement Id
            ViewBag.AllDWS = GetDWS(dwsRec.EntityId, dwsRec.AgreementId);

            // check if issue/return module is enabled
            if (Utils.SiteConfigData.IssueReturnModule)
            {
                // retrieve all Issues/Returns
                ViewBag.IssueReturns = GetIssueReturns(dwsRec.EntityId, dwsRec.AgreementId);
            }

            if (Utils.SiteConfigData.AdvanceRequestModule)
            {
                ViewBag.AdvanceRequests = GetAdvanceRequests(dwsRec.EntityId, dwsRec.AgreementId);
            }

            return PartialView("DWSApproveAmount/_Approve", dwsModel);
        }

        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.DWSApproveAmountOption)]
        public ActionResult ApproveDWSAmount(DWSApproveAmountModel model)
        {
            // validations are performed on client side - here just save the record;
            if (!ModelState.IsValid)
            {
                string errorList = GetModelErrors();
                return PartialView("_CustomError", $"Validation error occured while saving changes. Please refresh the page and try again. {errorList}");
            }

            DomainEntities.DWS origRec = model.OrigRec;

            origRec.CyclicCount = model.CyclicCount;
            origRec.Comments = Utils.TruncateString(model.Comments, 500);
            origRec.DeductAmount = model.DeductAmount;
            origRec.CurrentUser = CurrentUserStaffCode;

            origRec.BankAccountName = model.BeneficiaryBankAccount.AccountHolderName;
            origRec.BankName = model.BeneficiaryBankAccount.BankName;
            origRec.BankAccount = model.BeneficiaryBankAccount.BankAccount;
            origRec.BankIFSC = model.BeneficiaryBankAccount.BankIFSC;
            origRec.BankBranch = model.BeneficiaryBankAccount.BankBranch;

            try
            {
                DBSaveStatus status = Business.SaveDWSApprovedAmountData(origRec);
                if (status == DBSaveStatus.CyclicCheckFail)
                {
                    return PartialView("_CustomError", "An error occured while saving changes. Please refresh the page and try again.");
                }
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(ApproveDWSAmount), ex.ToString(), ">");
                return PartialView("_CustomError", ex.Message);
            }
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.DWSPaymentOption)]
        public ActionResult MakeDWSPayment(IEnumerable<long> dwsIds)
        {
            if (dwsIds == null || dwsIds.Count() == 0)
            {
                return PartialView("_CustomError", $"Please specify DWS.");
            }

            //
            // retrieve payment types
            //
            ICollection<CodeTableEx> paymentTypes = Business.GetCodeTable("PaymentType");
            if ((paymentTypes?.Count ?? 0) == 0)
            {
                return PartialView("_CustomError", $"Requested Action can't be completed, because the payment types are not available.");
            }
            ViewBag.PaymentTypes = paymentTypes;

            //
            // retrieve DWS for the Ids passed in
            //
            Tuple<ProcessStatus, long, ICollection<DWS>> tuple = Business.GetDWS(dwsIds);

            if (tuple.Item1 != ProcessStatus.Sucess)
            {
                return PartialView("_CustomError", $"One or more DWS does not exist. (Id {tuple.Item2})");
            }

            // check that all returned DWS are ready to pay
            if (tuple.Item3.Any(x => x.IsReadyToPay == false))
            {
                return PartialView("_CustomError", $"One or more DWS' payment status has changed. Please refresh the page and try again.");
            }

            ICollection<DWSModel> model = tuple.Item3.Select(x => CreateDWSModel(x)).ToList();

            //
            // retrieve remitter's bank account and put in view bag
            //
            DomainEntities.BankAccountFilter sc = Helper.GetDefaultBankAccountFilter();
            IEnumerable<DashboardBankAccount> bankAccounts = Business.GetDashboardBankAccount(sc);

            // count the number of active and approved accounts
            ICollection<BankAccountViewModel> activeBankAccounts = bankAccounts.Where(x => x.IsActive)
                .Select(x => CreateBankAccountViewModel(x))
                .ToList();

            if ((activeBankAccounts?.Count ?? 0) == 0)
            {
                return PartialView("_CustomError", $"Requested Action can't be completed, because Remiiter's bank account does not exist.");
            }

            ViewBag.RemitterBankAccounts = activeBankAccounts;

            //
            // View
            //
            ViewBag.MaxNotesLength = 100;

            return PartialView("DWSMakePayment/_Payment", model);
        }

        
        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.DWSPaymentOption)]
        public ActionResult MakeDWSPayment(DWSMakePaymentModel model)
        {
            //Request.InputStream.Position = 0;
            //var s = new StreamReader(Request.InputStream).ReadToEnd();

            if (!ModelState.IsValid)
            {
                string errorList = GetModelErrors();
                return PartialView("_CustomError", $"Validation error occured while making DWS payments. Please refresh the page and try again. {errorList}");
            }

            DWSPaymentReference dwsPaymentReference = new DWSPaymentReference()
            {
                Comments = model.Comments,
                PaymentReference = model.PaymentReference,
                DWSCount = model.DWSPayments.Count(),
                TotalNetPayable = model.TotalNetAmount,
                DWSNumbers = Utils.TruncateString(model.DWSNumbers, 2000),
                CurrentUser = CurrentUserStaffCode,

                AccountNumber = Utils.TruncateString(model.RemitterBankAccount.AccountNumber, 50),
                AccountName = Utils.TruncateString(model.RemitterBankAccount.AccountName, 50),
                AccountAddress = Utils.TruncateString(model.RemitterBankAccount.AccountAddress, 50),
                AccountEmail = Utils.TruncateString(model.RemitterBankAccount.AccountEmail, 50),
                PaymentType = Utils.TruncateString(model.PaymentType, 50),
                SenderInfo = Utils.TruncateString(model.SenderInfo, 50),
                LocalTimeStamp = Helper.GetCurrentIstDateTime(),
            };

            try
            {
                Business.CreateDWSPaymentReference(dwsPaymentReference);

                // Mark each of the record as paid
                var dwsRecs = model.DWSPayments.Select(x => new DWS()
                {
                    Id = x.Id,
                    CyclicCount = x.CyclicCount,
                    CurrentUser = CurrentUserStaffCode,
                    PaymentReference = model.PaymentReference,
                    Status = STRDWSStatus.Paid.ToString()
                }).ToList();

                Business.MarkDWSAsPaid(dwsRecs);

                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(MakeDWSPayment), ex.ToString(), ">");
                return PartialView("_CustomError", ex.Message);
            }
        }

        private void FillEditSTRViewBag()
        {
            ICollection<DomainEntities.Transporter> transporterData = Business.GetTransporterData();
            ViewBag.Vehicles = transporterData
                .OrderBy(x => x.CompanyName)
                .ThenBy(x => x.VehicleNo).ToList();
        }
        
        private void FillEditSTRWeightViewBag()
        {
            ICollection<DomainEntities.Transporter> transporterData = Business.GetTransporterData();
            ViewBag.Vehicles = transporterData
                .OrderBy(x => x.CompanyName)
                .ThenBy(x => x.VehicleNo).ToList();
        }

        // here we are checking, if the STR Record on Weighment page is in pending state.
        // If it is marked as Final, then edit on STR Page for this STR # is not allowed;
        private bool IsStrEditAllowed(string strNumber)
        {
            STRTag rec = Business.GetSTRTag(strNumber);

            return (rec == null) ? false : rec.IsEditAllowed;
        }

        private bool IsStrEditAllowed(long strTagId, STRTag strTagRec = null)
        {
            if (strTagRec == null)
            {
                strTagRec = Business.GetSingleSTRTag(strTagId);
            }

            return strTagRec.IsEditAllowed;
        }

        private static DWSModel CreateDWSModel(DWS inputRec)
        {
            return new DWSModel()
            {
                Id = inputRec.Id,
                STRId = inputRec.STRId,
                STRTagId = inputRec.STRTagId,
                DWSNumber = inputRec.DWSNumber,
                DWSDate = inputRec.DWSDate,
                BagCount = inputRec.BagCount,
                FilledBagsWeightKg = inputRec.FilledBagsWeightKg,
                EmptyBagsWeightKg = inputRec.EmptyBagsWeightKg,
                EntityId = inputRec.EntityId,
                EntityName = inputRec.EntityName,
                AgreementId = inputRec.AgreementId,
                Agreement = inputRec.Agreement,
                EntityWorkFlowDetailId = inputRec.EntityWorkFlowDetailId,
                TypeName = inputRec.TypeName,
                TagName = inputRec.TagName,
                ActivityId = inputRec.ActivityId,
                STRNumber = inputRec.STRNumber,
                StrTagCyclicCount = inputRec.StrTagCyclicCount,

                OrigBagCount = inputRec.OrigBagCount,
                OrigFilledBagsKg = inputRec.OrigFilledBagsKg,
                OrigEmptyBagsKg = inputRec.OrigEmptyBagsKg,
                Comments = inputRec.Comments,
                Status = inputRec.Status,

                IsPendingStatus = inputRec.IsPendingStatus,
                IsApproveAllowed = inputRec.IsApproveWeightAllowed,

                SiloDeductPercent = inputRec.SiloDeductPercent,
                GoodsWeight = inputRec.GoodsWeight,
                SiloDeductWt = inputRec.SiloDeductWt,
                SiloDeductWtOverride = inputRec.SiloDeductWtOverride,
                NetPayableWt = inputRec.NetPayableWt,
                RatePerKg = inputRec.RatePerKg,
                GoodsPrice = inputRec.GoodsPrice,
                DeductAmount = inputRec.DeductAmount,
                NetPayable = inputRec.NetPayable,

                WtApprovedBy = inputRec.WtApprovedBy,
                WtApprovedDateAsText = (inputRec.WtApprovedDate.HasValue ? inputRec.WtApprovedDate.Value.ToString("dd-MM-yyyy") : ""),
                AmountApprovedBy = inputRec.AmountApprovedBy,
                AmountApprovedDateAsText = (inputRec.AmountApprovedDate.HasValue ? inputRec.AmountApprovedDate.Value.ToString("dd-MM-yyyy") : ""),
                PaidBy = inputRec.PaidBy,
                PaidDateAsText = (inputRec.PaidDate.HasValue ? inputRec.PaidDate.Value.ToString("dd-MM-yyyy") : ""),
                PaymentReference = inputRec.PaymentReference,

                HQCode = inputRec.HQCode,
                CyclicCount = inputRec.CyclicCount,
                DWSDateAsText = inputRec.DWSDate.Date.ToString("dd-MM-yyyy"),

                BankAccountName = inputRec.BankAccountName,
                BankName = inputRec.BankName,
                BankAccount = inputRec.BankAccount,
                BankIFSC = inputRec.BankIFSC,
                BankBranch = inputRec.BankBranch
            };
        }

        private static ICollection<DWSModel> GetDWS(long entityId, long agreementId)
        {
            DomainEntities.DWSFilter dwsSearchCriteria = Helper.GetDefaultDWSFilter();
            if (entityId > 0)
            {
                dwsSearchCriteria.ApplyEntityIdFilter = true;
                dwsSearchCriteria.EntityId = entityId;
            }

            if (agreementId > 0)
            {
                dwsSearchCriteria.ApplyAgreementIdFilter = true;
                dwsSearchCriteria.AgreementId = agreementId;
            }

            ICollection<DWS> allDWS = Business.GetDWS(dwsSearchCriteria);
            return allDWS.Select(x => CreateDWSModel(x)).ToList();
        }

        #region TRANSPORTER PAYMENT MODULE

        // SA - 31/05/2021 - Add or Approve Vendor Payment Search Criteria | ModifiedBy : Kartik
        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult AddOrApproveVendorPayment()
        {
            ViewBag.Seasons = Business.GetSeasonNames();
            ViewBag.AvailableSTRPaymentStatus = new List<string>()
            {
                STRPaymentStatus.Pending.ToString(),
                STRPaymentStatus.AwaitingApproval.ToString()
            };
            ViewBag.SearchResultAction = nameof(GetSearchSTRForApproval);
            ViewBag.Title = "Add or Approve Transporter Payment";
            return View("VendorPayment/Index");
        }


        // SA - 31/05/2021 - Add or Approve Vendor Payment: Load search result with STR Status as Silo Checked | ModifiedBy : Kartik
        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult GetSearchSTRForApproval(VendorSTRFilter searchCriteria)
        {
            DomainEntities.VendorSTRFilter criteria = Helper.ParseSTRSearchCriteriaApproval(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            ICollection<VendorSTR> items = Business.GetSearchSTRForApproval(criteria);

            ICollection<VendorSTRModel> model =
                        items.Select(x => new VendorSTRModel()
                        {
                            STRTagId = x.STRTagId,
                            STRDate = x.STRDate,
                            STRNumber = x.STRNumber,
                            VehicleNumber = x.VehicleNumber,
                            VendorName = x.VendorName,
                            NumberOfBags = x.NumberOfBags,
                            StartOdo = x.StartOdo,
                            EndOdo = x.EndOdo,
                            SiloToShedKms = x.SiloToShedKms,
                            CostPerKm = x.CostPerKm,
                            VehicleCapacity = x.VehicleCapacity,
                            CostPerExtraTon = x.CostPerExtraTon,
                            STRStatus = x.STRStatus,
                            IsEditAllowed = x.IsEditAllowed,
                            //GrossWeight = x.GrossWeight,
                            NetWeight = x.NetWeight,
                            EntryWeight = x.EntryWeight,
                            ExitWeight = x.ExitWeight,
                            LoadedWeight = x.EntryWeight - x.ExitWeight,
                            HamaliRatePerBag = x.HamaliRatePerBag,
                            

                            ShedOdo = x.ShedOdo,
                            TotalRunningKms = x.TotalRunningKms,
                            TransportationCharges = x.TransportationCharges,
                            ExtraTonnage = x.ExtraTonnage,
                            ExtraTonCharges = x.ExtraTonCharges,
                            TollCharges = x.TollCharges.HasValue ? x.TollCharges : 0,
                            WeighmentCharges = x.WeighmentCharges.HasValue ? x.WeighmentCharges : 0,
                            Others = x.Others.HasValue ? x.Others : 0,
                            HamaliCharges = x.HamaliCharges.HasValue ? x.HamaliCharges : 0,
                            NetPayableAmount = x.NetPayableAmount.HasValue ? x.NetPayableAmount : 0,
                            Comments = x.Comments,
                            PaymentStatus = String.IsNullOrEmpty(x.PaymentStatus) ? "Pending" : x.PaymentStatus

                        }).ToList();


            return PartialView("VendorPayment/_VendorListSTR", model);
        }

        // SA - 31/05/2021 - Add or Approve Vendor Payment: Load popup to add additional transporter data | ModifiedBy : Kartik
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult AddTransporterData(long strTagId)
        {
            ViewBag.MaxSTRNotesLength = 500;
            ICollection<VendorSTR> items = Business.GetSingleSTRDetails(strTagId);

            var model =
                items.Select(x => new VendorSTRModel()
                {
                    STRTagId = x.STRTagId,
                    STRNumber = x.STRNumber,
                    StartOdo = x.StartOdo,
                    EndOdo = x.EndOdo,
                    SiloToShedKms = x.SiloToShedKms,
                    CostPerKm = x.CostPerKm,
                    VehicleCapacity = x.VehicleCapacity,
                    CostPerExtraTon = x.CostPerExtraTon,
                    NumberOfBags = x.NumberOfBags,
                    HamaliRatePerBag = x.HamaliRatePerBag,
                    HamaliCharges = x.NumberOfBags * x.HamaliRatePerBag,
                    //GrossWeight = x.GrossWeight,
                    NetWeight = x.NetWeight,
                    EntryWeight = x.EntryWeight,
                    ExitWeight = x.ExitWeight,
                    ExtraTonnage = x.NetWeight > x.VehicleCapacity ? x.NetWeight - x.VehicleCapacity : 0,
                    LoadedWeight = x.EntryWeight - x.ExitWeight,

                    ShedOdo = x.ShedOdo.HasValue ? x.ShedOdo : 0,
                    TollCharges = x.TollCharges.HasValue ? x.TollCharges : 0,
                    WeighmentCharges = x.WeighmentCharges.HasValue ? x.WeighmentCharges : 0,
                    Others = x.Others.HasValue ? x.Others : 0,
                    Comments = x.Comments

                }).FirstOrDefault();

            return PartialView("VendorPayment/_AddTransporterData", model);
        }

        // SA - 31/05/2021 - Add or Approve Vendor Payment: Add additional transporter data | ModifiedBy : Kartik
        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult AddTransporterData(VendorSTR model)
        {
            if(model.ShedOdo == null || model.TollCharges == null || model.WeighmentCharges == null || model.Others == null)
            {
                return PartialView("_CustomError", "Please enter valid transporter data for STR Number : " + model.STRNumber);
            }
            VendorSTRPayment strPayRec = new VendorSTRPayment()
            {
                STRTagId = model.STRTagId,
                STRNumber = model.STRNumber,
                ShedOdo = model.ShedOdo,
                StartOdo = model.StartOdo,
                EndOdo = model.EndOdo,
                SiloToShedKms = model.SiloToShedKms,
                TotalRunningKms = model.TotalRunningKms,
                CostPerKM = model.CostPerKm,
                TransportationCharges = model.TransportationCharges,
                VehicleCapacity = model.VehicleCapacity,
                //GrossWeight = model.GrossWeight,
                NetWeight = model.NetWeight,
                LoadedWeight = model.LoadedWeight,
                ExtraTonnage = model.ExtraTonnage,
                CostPerExtraTon = model.CostPerExtraTon,
                ExtraTonCharges = model.ExtraTonCharges,
                TollCharges = model.TollCharges,
                WeighmentCharges = model.WeighmentCharges,
                Others = model.Others,
                NumberOfBags = model.NumberOfBags,
                HamaliRatePerBag = model.HamaliRatePerBag,
                HamaliCharges = model.HamaliCharges,
                NetPayableAmount = model.NetPayableAmount,
                Comments = model.Comments,
                PaymentStatus = STRPaymentStatus.AwaitingApproval.ToString(),
                DateCreated = DateTime.Now,
                DateUpdated = DateTime.Now,
                CreatedBy = CurrentUserStaffCode,
                UpdatedBy = CurrentUserStaffCode
            };

            try
            {
                Business.CreateSTRPayment(strPayRec);

                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(AddTransporterData), ex.ToString(), ">");
                return PartialView("_CustomError", ex.Message);
            }

        }

        // SA - 31/05/2021 - Add or Approve Vendor Payment: Approve STRs with STR Payment status as Awaiting Approval | ModifiedBy : Kartik
        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult ApproveSTRForPayment(IEnumerable<long> strTagIds)
        {
            var strTagIdsList = strTagIds.ToList();
            try
            {
                ICollection<VendorSTR> payRec = Business.GetSTRPaymentDataForApproval(strTagIds);
                foreach(long id in strTagIds)
                {
                    var strDetails = payRec.Where(x => x.STRTagId == id).FirstOrDefault();
                    if(strDetails == null)
                    {
                        return PartialView("_CustomError", "Selected STR/STR's are in Pending Status. Kindly complete the Review Process.");
                    }
                    var bankDetails = payRec.Where(x => x.STRTagId == id && x.IsActive == true).FirstOrDefault();
                    if (bankDetails == null || String.IsNullOrEmpty(bankDetails.AccountNumber) || String.IsNullOrEmpty(bankDetails.IFSC))
                    {
                        return PartialView("_CustomError", "Bank details are not available for one of the selected STRs.");
                    }
                }

                var payRecData =
                        payRec.Select(x => new VendorSTRPayment()
                        {
                            STRTagId = x.STRTagId,
                            BankName = x.BankName,
                            BankBranch = x.BankBranch,
                            AccountHolderName = x.AccountName,
                            BankAccount = x.AccountNumber,
                            BankIFSC = x.IFSC,
                        }).ToList();
                Business.MarkSTRAsApprovedAddBankDetails(payRecData);
                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(ApproveSTRForPayment), ex.ToString(), ">");
                return PartialView("_CustomError", ex.Message);
            }

        }

        // SA  - 31/05/2021 - Prepare Vendor Payment File: Search Criteria  | ModifiedBy : Kartik
        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult VendorPreparePaymentFile()
        {
            ViewBag.Seasons = Business.GetSeasonNames();
            //ViewBag.AvailableSTRPaymentStatus = Business.GetCodeTable("STRPaymentStatus");
            ViewBag.AvailableSTRPaymentStatus = new List<string>()
            {
                STRPaymentStatus.Approved.ToString(),
                STRPaymentStatus.Paid.ToString()
            };
            ViewBag.SearchResultAction = nameof(GetSearchSTRForPayment);
            ViewBag.Title = "Prepare Transporter Payment File";

            return View("VendorPayment/Index");
        }

        // SA - 31/05/2021 - Prepare Vendor Payment File: Load search result  | ModifiedBy : Kartik
        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult GetSearchSTRForPayment(VendorSTRFilter searchCriteria)
        {
            DomainEntities.VendorSTRFilter criteria = Helper.ParseSTRSearchCriteriaApproval(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            ICollection<VendorSTR> items = Business.GetSearchSTRForPayment(criteria);
            ICollection<VendorSTRModel> model =
                items.Select(x => new VendorSTRModel()
                {
                    STRTagId = x.STRTagId,
                    STRNumber = x.STRNumber,
                    STRDate = x.STRDate,
                    VehicleNumber = x.VehicleNumber,
                    VendorName = x.VendorName,
                    ShedOdo = x.ShedOdo,
                    StartOdo = x.StartOdo,
                    EndOdo = x.EndOdo,
                    SiloToShedKms = x.SiloToShedKms,
                    TotalRunningKms = x.TotalRunningKms,
                    CostPerKm = x.CostPerKm,
                    TransportationCharges = x.TransportationCharges,
                    VehicleCapacity = x.VehicleCapacity,
                    //GrossWeight = x.GrossWeight,
                    NetWeight = x.NetWeight,
                    LoadedWeight = x.LoadedWeight,
                    ExtraTonnage = x.ExtraTonnage,
                    CostPerExtraTon = x.CostPerExtraTon,
                    ExtraTonCharges = x.ExtraTonCharges,
                    TollCharges = x.TollCharges,
                    WeighmentCharges = x.WeighmentCharges,
                    NumberOfBags = x.NumberOfBags,
                    HamaliRatePerBag = x.HamaliRatePerBag,
                    HamaliCharges = x.HamaliCharges,
                    Others = x.Others,
                    NetPayableAmount = x.NetPayableAmount,
                    PaymentStatus = x.PaymentStatus,
                    AccountNumber = x.AccountNumber,
                    AccountName = x.AccountName,
                    BankName = x.BankName,
                    IFSC = x.IFSC,
                    BankBranch = x.BankBranch,
                    PaymentReference = x.PaymentReference,
                    Comments = x.Comments
                }).ToList();

            return PartialView("VendorPayment/_VendorListSTRApproved", model);
        }

        // SA - 31/05/2021 - Prepare Vendor Payment File: Load payment details for selected STRs  | ModifiedBy : Kartik
        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult MakeSTRPayment(IEnumerable<long> strTagIds)
        {
            if (strTagIds == null || strTagIds.Count() == 0)
            {
                return PartialView("_CustomError", $"Please specify STR.");
            }

            ICollection<CodeTableEx> paymentTypes = Business.GetCodeTable("PaymentType");
            if ((paymentTypes?.Count ?? 0) == 0)
            {
                return PartialView("_CustomError", $"Requested Action can't be completed, because the payment types are not available.");
            }
            ViewBag.PaymentTypes = paymentTypes;

            Tuple<ProcessStatusSTR, long, ICollection<VendorSTR>> tuple = Business.GetSTRForPayment(strTagIds);

            if (tuple.Item1 != ProcessStatusSTR.Success)
            {
                return PartialView("_CustomError", $"One or more STR does not exist.");
            }

            if (tuple.Item3.Any(x => x.IsReadyToPay == false))
            {
                return PartialView("_CustomError", $"Payment is already done for one or more STR. Please refresh the page and try again.");
            }

            ICollection<VendorSTRModel> model =
                tuple.Item3.Select(x => new VendorSTRModel()
                {
                    STRTagId = x.STRTagId,
                    STRNumber = x.STRNumber,
                    VehicleNumber = x.VehicleNumber,
                    VendorName = x.VendorName,
                    TotalRunningKms = x.TotalRunningKms,
                    VehicleCapacity = x.VehicleCapacity,
                    ExtraTonnage = x.ExtraTonnage,
                    NetPayableAmount = x.NetPayableAmount,

                }).ToList();

            DomainEntities.BankAccountFilter sc = Helper.GetDefaultBankAccountFilter();
            IEnumerable<DashboardBankAccount> bankAccounts = Business.GetDashboardBankAccount(sc);
            // count the number of active and approved accounts
            ICollection<BankAccountViewModel> activeBankAccounts = bankAccounts.Where(x => x.IsActive)
                .Select(x => CreateBankAccountViewModel(x))
                .ToList();

            if ((activeBankAccounts?.Count ?? 0) == 0)
            {
                return PartialView("_CustomError", $"Requested Action can't be completed, because Remitter's bank account does not exist.");
            }

            ViewBag.RemitterBankAccounts = activeBankAccounts;
            ViewBag.MaxNotesLength = 100;

            return PartialView("VendorPayment/_VendorPayment", model);
        }

        // SA - 31/05/2021 - Prepare Vendor Payment File: Proceed to payment  | ModifiedBy : Kartik
        [AjaxOnly]
        [HttpPost]
        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult MakeSTRPayment(STRMakePaymentModel model)
        {
            if (!ModelState.IsValid)
            {
                string errorList = GetModelErrors();
                return PartialView("_CustomError", $"Validation error occured while making STR payments. Please refresh the page and try again. {errorList}");
            }

            STRPaymentReference strPaymentReference = new STRPaymentReference()
            {
                Comments = model.Comments,
                PaymentReference = model.PaymentReference,
                STRCount = model.STRPayments.Count(),
                TotalNetPayable = model.TotalNetAmount,
                STRNumber = Utils.TruncateString(model.STRNumbers, 2000),
                CurrentUser = CurrentUserStaffCode,

                AccountNumber = Utils.TruncateString(model.RemitterBankAccount.AccountNumber, 50),
                AccountName = Utils.TruncateString(model.RemitterBankAccount.AccountName, 50),
                AccountAddress = Utils.TruncateString(model.RemitterBankAccount.AccountAddress, 50),
                AccountEmail = Utils.TruncateString(model.RemitterBankAccount.AccountEmail, 50),
                PaymentType = Utils.TruncateString(model.PaymentType, 50),
                SenderInfo = Utils.TruncateString(model.SenderInfo, 50),
                LocalTimeStamp = Helper.GetCurrentIstDateTime(),
            };
            try
            {
                Business.CreateSTRPaymentReference(strPaymentReference);

                // Mark each of the record as paid
                var strRecs = model.STRPayments.Select(x => new VendorSTR()
                {
                    STRTagId = x.STRTagId,
                    CurrentUser = CurrentUserStaffCode,
                    PaymentReference = model.PaymentReference,
                    PaymentStatus = STRPaymentStatus.Paid.ToString()
                }).ToList();

                Business.MarkSTRAsPaid(strRecs);

                return PartialView("ConfirmMessage");
            }
            catch (Exception ex)
            {
                Business.LogError(nameof(MakeSTRPayment), ex.ToString(), ">");
                return PartialView("_CustomError", ex.Message);
            }


        }


        // SA - 31/05/2021 - Download Vendor Payment File: Search Criteria | ModifiedBy : Kartik
        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult VendorDownloadPaymentFile()
        {
            ViewBag.SearchResultAction = nameof(GetSTRPaymentReferences);
            ViewBag.DownloadAction = nameof(DownloadSTRPaymentReference);
            ViewBag.Title = "Download Transporter Payment File";

            ViewBag.IsSuperAdmin = IsSuperAdmin;
            return View("DWSDownloadPaymentFile/Index");
        }

        // SA - 31/05/2021 - Download Vendor Payment File: Load search result | ModifiedBy : Kartik
        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult GetSTRPaymentReferences(PaymentReferenceFilter searchCriteria)
        {
            DomainEntities.PaymentReferenceFilter criteria = Helper.ParsePaymentRefSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            ICollection<STRPaymentReference> items = Business.GetSTRPaymentReferences(criteria);

            ICollection<VendorPaymentReferenceViewModel> model =
                items.Select(x => new VendorPaymentReferenceViewModel()
                {
                    Id = x.Id,
                    STRCount = x.STRCount,
                    STRNumber = x.STRNumber,
                    Comments = x.Comments,
                    PaymentReference = x.PaymentReference,
                    TotalNetPayable = x.TotalNetPayable,
                    CreatedBy = x.CreatedBy,
                    DateCreated = x.LocalTimeStamp,

                    AccountNumber = x.AccountNumber,
                    AccountName = x.AccountName,
                    AccountAddress = x.AccountAddress,
                    AccountEmail = x.AccountEmail,
                    PaymentType = x.PaymentType,
                    SenderInfo = x.SenderInfo
                }).ToList();

            return PartialView("VendorPayment/_ListPaid", model);
        }

        [AjaxOnly]
        [HttpGet]
        [CheckRightsAuthorize(Feature = FeatureEnum.TransporterPaymentReport)]
        public ActionResult DownloadSTRPaymentReference(PaymentReferenceFilter searchCriteria)
        {
            DomainEntities.VendorSTRFilter strFilter = new DomainEntities.VendorSTRFilter()
            {
                ApplyPaymentReferenceFilter = true,
                IsExactPaymentReferenceMatch = true,
                PaymentReference = Utils.TruncateString(searchCriteria.PaymentReference, 50),
                IsSuperAdmin = true
            };

            ICollection<VendorSTR> items = Business.GetSearchSTRForPayment(strFilter);

            DomainEntities.PaymentReferenceFilter criteria = Helper.ParsePaymentRefSearchCriteria(searchCriteria);
            criteria.CurrentISTTime = Helper.GetCurrentIstDateTime();

            ICollection<STRPaymentReference> strPayRefitems = Business.GetSTRPaymentReferences(criteria);
            if ((strPayRefitems?.Count ?? 0) != 1)
            {
                Business.LogError($"{nameof(DownloadPaymentReference)}", $"Error: There are not exactly one record for Payment Reference {searchCriteria.PaymentReference}");
                return new EmptyResult();
            }

            STRPaymentReference singleItem = strPayRefitems.First();
            int sno = 0;

            IEnumerable<PaymentFileDownloadModel> model = items.Select(x => new PaymentFileDownloadModel()
            {
                SNo = ++sno,
                STRNumber = x.STRNumber,
                BankIFSC = x.IFSC,
                NetPayable = (decimal)x.NetPayableAmount,
                RemitterAccount = singleItem.AccountNumber,
                RemitterName = singleItem.AccountName,
                RemitterAddress = singleItem.AccountAddress,
                BankAccount = x.AccountNumber,
                BankAccountName = x.AccountName,
                BankName = x.BankName,
                BankBranch = x.BankBranch,
                PaymentDetails = singleItem.PaymentType,
                SenderToReceiverInfo = singleItem.SenderInfo,
                RemitterEmail = singleItem.AccountEmail
            }).ToList();

            return PartialView("VendorPayment/_Download", model);

        }

        #endregion

    }
}
