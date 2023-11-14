using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.IServices.Admin.IDM;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IInspectionOfUnitService;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.UnitDetails.AssetDetails
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]
    public class FurnitureInspectionAdController : Controller
    {
        private readonly IInspectionOfUnitService _inspectionOfUnitService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;

        public FurnitureInspectionAdController(IInspectionOfUnitService inspectionOfUnitService, ILogger logger, SessionManager sessionManager)
        {
            _inspectionOfUnitService = inspectionOfUnitService;
            _logger = logger;
            _sessionManager = sessionManager;

        }

        [HttpGet]
        public IActionResult Create(long accountNumber, long InspectionId, byte OffcCd, string LoanSub)
        {

            try
            {
                var registeredSate = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionRegisteredState));
                var furnitureDetails = _sessionManager.GetAllFurnitureInspectionList();
                var inspdetails = _sessionManager.GetAllInspectionDetail();
                var forfirstinspection = inspdetails.FirstOrDefault().DinNo;

                if (furnitureDetails.Count != 0)
                {
                    //ViewBag.SecurityDetails = furnitureDetails.Where(e => e.CreatedDate != null).Last().FurnSec;
                    ViewBag.SecurityDetails = furnitureDetails.Last().FurnSec;

                }
                else
                {
                    ViewBag.SecurityDetails = "";
                }

                if (furnitureDetails.Count != 0)
                {
                    ViewBag.ItemNumber = furnitureDetails.Select(x => x.FurnItemNo).ToList();
                    ViewBag.Eligibility = furnitureDetails.Last().FurnSec;
                    ViewBag.Created = furnitureDetails.Last().FurnSec;
                    ViewBag.Release = furnitureDetails.Last().IrfSecAmt;
                }
                else
                {
                    ViewBag.Eligibility = 0;
                    ViewBag.Release = "";
                }
                
                _logger.Information(CommonLogHelpers.CreateStarted + accountNumber);
                ViewBag.registeredSate = registeredSate;
                ViewBag.InspectionId = InspectionId;
                ViewBag.LoanAcc = accountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.FurnitureresultViewPathAd + Constants.createCS);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdmDChgFurnDTO furnitureInspection)
        {
            Random rand = new Random(); // <-- Make this static somewhere
            const int maxValue = 9999;
            var number = Convert.ToInt64(rand.Next(maxValue + 1).ToString("D4"));
            try
            {
                List<IdmDChgFurnDTO> furnitureInspectionDetails = new();
                if (_sessionManager.GetAllFurnitureInspectionList() != null)
                    furnitureInspectionDetails = _sessionManager.GetAllFurnitureInspectionList();
                IdmDChgFurnDTO list = new IdmDChgFurnDTO();
                list.LoanAcc = furnitureInspection.LoanAcc;
                list.LoanSub = furnitureInspection.LoanSub;
                list.OffcCd = furnitureInspection.OffcCd;
                list.Action = (int)Constant.Create;
                list.UniqueID = Guid.NewGuid().ToString();
                list.FurnIno = furnitureInspection.FurnIno;
                list.FurnItemNo = (int)number;
                list.FurnDetails = furnitureInspection.FurnDetails;
                list.FurnSec = furnitureInspection.FurnSec;
                list.FurnSupp = furnitureInspection.FurnSupp;
                list.FurnSuppAdd1 = furnitureInspection.FurnSuppAdd1;
                list.FurnQty = furnitureInspection.FurnQty;
                list.FurnTax = furnitureInspection.FurnTax;
                list.FurnSat = furnitureInspection.FurnSat;
                list.FurnTotalCost = furnitureInspection.FurnTotalCost;
                list.FurnActualCost = furnitureInspection.FurnActualCost;
                list.FurnAqrdStat = furnitureInspection.FurnAqrdStat;
                list.FurnCletStat = furnitureInspection.FurnCletStat;
                list.FurnReg = furnitureInspection.FurnReg;
                list.Stat = furnitureInspection.Stat;
                list.FurnCost = furnitureInspection.FurnCost;
                list.FurnRequDate = furnitureInspection.FurnRequDate;
                list.FurnSat = furnitureInspection.FurnSat;
                list.FurnStatChangeDate = furnitureInspection.FurnStatChangeDate;
                list.FurnInvoiceNo = furnitureInspection.FurnInvoiceNo;
                list.FurnInvoiceDate = furnitureInspection.FurnInvoiceDate;
                list.FurnDeleiverInWeek = furnitureInspection.FurnDeleiverInWeek;
                list.IrfSecAmt = furnitureInspection.IrfSecAmt;
                list.IrfTotalRelease = furnitureInspection.IrfTotalRelease;
                list.IrfRelStat = Convert.ToInt32(furnitureInspection.IrfRelStat);
                list.IsActive = true;
                list.IsDeleted = false;
                list.DfurnSecRel = furnitureInspection.DfurnSecRel;
                list.Eligibility = furnitureInspection.Eligibility;
                furnitureInspectionDetails.Add(list);
                ViewBag.AccountNumber = list.LoanAcc;
                ViewBag.LoanSub = list.LoanSub;
                ViewBag.OffcCd = list.OffcCd;
                ViewBag.InspectionId = furnitureInspection.FurnIno;
                var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                ViewBag.inspectiondetails = inspectiondetailslist;
                _sessionManager.SetFurnitureInspectionList(furnitureInspectionDetails);
                List<IdmDChgFurnDTO> activeList = furnitureInspectionDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.FurnitureviewPathAd + Constants.ViewAll, activeList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var AllFurnitureInspectionList = _sessionManager.GetAllFurnitureInspectionList();
                IdmDChgFurnDTO FurnitureInspectionList = AllFurnitureInspectionList.FirstOrDefault(x => x.UniqueID == id);
                if (FurnitureInspectionList != null)
                {
                    FurnitureInspectionList.Eligibility = AllFurnitureInspectionList.Last().FurnSec;

                }
                var allRegisteredState = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionRegisteredState));
                ViewBag.RegisteredState = allRegisteredState;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.FurnitureresultViewPathAd + Constants.ViewRecord, FurnitureInspectionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        public IActionResult Edit(long InspectionId, int row, string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var AllFurnitureInspectionList = _sessionManager.GetAllFurnitureInspectionList();
                if (AllFurnitureInspectionList != null)
                {
                    ViewBag.ItemNumber = AllFurnitureInspectionList.Select(x => x.FurnItemNo).ToList();
                }

                IdmDChgFurnDTO furnitureInspectionList = AllFurnitureInspectionList.FirstOrDefault(x => x.UniqueID == id);

                if (furnitureInspectionList != null)
                {
                    furnitureInspectionList.Eligibility = AllFurnitureInspectionList.Last().FurnSec;
                    var allRegisteredState = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionRegisteredState));

                    ViewBag.RegisteredState = allRegisteredState;
                    //ViewBag.InspectionId = furnitureInspectionList.FurnIno;
                    ViewBag.LoanAcc = furnitureInspectionList.LoanAcc;
                    ViewBag.LoanSub = furnitureInspectionList.LoanSub;
                    ViewBag.OffcCd = furnitureInspectionList.OffcCd;
                    ViewBag.Inspectionid = InspectionId;
                    ViewBag.row = row;
                }
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.FurnitureresultViewPathAd + Constants.editCs, furnitureInspectionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmDChgFurnDTO furnitureInspection)
        {
            try
            {
                List<IdmDChgFurnDTO> furnitureInspectionDetails = new();
                if (_sessionManager.GetAllLandInspectionList() != null)
                    furnitureInspectionDetails = _sessionManager.GetAllFurnitureInspectionList();
                IdmDChgFurnDTO furnitureInspectionExist = furnitureInspectionDetails.Find(x => x.UniqueID == id);
                if (furnitureInspectionExist != null)
                {

                    furnitureInspectionDetails.Remove(furnitureInspectionExist);
                    var list = furnitureInspectionExist;
                    list.LoanAcc = furnitureInspection.LoanAcc;
                    list.LoanSub = furnitureInspection.LoanSub;
                    list.OffcCd = furnitureInspection.OffcCd;
                    list.UniqueID = Guid.NewGuid().ToString();
                    list.FurnIno = furnitureInspection.FurnIno;
                    list.FurnItemNo = furnitureInspection.FurnItemNo;
                    list.FurnDetails = furnitureInspection.FurnDetails;
                    list.FurnSec = furnitureInspection.FurnSec;
                    list.FurnSupp = furnitureInspection.FurnSupp;
                    list.FurnSuppAdd1 = furnitureInspection.FurnSuppAdd1;
                    list.FurnQty = furnitureInspection.FurnQty;
                    list.FurnTax = furnitureInspection.FurnTax;
                    list.FurnSat = furnitureInspection.FurnSat;
                    list.FurnTotalCost = furnitureInspection.FurnTotalCost;
                    list.FurnActualCost = furnitureInspection.FurnActualCost;
                    list.FurnCletStat = furnitureInspection.FurnCletStat;
                    list.FurnReg = furnitureInspection.FurnReg;
                    list.Stat = furnitureInspection.Stat;
                    list.FurnCost = furnitureInspection.FurnCost;
                    list.FurnRequDate = furnitureInspection.FurnRequDate;
                    list.FurnSat = furnitureInspection.FurnSat;
                    list.FurnStatChangeDate = furnitureInspection.FurnStatChangeDate;
                    list.FurnInvoiceNo = furnitureInspection.FurnInvoiceNo;
                    list.FurnInvoiceDate = furnitureInspection.FurnInvoiceDate;
                    list.FurnDeleiverInWeek = furnitureInspection.FurnDeleiverInWeek;
                    list.DfurnSecRel = furnitureInspection.DfurnSecRel;
                    list.Eligibility = furnitureInspection.Eligibility;
                    list.IrfSecAmt = furnitureInspection.IrfSecAmt;
                    list.IrfRelStat = Convert.ToInt32(furnitureInspection.IrfRelStat);
                    //list.IrfId = furnitureInspection.IrfId;
                    list.IrfTotalRelease = furnitureInspection.IrfTotalRelease;

                    if (furnitureInspectionExist.Id > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    furnitureInspectionDetails.Add(list);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.InspectionId = list.FurnIno;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                    ViewBag.inspectiondetails = inspectiondetailslist;
                    _sessionManager.SetFurnitureInspectionList(furnitureInspectionDetails);
                    List<IdmDChgFurnDTO> activeList = furnitureInspectionDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();

                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.FurnitureviewPathAd + Constants.ViewAll, activeList) });
                }

                ViewBag.AccountNumber = furnitureInspection.LoanAcc;
                ViewBag.InspectionId = furnitureInspection.FurnIno;
                ViewBag.LoanSub = furnitureInspection.LoanSub;
                ViewBag.OffcCd = furnitureInspection.OffcCd;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.FurnitureviewPathAd + Constants.Edit, furnitureInspection) });
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
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, id));
                IEnumerable<IdmDChgFurnDTO> activeFurn = new List<IdmDChgFurnDTO>();
                var furnitureInspectionList = JsonConvert.DeserializeObject<List<IdmDChgFurnDTO>>(HttpContext.Session.GetString(Constants.SessionAllFurnitureInspectionDetail));
                var itemToRemove = furnitureInspectionList.Find(r => r.UniqueID == id);
                itemToRemove.Action = (int)Constant.Delete;
                itemToRemove.IsDeleted = true;
                itemToRemove.IsActive = false;
                furnitureInspectionList.Add(itemToRemove);
                _sessionManager.SetFurnitureInspectionList(furnitureInspectionList);
                if (furnitureInspectionList.Count != 0)
                {
                    activeFurn = furnitureInspectionList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                }

                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.InspectionId = itemToRemove.FurnIno;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                ViewBag.inspectiondetails = inspectiondetailslist;
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, id));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.FurnitureviewPathAd + Constants.ViewAll , activeFurn), delete = "Canceled" });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

    }
}
