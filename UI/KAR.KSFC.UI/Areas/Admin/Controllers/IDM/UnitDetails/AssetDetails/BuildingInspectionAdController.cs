using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.Razor;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Http;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.UnitDetails.AssetDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    public class BuildingInspectionAdController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;


        public BuildingInspectionAdController(ILogger logger, SessionManager sessionManager)
        {

            _logger = logger;
            _sessionManager = sessionManager;
        }

        [HttpGet]
        public IActionResult Create(long accountNumber, long InspectionId, byte OffcCd, string LoanSub)
        {
            try
            {
                var buildingDetails = _sessionManager.GetAllBuildingInspectionList();
                if (buildingDetails != null)
                {
                    ViewBag.ItemNumber = buildingDetails.Select(x => x.DcBdgItmNo).ToList();
                }
                ViewBag.AccountNumber = accountNumber;
                ViewBag.InspectionId = InspectionId;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;

                var buildingInspectionDetails = _sessionManager.GetAllBuildingInspectionList();
                var inspdetails = _sessionManager.GetAllInspectionDetail();
                var forfirstinspection = inspdetails.FirstOrDefault().DinNo;


                if (buildingDetails.Count != 0)
                {
                    ViewBag.SecurityDetails = buildingDetails.Where(e => e.CreatedDate != null).Last().DcBdgSecCreatd;
                }
                else
                {
                    ViewBag.SecurityDetails = "0";
                }

                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.BuildingInspectionresultViewPathAd + Constants.createCS);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }
        [HttpPost]
        public IActionResult Create(IdmDchgBuildingDetDTO model)
        {
            try
            {
                Random rand = new Random();
                const int maxValue = 9999;
                var number = Convert.ToInt64(rand.Next(maxValue + 1).ToString("D4"));
                if (ModelState.IsValid)
                {
                    List<IdmDchgBuildingDetDTO> buildingInspectionDetails = new();
                    if (_sessionManager.GetAllBuildingInspectionList() != null)
                        buildingInspectionDetails = _sessionManager.GetAllBuildingInspectionList();
                    IdmDchgBuildingDetDTO list = new IdmDchgBuildingDetDTO();
                    list.LoanAcc = model.LoanAcc;
                    list.LoanSub = model.LoanSub;
                    list.OffcCd = model.OffcCd;
                    list.Action = (int)Constant.Create;
                    list.DcBdgPlnth = model.DcBdgPlnth;
                    list.DcBdgUcost = model.DcBdgUcost;
                    list.DcBdgAplnth = model.DcBdgAplnth;
                    list.DcBdgAtCost = model.DcBdgAtCost;
                    list.DcBdgRqrdStat = model.DcBdgRqrdStat;
                    list.DcBdgStat = model.DcBdgStat;
                    list.DcBdgTcost = model.DcBdgTcost;
                    list.DcBdgStatChgDate = model.DcBdgStatChgDate;
                    list.DcBdgSecCreatd = model.DcBdgSecCreatd;
                    list.DcBdgIno = model.DcBdgIno;
                    list.DcBdgItmNo = number;
                    list.DcBdgDets = model.DcBdgDets;
                    list.IrbBldgConstStatus = model.IrbBldgConstStatus;
                    list.IrbPercentage = model.IrbPercentage;
                    list.IrbSecValue = model.IrbSecValue;
                    list.IrbUnitCost = model.IrbUnitCost;
                    list.RoofType = model.RoofType;
                    list.IsActive = true;
                    list.CreatedDate = DateTime.Now;
                    list.IsDeleted = false;
                    list.UniqueId = Guid.NewGuid().ToString();
                    buildingInspectionDetails.Add(list);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.InspectionId = model.DcBdgIno;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                    ViewBag.inspectiondetails = inspectiondetailslist;
                    _sessionManager.SetBuildingInspectionList(buildingInspectionDetails);
                    List<IdmDchgBuildingDetDTO> activeList = buildingInspectionDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.BuildingInspectionviewPathAd + Constants.ViewAll, activeList) });
                }
                ViewBag.AccountNumber = model.LoanAcc;
                ViewBag.InspectionId = model.DcBdgIno;
                ViewBag.LoanSub = model.LoanSub;
                ViewBag.OffcCd = model.OffcCd;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.BuildingInspectionviewPathAd + Constants.Create, model) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.createBuildingInspection + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var allBuildingInspectionList = _sessionManager.GetAllBuildingInspectionList();
                IdmDchgBuildingDetDTO buildingInspectionList = allBuildingInspectionList.FirstOrDefault(x => x.UniqueId == id);
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.BuildingInspectionresultViewPathAd + Constants.ViewRecord, buildingInspectionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpGet]
        public IActionResult Edit(long InspectionId, int row, string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var allBuildingInspectionList = _sessionManager.GetAllBuildingInspectionList();

                if (allBuildingInspectionList != null)
                {
                    ViewBag.ItemNumber = allBuildingInspectionList.Select(x => x.DcBdgItmNo).ToList();
                    ViewBag.SecurityDetails = allBuildingInspectionList.Where(e => e.CreatedDate != null).Last().DcBdgSecCreatd;
                }

                IdmDchgBuildingDetDTO buildingInspectionList = allBuildingInspectionList.FirstOrDefault(x => x.UniqueId == id);
                if (buildingInspectionList != null)
                {
                    ViewBag.AccountNumber = buildingInspectionList.LoanAcc;
                    ViewBag.LoanSub = buildingInspectionList.LoanSub;
                    ViewBag.OffcCd = buildingInspectionList.OffcCd;
                    ViewBag.InspectionId = InspectionId;
                    ViewBag.row = row;

                }
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.BuildingInspectionresultViewPathAd + Constants.editCs, buildingInspectionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmDchgBuildingDetDTO inspection)
        {
            try
            {
                List<IdmDchgBuildingDetDTO> buildingInspectionDetails = _sessionManager.GetAllBuildingInspectionList();
                IdmDchgBuildingDetDTO buildingInspectionExist = buildingInspectionDetails.Find(x => x.UniqueId == id);
                var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                if (buildingInspectionExist != null)
                {

                    buildingInspectionDetails.Remove(buildingInspectionExist);
                    var list = buildingInspectionExist;
                    list.LoanAcc = inspection.LoanAcc;
                    list.LoanSub = inspection.LoanSub;
                    list.OffcCd = inspection.OffcCd;
                    list.DcBdgItmNo = inspection.DcBdgItmNo;
                    list.DcBdgDets = inspection.DcBdgDets;
                    list.DcBdgAplnth = inspection.DcBdgAplnth;
                    list.DcBdgPlnth = inspection.DcBdgPlnth;
                    list.DcBdgAtCost = inspection.DcBdgAtCost;
                    list.DcBdgUcost = inspection.DcBdgUcost;
                    list.DcBdgSecCreatd = inspection.DcBdgSecCreatd;
                    list.DcBdgStat = inspection.DcBdgStat;
                    list.DcBdgStatChgDate = inspection.DcBdgStatChgDate;
                    list.DcBdgRqrdStat = inspection.DcBdgRqrdStat;
                    list.UniqueId = inspection.UniqueId;
                    list.DcBdgIno = inspection.DcBdgIno;
                    list.DcBdgDets = inspection.DcBdgDets;
                    list.IrbSecValue = inspection.IrbSecValue;
                    list.IrbBldgConstStatus = inspection.IrbBldgConstStatus;
                    list.IrbPercentage = inspection.IrbPercentage;
                    list.IrbUnitCost = inspection.IrbUnitCost;
                    list.RoofType = inspection.RoofType;
                    list.ModifiedDate = DateTime.Now;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.IsActive = true;
                    list.IsDeleted = false;

                    if (buildingInspectionExist.DcBdgRowId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    buildingInspectionDetails.Add(list);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.InspectionId = list.DcBdgIno;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.inspectiondetails = inspectiondetailslist;


                    _sessionManager.SetBuildingInspectionList(buildingInspectionDetails);
                    List<IdmDchgBuildingDetDTO> activeList = buildingInspectionDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.BuildingInspectionviewPathAd + Constants.ViewAll, buildingInspectionDetails) });
                }

                ViewBag.AccountNumber = inspection.LoanAcc;
                ViewBag.InspectionId = inspection.DcBdgIno;
                ViewBag.LoanSub = inspection.LoanSub;
                ViewBag.OffcCd = inspection.OffcCd;


                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.BuildingInspectionviewPathAd + Constants.Edit, inspection) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            try
            {
                IEnumerable<IdmDchgBuildingDetDTO> activeBuildingInsp = new List<IdmDchgBuildingDetDTO>();

                var buildingInspectionList = JsonConvert.DeserializeObject<List<IdmDchgBuildingDetDTO>>(HttpContext.Session.GetString(Constants.SessionAllBuildingInspectionList));
                var itemToRemove = buildingInspectionList.Find(r => r.UniqueId == id);
                var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                itemToRemove.Action = (int)Constant.Delete;
                itemToRemove.IsDeleted = true;
                itemToRemove.IsActive = false;

                buildingInspectionList.Add(itemToRemove);

                _sessionManager.SetBuildingInspectionList(buildingInspectionList);
                if (buildingInspectionList.Count != 0)
                {
                    activeBuildingInsp = buildingInspectionList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                }

                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.InspectionId = itemToRemove.DcBdgIno;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                ViewBag.inspectiondetails = inspectiondetailslist;
                string html = Helper.RenderRazorViewToString(this, Constants.BuildingInspectionviewPathAd + Constants.ViewAll, activeBuildingInsp);
                return Json(new { isValid = true, html,delete="Canceled" });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

    }
}
