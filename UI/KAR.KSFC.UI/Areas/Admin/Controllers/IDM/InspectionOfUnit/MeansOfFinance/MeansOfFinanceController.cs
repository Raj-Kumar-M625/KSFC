using KAR.KSFC.UI.Helpers;
using KAR.KSFC.UI.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using KAR.KSFC.Components.Common.Logging.Client;
using System.Linq;
using KAR.KSFC.Components.Common.Dto.IDM.InspectionOfUnit;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using KAR.KSFC.Components.Common.Dto.Enums;
using KAR.KSFC.UI.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IInspectionOfUnitService;
using KAR.KSFC.Components.Common.Dto.EnquirySubmission.Masters;
using KAR.KSFC.UI.Services.IServices.Admin.IDM.IUnitDetailsService;

namespace KAR.KSFC.UI.Areas.Admin.Controllers.IDM.MeansOfFinance
{
    [Area(Constants.Admin)]
    [Authorize(Roles = RolesEnum.Employee)]

    public class MeansOfFinanceController : Controller
    {
        private readonly IUnitDetailsService _unitDetailsService;
        private readonly ILogger _logger;
        private readonly SessionManager _sessionManager;

        public MeansOfFinanceController(ILogger logger, SessionManager sessionManager, IUnitDetailsService UnitDetailsService)
        {
            _unitDetailsService = UnitDetailsService;
            _logger = logger;
            _sessionManager = sessionManager;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create(long accountNumber, byte OffcCd, string LoanSub)
        {
            try
            {
                var financeCategory = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("SessionDDLofFinanceCategory"));
                ViewBag.Category = financeCategory;
                ViewBag.AccountNumber = accountNumber;
                ViewBag.LoanSub = LoanSub;
                ViewBag.OffcCd = OffcCd;

                return View(Constants.MOFresultViewPath + Constants.createCS);
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }

        }
        [HttpPost]
        public IActionResult Create(IdmDchgMeansOfFinanceDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    List<IdmDchgMeansOfFinanceDTO> meansOfFinanceList = new();

                    if (_sessionManager.GetAllMeansOfFinanceList().Count != 0)
                        meansOfFinanceList = _sessionManager.GetAllMeansOfFinanceList();

                    IdmDchgMeansOfFinanceDTO list = new IdmDchgMeansOfFinanceDTO();

                    list.LoanAcc = model.LoanAcc;
                    list.LoanSub=model.LoanSub;
                    list.OffcCd = model.OffcCd;
                    list.Action = (int)Constant.Create;
                    list.LoanAcc = model.LoanAcc;
                    list.DcmfCd = model.DcmfCd;
                    list.FinanceType = model.FinanceType;
                    var allcategory = _sessionManager.GetAllFinanceCategoryList();
                    var FinanceCategory = allcategory.Where(x => x.Value == model.DcmfCd.ToString());
                    list.Category = FinanceCategory.First().Text;
                    list.DcmfAmt = model.DcmfAmt;
                    list.DcmfMfType = model.DcmfMfType;
                    list.IsActive = true;
                    list.CreatedDate = DateTime.Now;
                    list.IsDeleted = false;
                    list.UniqueId = Guid.NewGuid().ToString();
                    meansOfFinanceList.Add(list);
                    _sessionManager.SetMeansOfFinanceList(meansOfFinanceList);
                    List<IdmDchgMeansOfFinanceDTO> activeList = meansOfFinanceList.Where(x => x.IsDeleted == false && x.IsActive == true).ToList();
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;
                    return Json(new { isValid = true,data=list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.MOFviewPath + Constants.ViewAll, activeList) });
                }
                ViewBag.LoanSub = model.LoanSub;
                ViewBag.OffcCd = model.OffcCd;
                ViewBag.AccountNumber = model.LoanAcc;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.MOFviewPath + Constants.Create, model) });
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.CreateErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpGet]
        public IActionResult ViewRecord(string id = "")
        {
            try
            {
                _logger.Information(CommonLogHelpers.ViewRecordStarted + id);
                var allMeansOfFinanceList = _sessionManager.GetAllMeansOfFinanceList();
                IdmDchgMeansOfFinanceDTO meansOfFinanceList = allMeansOfFinanceList.FirstOrDefault(x => x.UniqueId == id);
                var financeCategory = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("SessionDDLofFinanceCategory"));
                ViewBag.Category = financeCategory;
                _logger.Information(CommonLogHelpers.ViewRecordCompleted + id);
                return View(Constants.MOFresultViewPath + Constants.ViewRecord, meansOfFinanceList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpGet]
        public IActionResult Edit(string id = "")
        {
            try
            {

                var allMeansOfFinanceList = _sessionManager.GetAllMeansOfFinanceList();

                IdmDchgMeansOfFinanceDTO buildingInspectionList = allMeansOfFinanceList.FirstOrDefault(x => x.UniqueId == id);
                var financeCategory = JsonConvert.DeserializeObject<List<SelectListItem>>(HttpContext.Session.GetString("SessionDDLofFinanceCategory"));
                ViewBag.Category = financeCategory;
                if (buildingInspectionList != null)
                {
                    ViewBag.AccountNumber = buildingInspectionList.LoanAcc;
                    ViewBag.LoanSub = buildingInspectionList.LoanSub;
                    ViewBag.OffcCd = buildingInspectionList.OffcCd;
                }
               
                return View(Constants.MOFresultViewPath + Constants.editCs, buildingInspectionList);

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.ViewRecordErrorMsg + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, IdmDchgMeansOfFinanceDTO model)
        {
            try
            {

                List<IdmDchgMeansOfFinanceDTO> meansOfFinanceList = _sessionManager.GetAllMeansOfFinanceList();
                List<IdmDchgMeansOfFinanceDTO> activeList = new();
                IdmDchgMeansOfFinanceDTO meansOfFinanceExist = meansOfFinanceList.Find(x => x.UniqueId == id);
                if (meansOfFinanceExist != null)
                {

                    meansOfFinanceList.Remove(meansOfFinanceExist);
                    var list = meansOfFinanceExist;
                   
                    list.LoanAcc = model.LoanAcc;
                    list.LoanSub=model.LoanSub;
                    list.OffcCd=model.OffcCd;
                    list.DcmfAmt = model.DcmfAmt;
                    list.DcmfCd = model.DcmfCd;
                    list.DcmfMfType = model.DcmfMfType;
                    list.ModifiedDate = DateTime.Now;
                    list.UniqueId = Guid.NewGuid().ToString();
                    list.IsActive = true;
                    list.IsDeleted = false;

                    if (meansOfFinanceExist.DcmfRowId > 0)
                    {
                        list.Action = (int)Constant.Update;
                    }
                    else
                    {
                        list.Action = (int)Constant.Create;

                    }

                    meansOfFinanceList.Add(list);
                    ViewBag.AccountNumber = list.LoanAcc;
                    ViewBag.LoanSub = list.LoanSub;
                    ViewBag.OffcCd = list.OffcCd;

                    _sessionManager.SetMeansOfFinanceList(meansOfFinanceList);
                    if (meansOfFinanceList.Count != 0)
                    {
                        activeList = meansOfFinanceList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList();
                    }
                    
                    return Json(new { isValid = true, data = list.LoanAcc, html = Helper.RenderRazorViewToString(this, Constants.MOFviewPath + Constants.ViewAll, activeList) });
                }
                ViewBag.AccountNumber = model.LoanAcc;
                ViewBag.LoanSub = model.LoanSub;
                ViewBag.OffcCd = model.OffcCd;
                return Json(new { isValid = false, html = Helper.RenderRazorViewToString(this, Constants.MOFviewPath + Constants.Edit, model) });
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
                List<IdmDchgMeansOfFinanceDTO> activeMeansOfFinanceList = new();

                var meansOfFinanceList = JsonConvert.DeserializeObject<List<IdmDchgMeansOfFinanceDTO>>(HttpContext.Session.GetString(Constants.SessionAllMeansOfFinanceList));
                var itemToRemove = meansOfFinanceList.Find(r => r.UniqueId == id);
                itemToRemove.Action = (int)Constant.Delete;
                itemToRemove.IsDeleted = true;
                itemToRemove.IsActive = false;

                meansOfFinanceList.Add(itemToRemove);

                _sessionManager.SetMeansOfFinanceList(meansOfFinanceList);

                if (meansOfFinanceList.Count != 0)
                {
                    activeMeansOfFinanceList = (meansOfFinanceList.Where(x => x.IsActive == true && x.IsDeleted == false).ToList());
                }

                ViewBag.AccountNumber = itemToRemove.LoanAcc;
                ViewBag.LoanSub = itemToRemove.LoanSub;
                ViewBag.OffcCd = itemToRemove.OffcCd;
                return Json(new { isValid = true, html = Helper.RenderRazorViewToString(this, Constants.MOFviewPath + Constants.ViewAll, activeMeansOfFinanceList) });

            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }

        public async Task<IActionResult> FinanceType(int Id)
        {
            try
            {
               var AllFinancetype = await _unitDetailsService.GetFinanceTypeAsync();  // FROM US#05
                var Financetype = AllFinancetype.Select(x => new { x.Value, x.Text });
                return Json(new SelectList(Financetype,"Value", "Text"));
            }
            catch (System.Exception ex)
            {
                _logger.Error(Error.DeleteErrorMsgPost + ex.Message + Environment.NewLine + Error.Stack + ex.StackTrace);
                return View(Error.ErrorPath);
            }
        }
    }
}
