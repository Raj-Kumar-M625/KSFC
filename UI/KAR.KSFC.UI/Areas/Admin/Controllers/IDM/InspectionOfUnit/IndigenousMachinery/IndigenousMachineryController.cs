using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using KAR.KSFC.Components.Common.Logging.Client;
using KAR.KSFC.UI.Filters;
using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Services.Admin.IDM.InspectionOfUnitService;
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

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.InspectionOfUnit.IndigenousMachinery
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class IndigenousMachineryController : Controller
    {
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;

       

        public IndigenousMachineryController(ILogger logger, SessionManager sessionManager)
        {
            _logger = logger;
            _sessionManager = sessionManager;

        }
        //Author Manoj on 25/08/2022
        [HttpGet]
        public IActionResult Create(long accountNumber, long InspectionId, byte OffcCd, string LoanSub, bool firstinspection)
        {
            
            try
            {
                _logger.Information(CommonLogHelpers.CreateStarted + accountNumber);
                var indeigenousDetails = _sessionManager.GetAllIndigenousMachineryInspectionList();

                if (indeigenousDetails.Count != 0)
                {
                    ViewBag.ItemNumber = indeigenousDetails.Select(x => x.ItemNo).ToList();
                    ViewBag.Eligibility = indeigenousDetails.Last().SecurityCreated;
                    ViewBag.Created = indeigenousDetails.Last().SecurityCreated;
                    ViewBag.Release = indeigenousDetails.Last().IrPlmcSecAmt;
                }
                else
                {
                    ViewBag.Eligibility = 0;
                    ViewBag.Release = "";
                    ViewBag.Created = 0;

                }

            
                var machinarylist = _sessionManager.GetAllMachinaryStatusList();

                var registeredSate = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionRegisteredState));
                var procureList = _sessionManager.GetAllProcureList();
                ViewBag.ProcureList = procureList;
                ViewBag.Machinarystatus = machinarylist;
                ViewBag.registeredSate = registeredSate;
                ViewBag.LoanAcc = accountNumber;
                ViewBag.InspectionId = InspectionId;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;
                ViewBag.firstindigenousMachine = firstinspection;
                var IndigenousMachineryInspectionDetails = _sessionManager.GetAllIndigenousMachineryInspectionList();
                var inspdetails = _sessionManager.GetAllInspectionDetail();
                var forfirstinspection = inspdetails.FirstOrDefault().DinNo;

                if (IndigenousMachineryInspectionDetails.Where(x => x.Ino == InspectionId).Count() > 0 || forfirstinspection == InspectionId)
                {
                
                    ViewBag.firstindigenousMachine = false;
                }

                var previousinspection = inspdetails
                        .OrderByDescending(x => x.DinNo)
                        .Skip(1)
                         .FirstOrDefault();
                if (previousinspection != null && previousinspection.DinNo == InspectionId)
                {
                    ViewBag.firstindigenousMachine = false;
                }

                _logger.Information(CommonLogHelpers.CreateCompleted + accountNumber);
                return View(Constants.IndigenousresultViewPath + Constants.createCS);
            }
            catch (System.Exception ex)

            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        //Author Manoj on 25/08/2022
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(IdmDchgIndigenousInspectionDTO indigenousMachineryInspection)
        {
            Random rand = new Random(); // <-- Make this static somewhere 
            const int maxValue = 9999;
            var number = Convert.ToInt64(rand.Next(maxValue + 1).ToString("D4"));
            try
            {
                if (ModelState.IsValid)
                {
                    List<IdmDchgIndigenousInspectionDTO> indigenousMachineryInspectionDetails = new();
                    if (_sessionManager.GetAllIndigenousMachineryInspectionList() != null)
                        indigenousMachineryInspectionDetails = _sessionManager.GetAllIndigenousMachineryInspectionList();
                    IdmDchgIndigenousInspectionDTO list = new IdmDchgIndigenousInspectionDTO();
                    list.LoanAcc = indigenousMachineryInspection.LoanAcc;
                    list.LoanSub = indigenousMachineryInspection.LoanSub;
                    list.OffcCd = indigenousMachineryInspection.OffcCd;
                    list.ItemNo = (int)number;
                    list.ItemDetails = indigenousMachineryInspection.ItemDetails;
                    list.SupplierName = indigenousMachineryInspection.SupplierName;
                    list.SupplierAddress1 = indigenousMachineryInspection.SupplierAddress1;
                    list.Quantity = indigenousMachineryInspection.Quantity;
                    list.Cost = indigenousMachineryInspection.Cost;
                    list.Tax = indigenousMachineryInspection.Tax;
                    list.TotalCost = indigenousMachineryInspection.TotalCost;
                    list.ActualCost = indigenousMachineryInspection.ActualCost;
                    list.MachineryStatus = indigenousMachineryInspection.MachineryStatus;
                    list.AquiredStatus = indigenousMachineryInspection.AquiredStatus;
                    list.SecurityRelease = indigenousMachineryInspection.SecurityRelease;
                    list.SecurityCreated = indigenousMachineryInspection.SecurityCreated;
                    list.SecurityEligibility = indigenousMachineryInspection.SecurityEligibility;
                    list.RequestDate = indigenousMachineryInspection.RequestDate;
                    list.StatusChangedDate = indigenousMachineryInspection.StatusChangedDate;
                    list.Status = indigenousMachineryInspection.Status;
                    list.InvoiceNo = indigenousMachineryInspection.InvoiceNo;
                    list.InvoiceDate = indigenousMachineryInspection.InvoiceDate;
                    list.Delivery = indigenousMachineryInspection.Delivery;
                    list.Ino = indigenousMachineryInspection.Ino;
                    list.IrPlmcSecAmt = indigenousMachineryInspection.IrPlmcSecAmt;
                    list.IrPlmcAamt = indigenousMachineryInspection.IrPlmcAamt;
                    list.IrPlmcTotalRelease = indigenousMachineryInspection.IrPlmcTotalRelease;
                    list.IrPlmcRelseStat = Convert.ToInt32(indigenousMachineryInspection.IrPlmcRelseStat);
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.RegisteredState = indigenousMachineryInspection.RegisteredState;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.Action = (int)Constant.Create;
                    indigenousMachineryInspectionDetails.Add(list);
                    _sessionManager.SetIndigenousMachineryInspectionList(indigenousMachineryInspectionDetails);
                    List<IdmDchgIndigenousInspectionDTO> activeList = indigenousMachineryInspectionDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.InspectionId = list.Ino;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.firstindigenousMachine = true;
                    var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                    ViewBag.inspectiondetails = inspectiondetailslist;
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.IndigenousviewPath + Constants.ViewAll, indigenousMachineryInspectionDetails) });
                }
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.IndigenousviewPath + Constants.Create, indigenousMachineryInspection) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        //Author Manoj on 01/09/2022
        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var registeredSate = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionRegisteredState));

                var machinarylist = _sessionManager.GetAllMachinaryStatusList();
                var procureList = _sessionManager.GetAllProcureList();
                ViewBag.ProcureList = procureList;
                ViewBag.Machinarystatus = machinarylist;
                ViewBag.registeredSate = registeredSate;
                var AllIndigenousMachineryInspectionList = _sessionManager.GetAllIndigenousMachineryInspectionList();

                IdmDchgIndigenousInspectionDTO indigenousMachineryInspectionList = AllIndigenousMachineryInspectionList.FirstOrDefault(x => x.UniqueId == id);
                if (indigenousMachineryInspectionList != null)
                {
                    indigenousMachineryInspectionList.SecurityEligibility = AllIndigenousMachineryInspectionList.Last().SecurityCreated;

                }
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.IndigenousresultViewPath + Constants.ViewRecord, indigenousMachineryInspectionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        //Author Manoj on 01/09/2022
        public IActionResult Edit(long InspectionId,int row, string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var registeredSate = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString(Constants.SessionRegisteredState));
                ViewBag.registeredSate = registeredSate;
               
                var machinarylist = _sessionManager.GetAllMachinaryStatusList();
                var procureList = _sessionManager.GetAllProcureList();
                ViewBag.ProcureList = procureList;

                ViewBag.Machinarystatus = machinarylist;
                var AllIndigenousMachineryInspectionList = _sessionManager.GetAllIndigenousMachineryInspectionList();
                if (AllIndigenousMachineryInspectionList != null)
                {
                    ViewBag.ItemNumber = AllIndigenousMachineryInspectionList.Select(x => x.ItemNo).ToList();
                }
                IdmDchgIndigenousInspectionDTO indigenousMachineryInspectionList = AllIndigenousMachineryInspectionList.FirstOrDefault(x => x.UniqueId == id);
                if (indigenousMachineryInspectionList != null)
                {
                    indigenousMachineryInspectionList.SecurityEligibility = AllIndigenousMachineryInspectionList.Last().SecurityCreated;

                    //ViewBag.InspectionId = indigenousMachineryInspectionList.Ino;
                    ViewBag.LoanAcc = indigenousMachineryInspectionList.LoanAcc;
                    ViewBag.LoanSub = indigenousMachineryInspectionList.LoanSub;
                    ViewBag.OffcCd = indigenousMachineryInspectionList.OffcCd;
                    ViewBag.AccountNumber = indigenousMachineryInspectionList.LoanAcc;
                    ViewBag.InspectionId = InspectionId;
                    ViewBag.row = row;
                }
                
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.IndigenousresultViewPath + Constants.editCs, indigenousMachineryInspectionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        //Author Manoj on 25/08/2022

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmDchgIndigenousInspectionDTO indigenousMachineryInspection)
        {
            try
            {
                List<IdmDchgIndigenousInspectionDTO> indigenousMachineryInspectionDetails = new();

                if (_sessionManager.GetAllIndigenousMachineryInspectionList() != null)
                    indigenousMachineryInspectionDetails = _sessionManager.GetAllIndigenousMachineryInspectionList();
                IdmDchgIndigenousInspectionDTO indigenousMachineryInspectionExist = indigenousMachineryInspectionDetails.Find(x => x.UniqueId == id);
                if (indigenousMachineryInspectionExist != null)
                {

                    indigenousMachineryInspectionDetails.Remove(indigenousMachineryInspectionExist);
                    var list = indigenousMachineryInspectionExist;
                    list.LoanAcc = indigenousMachineryInspection.LoanAcc;
                    list.LoanSub = indigenousMachineryInspection.LoanSub;
                    list.OffcCd = indigenousMachineryInspection.OffcCd;
                    list.ItemDetails = indigenousMachineryInspection.ItemDetails;
                    list.SupplierName = indigenousMachineryInspection.SupplierName;
                    list.SupplierAddress1 = indigenousMachineryInspection.SupplierAddress1;
                    list.Quantity = indigenousMachineryInspection.Quantity;
                    list.Cost = indigenousMachineryInspection.Cost;
                    list.Tax = indigenousMachineryInspection.Tax;
                    list.TotalCost = indigenousMachineryInspection.TotalCost;
                    list.ActualCost = indigenousMachineryInspection.ActualCost;
                    list.MachineryStatus = indigenousMachineryInspection.MachineryStatus;
                    list.AquiredStatus = indigenousMachineryInspection.AquiredStatus;
                    list.SecurityRelease = indigenousMachineryInspection.SecurityRelease;
                    list.SecurityCreated = indigenousMachineryInspection.SecurityCreated;
                    list.SecurityEligibility = indigenousMachineryInspection.SecurityEligibility;
                    list.IrPlmcId = indigenousMachineryInspection.IrPlmcId;
                    list.IrPlmcSecAmt = indigenousMachineryInspection.IrPlmcSecAmt;
                    list.IrPlmcAamt = indigenousMachineryInspection.IrPlmcAamt;
                    list.IrPlmcTotalRelease = indigenousMachineryInspection.IrPlmcTotalRelease;
                    list.IrPlmcRelseStat = Convert.ToInt32(indigenousMachineryInspection.IrPlmcRelseStat);
                    list.RequestDate = indigenousMachineryInspection.RequestDate;
                    list.StatusChangedDate = indigenousMachineryInspection.StatusChangedDate;
                    list.Status = indigenousMachineryInspection.Status;
                    list.InvoiceNo = indigenousMachineryInspection.InvoiceNo;
                    list.InvoiceDate = indigenousMachineryInspection.InvoiceDate;
                    list.Delivery = indigenousMachineryInspection.Delivery;
                    list.Ino = indigenousMachineryInspection.Ino;
                    list.IsActive = true;
                    list.IsDeleted = false;
                    list.RegisteredState = indigenousMachineryInspection.RegisteredState;
                    list.UniqueId = Guid.NewGuid().ToString();
                    if (indigenousMachineryInspectionExist.Id > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    indigenousMachineryInspectionDetails.Add(list);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.InspectionId = list.Ino;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    ViewBag.InspectionId = list.Ino;
                    ViewBag.firstindigenousMachine = true;

                    var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                    ViewBag.inspectiondetails = inspectiondetailslist;
                      _sessionManager.SetIndigenousMachineryInspectionList(indigenousMachineryInspectionDetails);

                    List<IdmDchgIndigenousInspectionDTO> activeList = indigenousMachineryInspectionDetails.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                 
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.IndigenousviewPath + Constants.ViewAll, indigenousMachineryInspectionDetails) });
                }

                ViewBag.AccountNumber = indigenousMachineryInspection.LoanAcc;
                ViewBag.InspectionId = indigenousMachineryInspection.Ino;
                ViewBag.LoanSub = indigenousMachineryInspection.LoanSub;
                ViewBag.OffcCd = indigenousMachineryInspection.OffcCd;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.IndigenousviewPath + Constants.Edit, indigenousMachineryInspection) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.UpdateErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        //Author Manoj on 25/08/2022
        [HttpPost]
        public IActionResult Delete(long InspectionId, string id)
        {
            try
            {
                _logger.Information(string.Format(CommonLogHelpers.DeleteStarted, id));
                IEnumerable<IdmDchgIndigenousInspectionDTO> activeIndigenousMach = new List<IdmDchgIndigenousInspectionDTO>();
                var indigenousMachineryInspectionList = JsonConvert.DeserializeObject<List<IdmDchgIndigenousInspectionDTO>>(HttpContext.Session.GetString(Constants.SessionAllIndigenousMachineryInspectionList));
                var itemToRemove = indigenousMachineryInspectionList.Find(r => r.UniqueId == id);
                itemToRemove.Action = (int)Constant.Delete;
                itemToRemove.IsDeleted = true;
                itemToRemove.IsActive = false;
                indigenousMachineryInspectionList.Add(itemToRemove);
                _sessionManager.SetIndigenousMachineryInspectionList(indigenousMachineryInspectionList);
                if (indigenousMachineryInspectionList.Count != 0)
                {
                    activeIndigenousMach = indigenousMachineryInspectionList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                }
                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.InspectionId = itemToRemove.Ino;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                ViewBag.InspectionId = InspectionId;
                ViewBag.firstindigenousMachine = true;
                var inspectiondetailslist = _sessionManager.GetAllInspectionDetail();
                ViewBag.inspectiondetails = inspectiondetailslist;
                _logger.Information(string.Format(CommonLogHelpers.DeleteCompleted, id));
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.IndigenousviewPath + Constants.ViewAll, activeIndigenousMach) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
